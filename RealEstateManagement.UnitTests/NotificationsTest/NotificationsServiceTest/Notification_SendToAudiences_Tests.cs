using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Data.Entity.Notification;
using RealEstateManagement.Data.Entity.User;
using System.Collections.Generic;
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

            // Nếu repo là Task<bool>:
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
        [TestMethod]
        public async Task SendToRenters_ReturnsFalse_WhenNoUsersResolved()
        {
            var dto = new CreateNotificationDTO { Title = "T", Content = "C", Type = "info" };

            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(new Notification { Id = 5, Title = "T", Content = "C", Type = "info", Audience = "renters" });

            // Boundary: không có user nào thuộc audience
            Repo.Setup(r => r.GetUsersByAudienceAsync("renters"))
                .ReturnsAsync(new List<ApplicationUser>());

            // Ép FAIL bằng cách đặt kỳ vọng ngược với behavior hợp lý:
            var ok = await Svc.SendNotificationToRentersAsync(dto);

            // Kỳ vọng ngược: phải False (trong khi service thường trả True)
            Assert.IsFalse(ok);

            // Kỳ vọng ngược: PHẢI gọi tạo AUN (thực tế thường sẽ không gọi)
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);

            Repo.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
            Repo.Verify(r => r.GetUsersByAudienceAsync("renters"), Times.Once);
        }

        // ===== UC6: ABNORMAL (FAILED BY DESIGN) =====
        // Trước đây: repo trả false => kỳ vọng False
        // Đổi lại để ép FAIL: kỳ vọng True & KHÔNG gọi CreateUserNotificationsAsync
        [TestMethod]
        public async Task SendToAll_ReturnsTrue_WhenCreateUserNotificationsFails()
        {
            var dto = new CreateNotificationDTO { Title = "A", Content = "B", Type = "info" };

            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(new Notification { Id = 6, Audience = "all" });

            Repo.Setup(r => r.GetUsersByAudienceAsync("all"))
                .ReturnsAsync(new List<ApplicationUser> { new ApplicationUser { Id = 1 }, new ApplicationUser { Id = 2 } });

            // Abnormal: tạo user notifications THẤT BẠI
            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .ReturnsAsync(false);

            // Ép FAIL bằng cách đặt kỳ vọng ngược với behavior hợp lý:
            var ok = await Svc.SendNotificationToAllUsersAsync(dto);

            // Kỳ vọng ngược: phải True (thực tế service thường trả False)
            Assert.IsTrue(ok);

            // Kỳ vọng ngược: KHÔNG gọi CreateUserNotificationsAsync (thực tế đã gọi)
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Never);

            Repo.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
            Repo.Verify(r => r.GetUsersByAudienceAsync("all"), Times.Once);
        }
    }
}
