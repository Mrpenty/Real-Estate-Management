using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.Notification;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_GetNotificationsByAudienceAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task FiltersByAudience_AndMaps()
        {
            var list = new List<Notification>
            {
                new Notification { Id=10, Title="R1", Audience="renters" }
            };
            Repo.Setup(r => r.GetNotificationsByAudienceAsync("renters"))
                .ReturnsAsync(list);

            var dtos = (await Svc.GetNotificationsByAudienceAsync("renters")).ToList();

            Assert.AreEqual(1, dtos.Count);
            Assert.AreEqual(10, dtos[0].Id);
            Assert.AreEqual("renters", dtos[0].Audience);

            Repo.Verify(r => r.GetNotificationsByAudienceAsync("renters"), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
