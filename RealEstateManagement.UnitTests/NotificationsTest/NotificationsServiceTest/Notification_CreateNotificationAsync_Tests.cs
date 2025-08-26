using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Data.Entity.Notification;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_CreateNotificationAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task Creates_And_Dispatches_To_All_Audience()
        {
            var dto = new CreateNotificationDTO { Title = "T", Content = "C", Type = "info", Audience = "all" };

            var created = new Notification { Id = 10, Title = "T", Content = "C", Type = "info", Audience = "all", CreatedAt = DateTime.UtcNow };

            var users = new List<ApplicationUser>
    {
        new ApplicationUser{ Id = 1, UserName="u1"},
        new ApplicationUser{ Id = 2, UserName="u2"}
    };

            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(created);

            Repo.Setup(r => r.GetUsersByAudienceAsync("all"))
                .ReturnsAsync(users);

            List<ApplicationUserNotification> captured = null;
            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .Callback<List<ApplicationUserNotification>>(x => captured = x)
                .ReturnsAsync(true);

            var result = await Svc.CreateNotificationAsync(dto);

            Assert.AreEqual(10, result.Id);
            Assert.AreEqual("T", result.Title);
            Assert.IsNotNull(captured);
            Assert.AreEqual(2, captured.Count);
            Assert.IsTrue(captured.All(x => x.NotificationId == 10));
            CollectionAssert.AreEquivalent(new[] { 1, 2 }, captured.Select(x => x.UserId).ToArray());

            Repo.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
            Repo.Verify(r => r.GetUsersByAudienceAsync("all"), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
            Repo.VerifyNoOtherCalls();
        }


        [TestMethod]
        public async Task Creates_SpecificUsers_AllIdsExist()
        {
            var dto = new CreateNotificationDTO
            {
                Title = "T",
                Content = "C",
                Type = "warn",
                Audience = "specific",
                SpecificUserIds = new List<int> { 5, 7 }
            };

            var created = new Notification { Id = 99, Title = "T", Content = "C", Type = "warn", Audience = "specific" };
            Repo.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(created);

            var foundUsers = new List<ApplicationUser>
    {
        new ApplicationUser { Id = 5 },
        new ApplicationUser { Id = 7 }
    };
            Repo.Setup(r => r.GetUsersByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(foundUsers);

            List<ApplicationUserNotification> captured = null;
            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .Callback<List<ApplicationUserNotification>>(x => captured = x)
                .ReturnsAsync(true);

            var result = await Svc.CreateNotificationAsync(dto);

            Assert.AreEqual(99, result.Id);
            Assert.IsNotNull(captured);
            CollectionAssert.AreEquivalent(new[] { 5, 7 }, captured.Select(u => u.UserId).ToArray());

            Repo.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
            Repo.Verify(r => r.GetUsersByIdsAsync(
                It.Is<List<int>>(ids => ids.SequenceEqual(new List<int> { 5, 7 }))
            ), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
            Repo.VerifyNoOtherCalls();
        }


        [TestMethod]
        public async Task Creates_SpecificUsers_Throws_When_SomeIdsNotFound()
        {
            // Arrange
            var dto = new CreateNotificationDTO
            {
                Title = "T",
                Content = "C",
                Type = "warn",
                Audience = "specific",
                SpecificUserIds = new List<int> { 1, 2, 3 }
            };

            // Repo chỉ trả về user 1 và 3 -> thiếu 2 => phải throw, không được tạo notification/user-notifications
            Repo.Setup(r => r.GetUsersByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<ApplicationUser>
                {
                    new ApplicationUser { Id = 1 },
                    new ApplicationUser { Id = 3 }
                });

            // Act + Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => Svc.SendNotificationToSpecificUsersAsync(dto));

            Repo.Verify(r => r.GetUsersByIdsAsync(It.Is<List<int>>(ids => ids.SequenceEqual(new List<int> { 1, 2, 3 }))), Times.Once);
            Repo.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Never);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Never);
            Repo.VerifyNoOtherCalls();
        }
    }
}
