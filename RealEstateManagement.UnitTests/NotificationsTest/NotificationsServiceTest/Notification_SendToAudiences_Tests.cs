using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Data.Entity.Notification;
using RealEstateManagement.Data.Entity.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_SendToAudiences_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task SendToAll_Works()
        {
            var dto = new CreateNotificationDTO { Title = "A", Content = "B", Type = "info" };

            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(new Notification { Id = 1, Title = "A", Content = "B", Type = "info", Audience = "all" });

            Repo.Setup(r => r.GetUsersByAudienceAsync("all"))
                .ReturnsAsync(new List<ApplicationUser> { new ApplicationUser { Id = 1 } });

            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .ReturnsAsync(true);

            var ok = await Svc.SendNotificationToAllUsersAsync(dto);

            Assert.IsTrue(ok);
            Repo.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
            Repo.Verify(r => r.GetUsersByAudienceAsync("all"), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task SendToRenters_Works()
        {
            var dto = new CreateNotificationDTO { Title = "T", Content = "C", Type = "info" };

            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(new Notification { Id = 2, Title = "T", Content = "C", Type = "info", Audience = "renters" });

            Repo.Setup(r => r.GetUsersByAudienceAsync("renters"))
                .ReturnsAsync(new List<ApplicationUser> { new ApplicationUser { Id = 9 } });

            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .ReturnsAsync(true);

            var ok = await Svc.SendNotificationToRentersAsync(dto);

            Assert.IsTrue(ok);
            Repo.Verify(r => r.GetUsersByAudienceAsync("renters"), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
        }

        [TestMethod]
        public async Task SendToLandlords_Works()
        {
            var dto = new CreateNotificationDTO { Title = "T", Content = "C", Type = "info" };

            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(new Notification { Id = 3 });

            Repo.Setup(r => r.GetUsersByAudienceAsync("landlords"))
                .ReturnsAsync(new List<ApplicationUser> { new ApplicationUser { Id = 7 } });

            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .ReturnsAsync(true);

            var ok = await Svc.SendNotificationToLandlordsAsync(dto);

            Assert.IsTrue(ok);
            Repo.Verify(r => r.GetUsersByAudienceAsync("landlords"), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
        }

        [TestMethod]
        public async Task SendToAdmins_Works()
        {
            var dto = new CreateNotificationDTO { Title = "T", Content = "C", Type = "info" };

            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(new Notification { Id = 4 });

            Repo.Setup(r => r.GetUsersByAudienceAsync("admin"))
                .ReturnsAsync(new List<ApplicationUser> { new ApplicationUser { Id = 1 } });

            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .ReturnsAsync(true);

            var ok = await Svc.SendNotificationToAdminsAsync(dto);

            Assert.IsTrue(ok);
            Repo.Verify(r => r.GetUsersByAudienceAsync("admin"), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
        }

        // ✅ Sửa: Không có user → trả true & KHÔNG gọi CreateUserNotificationsAsync
        [TestMethod]
        public async Task SendToRenters_ReturnsTrue_WhenNoUsersResolved_DoesNotCreateUserNotifications()
        {
            var dto = new CreateNotificationDTO { Title = "T", Content = "C", Type = "info" };

            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(new Notification { Id = 5, Title = "T", Content = "C", Type = "info", Audience = "renters" });

            Repo.Setup(r => r.GetUsersByAudienceAsync("renters"))
                .ReturnsAsync(new List<ApplicationUser>()); // không có user

            var ok = await Svc.SendNotificationToRentersAsync(dto);

            Assert.IsTrue(ok);
            Repo.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
            Repo.Verify(r => r.GetUsersByAudienceAsync("renters"), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Never);
            Repo.VerifyNoOtherCalls();
        }

        // ✅ Sửa: Tạo user-notifications thất bại → trả false & đã gọi CreateUserNotificationsAsync 1 lần
        [TestMethod]
        public async Task SendToAll_ReturnsFalse_WhenCreateUserNotificationsFails()
        {
            var dto = new CreateNotificationDTO { Title = "A", Content = "B", Type = "info" };

            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(new Notification { Id = 6, Audience = "all" });

            Repo.Setup(r => r.GetUsersByAudienceAsync("all"))
                .ReturnsAsync(new List<ApplicationUser>
                {
                    new ApplicationUser { Id = 1 },
                    new ApplicationUser { Id = 2 }
                });

            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .ReturnsAsync(false); // thất bại

            var ok = await Svc.SendNotificationToAllUsersAsync(dto);

            Assert.IsFalse(ok);
            Repo.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
            Repo.Verify(r => r.GetUsersByAudienceAsync("all"), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
