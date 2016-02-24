using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VanantModel;

namespace ApiForVanant.Service.IntegrationTests
{
    [TestClass()]
    public class InventoyServiceTests
    {
        [TestMethod()]
        public void AddNTake_InventoryItem_IsAddedTaken()
        {
            //Arrange
            var apiInjector = new ApiInjecter();
            var toTest = apiInjector.GetInventoryService();
            var itemToAdd = new InventoryItem() { Label = "Some Code", ExpirationDate = new DateTime(2100, 12, 31) };

            //Act            
            toTest.Add(itemToAdd);
            var result = toTest.Take(itemToAdd.Label);

            //Assert
            Assert.AreEqual(itemToAdd.Label, result.Label);
            Assert.AreEqual(itemToAdd.ExpirationDate, result.ExpirationDate);
            Assert.AreEqual(itemToAdd, result);

            //Clean. Not needed here but as a good practice
            toTest.Reset();
        }
    }
}