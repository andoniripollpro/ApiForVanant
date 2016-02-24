using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ApiForVanant.IntegrationTests
{
    [TestClass()]
    public class ApiInjecterTests
    {
        [TestMethod()]
        public void GetInventoryService_NoParam_ServiceInjected()
        {
            //Arrange
            var toTest = new ApiInjecter();

            //Act
            var result = toTest.GetInventoryService();
            var item = result.Take("Whatever"); //This only checks if any error injecting provokes failure

            //Assert
            Assert.IsNotNull(result);

            //Clean. Not needed here but as a good practice
            result.Reset();
        }
    }
}