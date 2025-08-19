using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.Notification;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_GetUserNotificationsAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task MapsUserNotifications_Correctly()
        {
            var items = new List<ApplicationUserNotification>
            {
                new ApplicationUserNotification
                {
                    NotificationId = 5,
                    UserId = 1,
                    IsRead = false,
                    Notification = new Notification
                    {
                        Id=5, Title="T", Content="C", Type="info", CreatedAt=DateTime.UtcNow
                    }
                }
            };

            Repo.Setup(r => r.GetUserNotificationsAsync(1)).ReturnsAsync(items);

            var dtos = (await Svc.GetUserNotificationsAsync(1)).ToList();

            Assert.AreEqual(1, dtos.Count);
            Assert.AreEqual(5, dtos[0].NotificationId);
            Assert.AreEqual("T", dtos[0].Title);
            Assert.IsFalse(dtos[0].IsRead);

            Repo.Verify(r => r.GetUserNotificationsAsync(1), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
