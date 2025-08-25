using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.Notification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_GetNotificationByIdAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task MapsToDto_WhenFound()
        {
            var entity = new Notification
            {
                Id = 7,
                Title = "T",
                Content = "C",
                Type = "info",
                Audience = "all",
                UserNotifications = new List<ApplicationUserNotification>
                {
                    new ApplicationUserNotification{ NotificationId=7, UserId=1 },
                    new ApplicationUserNotification{ NotificationId=7, UserId=2 },
                }
            };

            Repo.Setup(r => r.GetNotificationByIdAsync(7)).ReturnsAsync(entity);

            var dto = await Svc.GetNotificationByIdAsync(7);

            Assert.IsNotNull(dto);
            Assert.AreEqual(7, dto!.Id);
            Assert.AreEqual("T", dto.Title);
            Assert.AreEqual("info", dto.Type);
            Assert.AreEqual("all", dto.Audience);
            Assert.AreEqual(2, dto.RecipientCount); // map từ UserNotifications.Count

            Repo.Verify(r => r.GetNotificationByIdAsync(7), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task ReturnsNull_WhenNotFound()
        {
            Repo.Setup(r => r.GetNotificationByIdAsync(55)).ReturnsAsync((Notification)null!);

            var dto = await Svc.GetNotificationByIdAsync(55);

            Assert.IsNull(dto);
            Repo.Verify(r => r.GetNotificationByIdAsync(55), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task PropagatesException_WhenRepositoryThrows()
        {
            Repo.Setup(r => r.GetNotificationByIdAsync(9))
                .ThrowsAsync(new InvalidOperationException("DB err"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.GetNotificationByIdAsync(9));

            Repo.Verify(r => r.GetNotificationByIdAsync(9), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
