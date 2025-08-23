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
        [TestMethod]
        public async Task ReturnsEmpty_WhenNoUnread()
        {
            // Params
            var userId = 2;

            // Arrange
            Repo.Setup(r => r.GetUnreadUserNotificationsAsync(userId))
                .ReturnsAsync(new List<ApplicationUserNotification>());

            // Act
            var dtos = (await Svc.GetUnreadUserNotificationsAsync(userId)).ToList();

            // Assert (Return)
            Assert.IsNotNull(dtos);
            Assert.AreEqual(0, dtos.Count);

            // Verify
            Repo.Verify(r => r.GetUnreadUserNotificationsAsync(userId), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task PropagatesException_WhenRepositoryThrows()
        {
            // Params
            var userId = 3;

            // Arrange
            Repo.Setup(r => r.GetUnreadUserNotificationsAsync(userId))
                .ThrowsAsync(new InvalidOperationException("DB err")); // Log/Message

            // Act + Assert (Exception + LogMessage)
            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.GetUnreadUserNotificationsAsync(userId));

            Assert.AreEqual("DB err", ex.Message);

            // Verify
            Repo.Verify(r => r.GetUnreadUserNotificationsAsync(userId), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

    }
}
