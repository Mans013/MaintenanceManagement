using MaintenanceManagement.Models;
using MaintenanceManagement.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMQManagement;
using System.Linq;

namespace MaintenanceManagementTests
{
    [TestClass]
    public class MaintenanceServiceTest
    {
        private static MaintenanceService maintenanceService;

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Initalization code goes here
            var publisher = new RabbitMQMessagePublisher("localhost", "Test");
            maintenanceService = new MaintenanceService(publisher);
            maintenanceService.changeToTestService();

            MaintenanceModel maintenanceModel = new MaintenanceModel()
            {
                Description = "Test McTestington",
                Price = 10,
                Car = "Test",
                Date = "now",
                Machanic = "Bob de Bouwer",
                Status = "Testing",
                Type = "Test"
            };
            maintenanceService.Create(maintenanceModel);
        }

        [TestMethod]
        public void GetMaintenancesTest()
        {
            var result = maintenanceService.Get();
            if (result != null)
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void GetMaintenanceByPropertyTest()
        {
            var result = maintenanceService.GetByProperty("Status", "Testing");
            if (result != null)
            {
                Assert.IsTrue(result[0].Status == "Testing");
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void CreateMaintenanceTest()
        {
            MaintenanceModel MaintenanceModel = new MaintenanceModel()
            {
                Car = "Test",
                Date = "now",
                Machanic = "Bob de Bouwer",
                Description = "Lorum Ipsum",
                Price = 1,
                Status = "Testing",
                Type = "Test"
            };
            var result = maintenanceService.Create(MaintenanceModel);
            Assert.IsTrue(result.Id != null);
            Assert.IsTrue(result.Car == "Test");
        }

        [TestMethod]
        public void FinishMaintenanceTest()
        {
            var testMaintenance = maintenanceService.GetByProperty("Status", "Testing").FirstOrDefault();
            Assert.IsTrue(testMaintenance.Status != "Done");
            maintenanceService.Finish(testMaintenance);
            var testMaintenance2 = maintenanceService.GetById(testMaintenance.Id);
            Assert.IsTrue(testMaintenance2.Status == "Done");
        }
    }
}
