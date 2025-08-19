using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.Notification;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_GetUnreadUserNotificationsAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task MapsUnread_Correctly()
        {
            var items = new List<ApplicationUserNotification>
            {
                new ApplicationUserNotification
                {
                    NotificationId = 9, UserId=1, IsRead = false,
                    Notification = new Notification { Id=9, Title="U", Content="Body", Type="warn" }
                }
            };

            Repo.Setup(r => r.GetUnreadUserNotificationsAsync(1)).ReturnsAsync(items);

            var dtos = (await Svc.GetUnreadUserNotificationsAsync(1)).ToList();

            Assert.AreEqual(1, dtos.Count);
            Assert.AreEqual(9, dtos[0].NotificationId);
            Assert.AreEqual("U", dtos[0].Title);
            Assert.IsFalse(dtos[0].IsRead);

            Repo.Verify(r => r.GetUnreadUserNotificationsAsync(1), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
