using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApiForVanant.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanantModel;
using VanantDAL.Repository.Interface;
using Moq;
using Moq.Matchers;
using ApiForVanant.Service.Interface;

namespace ApiForVanant.Service.UnitTests
{
    [TestClass()]
    public class InventoyServiceTests
    {
        #region Add

        [TestMethod()]
        public void Add_InventoryItem_AddIsCalled()
        {
            //Arrange
            var inventoryRepositryMock = new Mock<IInventoryRepository>();
            var inventoryItemToAdd = new InventoryItem() { Label = "AnyLabelCode123", ExpirationDate = new DateTime(2100, 12, 31) };
            inventoryRepositryMock.Setup(i => i.Add(It.IsAny<InventoryItem>())).Verifiable();
            inventoryRepositryMock.Setup(i => i.GetByLessExpiration(It.IsAny<DateTime>())).Returns(new List<InventoryItem>());
            IInventoyService toTest = new InventoyService(inventoryRepositryMock.Object);

            //Act
            toTest.Add(inventoryItemToAdd);

            //Assert
            inventoryRepositryMock.Verify();
        }

        [TestMethod()]
        public void Add_InventoryItem_GetByLessExpirationIsCalled()
        {
            //Arrange
            var inventoryRepositryMock = new Mock<IInventoryRepository>();
            var inventoryItemToAdd = new InventoryItem() { Label = "AnyLabelCode123", ExpirationDate = new DateTime(2100, 12, 31) };
            inventoryRepositryMock.Setup(i => i.Add(It.IsAny<InventoryItem>()));
            inventoryRepositryMock.Setup(i => i.GetByLessExpiration(It.IsAny<DateTime>())).Returns(new List<InventoryItem>()).Verifiable();
            IInventoyService toTest = new InventoyService(inventoryRepositryMock.Object);

            //Act
            toTest.Add(inventoryItemToAdd);

            //Assert
            inventoryRepositryMock.Verify();
        }

        [TestMethod()]
        public void Add_Null_ExceptionThrown()
        {
            //Arrange
            InventoryItem inventoryItemToAdd = null;
            IInventoyService toTest = new InventoyService(null);

            //Act
            try {
                toTest.Add(inventoryItemToAdd);

                //Assert
                Assert.Fail("Wrong parameters. An exception should be thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.GetType(), typeof(ArgumentNullException));
            }            
        }

        [TestMethod()]
        public void Add_GetByLessExpirationReturnsOne_NotificationIsCalled()
        {
            //Arrange
            var inventoryRepositryMock = new Mock<IInventoryRepository>();
            var inventoryItemToExpire = new InventoryItem() { Label = "AnyLabelCode123", ExpirationDate = new DateTime(1900, 12, 31) };
            var inventoryItemToAdd = new InventoryItem() { Label = "AnyOtherLabelCode123", ExpirationDate = new DateTime(2100, 12, 31) };
            inventoryRepositryMock.Setup(i => i.Add(It.IsAny<InventoryItem>()));
            inventoryRepositryMock.Setup(i => i.GetByLessExpiration(It.IsAny<DateTime>())).Returns(new List<InventoryItem>() { inventoryItemToExpire });
            IInventoyService toTest = new InventoyService(inventoryRepositryMock.Object);
            var fakeListener = new FakeListener();
            toTest.Notify += fakeListener.NotifyMe;

            //Act
            toTest.Add(inventoryItemToAdd);

            //Assert
            Assert.IsTrue(fakeListener.Called);
            Assert.AreNotEqual(fakeListener.Message, string.Empty);
        }

        #endregion

        #region Take

        [TestMethod()]
        public void Take_Lable_GetByLabelNDeleteAreCalled()
        {
            //Arrange
            var inventoryRepositryMock = new Mock<IInventoryRepository>();
            var inventoryItemToBeTaken = new InventoryItem() { Label = "AnyLabelCode123", ExpirationDate = new DateTime(2100, 12, 31) };
            inventoryRepositryMock.Setup(i => i.GetByLabel(inventoryItemToBeTaken.Label)).Returns(inventoryItemToBeTaken).Verifiable();
            inventoryRepositryMock.Setup(i => i.DeleteByLabel(inventoryItemToBeTaken.Label)).Returns(1).Verifiable();
            inventoryRepositryMock.Setup(i => i.GetByLessExpiration(It.IsAny<DateTime>())).Returns(new List<InventoryItem>());
            IInventoyService toTest = new InventoyService(inventoryRepositryMock.Object);

            //Act
            var result = toTest.Take(inventoryItemToBeTaken.Label);

            //Assert
            inventoryRepositryMock.Verify();
            Assert.AreEqual(inventoryItemToBeTaken, result);
        }

        [TestMethod()]
        public void Take_Label_NotificationIsCalled()
        {
            //Arrange
            var inventoryRepositryMock = new Mock<IInventoryRepository>();
            var inventoryItemToTake = new InventoryItem() { Label = "AnyLabelCode123", ExpirationDate = new DateTime(2100, 12, 31) };
            inventoryRepositryMock.Setup(i => i.GetByLabel(It.IsAny<string>())).Returns(inventoryItemToTake);
            inventoryRepositryMock.Setup(i => i.DeleteByLabel(It.IsAny<string>())).Returns(1);
            inventoryRepositryMock.Setup(i => i.GetByLessExpiration(It.IsAny<DateTime>())).Returns(new List<InventoryItem>() );
            IInventoyService toTest = new InventoyService(inventoryRepositryMock.Object);
            var fakeListener = new FakeListener();
            toTest.Notify += fakeListener.NotifyMe;

            //Act
            var result = toTest.Take(inventoryItemToTake.Label);

            //Assert
            Assert.IsTrue(fakeListener.Called);
            Assert.AreNotEqual(fakeListener.Message, string.Empty);
            Assert.AreEqual(inventoryItemToTake, result);
            Assert.AreEqual(inventoryItemToTake.Label, result.Label);
        }

        [TestMethod()]
        public void Take_LabelExpiredItem_NotificationIsCalledNNullReturned()
        {
            //Arrange
            var inventoryRepositryMock = new Mock<IInventoryRepository>();
            var inventoryItemToTake = new InventoryItem() { Label = "AnyLabelCode123", ExpirationDate = new DateTime(1900, 12, 31) };
            inventoryRepositryMock.Setup(i => i.GetByLabel(It.IsAny<string>())).Returns((InventoryItem)null);
            inventoryRepositryMock.Setup(i => i.DeleteByLabel(It.IsAny<string>())).Returns(1);
            inventoryRepositryMock.Setup(i => i.GetByLessExpiration(It.IsAny<DateTime>())).Returns(new List<InventoryItem>() { inventoryItemToTake });
            IInventoyService toTest = new InventoyService(inventoryRepositryMock.Object);
            var fakeListener = new FakeListener();
            toTest.Notify += fakeListener.NotifyMe;

            //Act
            var result = toTest.Take(inventoryItemToTake.Label);

            //Assert
            Assert.IsTrue(fakeListener.Called);
            Assert.IsTrue(fakeListener.Message.Contains("expired"));
            Assert.IsFalse(fakeListener.Message.Contains("removed"));
            Assert.IsNull(result);
        }

        #endregion

        #region Fake to test notifications

        public class FakeListener
        {
            public bool Called { get; set; }
            public string Message { get; set; }

            public FakeListener()
            {
                Called = false;
                Message = string.Empty;
            }

            public void NotifyMe(string message)
            {
                this.Called = true;
                this.Message = message;
            }
        }

        #endregion
    }
}