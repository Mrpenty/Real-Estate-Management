using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.Notification;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_GetAllNotificationsAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task MapsList_WhenRepositoryReturnsItems()
        {
            var list = new List<Notification>
            {
                new Notification { Id=1, Title="A", Audience="all" },
                new Notification { Id=2, Title="B", Audience="renters" }
            };
            Repo.Setup(r => r.GetAllNotificationsAsync()).ReturnsAsync(list);

            var dtos = (await Svc.GetAllNotificationsAsync()).ToList();

            Assert.AreEqual(2, dtos.Count);
            Assert.AreEqual(1, dtos[0].Id);
            Assert.AreEqual("A", dtos[0].Title);
            Assert.AreEqual("all", dtos[0].Audience);

            Repo.Verify(r => r.GetAllNotificationsAsync(), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task ReturnsEmpty_WhenRepositoryReturnsEmpty()
        {
            Repo.Setup(r => r.GetAllNotificationsAsync()).ReturnsAsync(new List<Notification>());

            var dtos = (await Svc.GetAllNotificationsAsync()).ToList();

            Assert.AreEqual(0, dtos.Count);
            Repo.Verify(r => r.GetAllNotificationsAsync(), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
