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
    public class Notification_SendNotificationToSpecificUsersAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task Works_WhenAllIdsExist()
        {
            var dto = new CreateNotificationDTO
            {
                Title = "T",
                Content = "C",
                Type = "info",
                SpecificUserIds = new List<int> { 5, 7 }
            };

            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(new Notification { Id = 10 });

            Repo.Setup(r => r.GetUsersByIdsAsync(It.Is<List<int>>(ids => ids.SequenceEqual(new List<int> { 5, 7 }))))
                .ReturnsAsync(new List<ApplicationUser> { new ApplicationUser { Id = 5 }, new ApplicationUser { Id = 7 } });

            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .ReturnsAsync(true);

            var ok = await Svc.SendNotificationToSpecificUsersAsync(dto);

            Assert.IsTrue(ok);
            Repo.Verify(
                r => r.GetUsersByIdsAsync(
                    It.Is<List<int>>(ids => ids.SequenceEqual(new List<int> { 5, 7 }))
                ),
                Times.Exactly(2)   // <-- gọi 2 lần: validate + tạo notification
            );

            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
        }

        [TestMethod]
        public async Task Throws_WhenMissingIds()
        {
            var dto = new CreateNotificationDTO
            {
                Title = "T",
                Content = "C",
                Type = "info",
                SpecificUserIds = new List<int> { 1, 2, 3 }
            };

            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(new Notification { Id = 11 });

            Repo.Setup(r => r.GetUsersByIdsAsync(It.Is<List<int>>(ids => ids.SequenceEqual(new List<int> { 1, 2, 3 }))))
                .ReturnsAsync(new List<ApplicationUser> { new ApplicationUser { Id = 1 }, new ApplicationUser { Id = 3 } });

            await Assert.ThrowsExceptionAsync<ArgumentException>(
                () => Svc.SendNotificationToSpecificUsersAsync(dto));

            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Never);
        }
    }
}
