using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Data.Entity.Notification;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_UpdateNotificationAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task Updates_WhenFound_DeletesOldUserNoti_RecreatesByAudience()
        {
            var existing = new Notification { Id = 10, Title = "Old", Content = "OldC", Type = "info", Audience = "renters" };
            Repo.Setup(r => r.GetNotificationByIdAsync(10)).ReturnsAsync(existing);
            Repo.Setup(r => r.DeleteUserNotificationsByNotificationIdAsync(10)).ReturnsAsync(true);

            // update -> repo.UpdateNotificationAsync trả về bản đã cập nhật
            Repo.Setup(r => r.UpdateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync((Notification n) => n);

            // audience renters => load users
            var renters = new List<ApplicationUser> { new ApplicationUser { Id = 1 }, new ApplicationUser { Id = 2 } };
            Repo.Setup(r => r.GetUsersByAudienceAsync("renters")).ReturnsAsync(renters);

            // tạo user notifications
            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .ReturnsAsync(true);

            var dto = new UpdateNotificationDTO
            {
                Id = 10,
                Title = "New",
                Content = "NewC",
                Type = "warn",
                Audience = "renters"
            };

            var result = await Svc.UpdateNotificationAsync(dto);

            Assert.AreEqual(10, result.Id);
            Assert.AreEqual("New", result.Title);
            Assert.AreEqual("warn", result.Type);

            Repo.Verify(r => r.GetNotificationByIdAsync(10), Times.Once);
            Repo.Verify(r => r.DeleteUserNotificationsByNotificationIdAsync(10), Times.Once);
            Repo.Verify(r => r.UpdateNotificationAsync(It.Is<Notification>(n =>
                n.Id == 10 && n.Title == "New" && n.Content == "NewC" && n.Type == "warn" && n.Audience == "renters"
            )), Times.Once);
            Repo.Verify(r => r.GetUsersByAudienceAsync("renters"), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.Is<List<ApplicationUserNotification>>(lst =>
                lst.Count == 2 && lst.All(x => x.NotificationId == 10) &&
                lst.Select(x => x.UserId).OrderBy(i => i).SequenceEqual(new[] { 1, 2 })
            )), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Throws_When_NotFound()
        {
            Repo.Setup(r => r.GetNotificationByIdAsync(99)).ReturnsAsync((Notification)null!);

            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                Svc.UpdateNotificationAsync(new UpdateNotificationDTO { Id = 99, Title = "x" }));

            StringAssert.Contains(ex.Message, "Notification not found");
            Repo.Verify(r => r.GetNotificationByIdAsync(99), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
        [TestMethod]
        public async Task Update_AllAudience_DispatchesToAllUsers()
        {
            // Arrange
            var existing = new Notification { Id = 21, Title = "Old", Content = "OldC", Type = "info", Audience = "all" };
            Repo.Setup(r => r.GetNotificationByIdAsync(21)).ReturnsAsync(existing);
            Repo.Setup(r => r.DeleteUserNotificationsByNotificationIdAsync(21)).ReturnsAsync(true);
            Repo.Setup(r => r.UpdateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync((Notification n) => n);

            var users = new List<ApplicationUser>
    {
        new ApplicationUser { Id = 1, UserName = "u1" },
        new ApplicationUser { Id = 2, UserName = "u2" },
        new ApplicationUser { Id = 3, UserName = "u3" }
    };
            Repo.Setup(r => r.GetUsersByAudienceAsync("all")).ReturnsAsync(users);

            List<ApplicationUserNotification>? captured = null;
            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .Callback<List<ApplicationUserNotification>>(lst => captured = lst)
                .ReturnsAsync(true);

            var dto = new UpdateNotificationDTO
            {
                Id = 21,
                Title = "NewAll",
                Content = "NewAllC",
                Type = "info",
                Audience = "all"
            };

            // Act
            var result = await Svc.UpdateNotificationAsync(dto);

            // Assert
            Assert.AreEqual(21, result.Id);
            Assert.AreEqual("NewAll", result.Title);
            Assert.IsNotNull(captured);
            Assert.AreEqual(3, captured!.Count);
            CollectionAssert.AreEquivalent(new[] { 1, 2, 3 }, captured.Select(x => x.UserId).ToArray());

            Repo.Verify(r => r.GetNotificationByIdAsync(21), Times.Once);
            Repo.Verify(r => r.DeleteUserNotificationsByNotificationIdAsync(21), Times.Once);
            Repo.Verify(r => r.UpdateNotificationAsync(It.Is<Notification>(n =>
                n.Id == 21 && n.Title == "NewAll" && n.Content == "NewAllC" && n.Audience == "all"
            )), Times.Once);
            Repo.Verify(r => r.GetUsersByAudienceAsync("all"), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Update_RentersAudience_DispatchesToRenters()
        {
            // Arrange
            var existing = new Notification { Id = 22, Title = "Old", Content = "OldC", Type = "warn", Audience = "renters" };
            Repo.Setup(r => r.GetNotificationByIdAsync(22)).ReturnsAsync(existing);
            Repo.Setup(r => r.DeleteUserNotificationsByNotificationIdAsync(22)).ReturnsAsync(true);
            Repo.Setup(r => r.UpdateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync((Notification n) => n);

            var renters = new List<ApplicationUser>
    {
        new ApplicationUser { Id = 10, UserName = "r1" },
        new ApplicationUser { Id = 11, UserName = "r2" }
    };
            Repo.Setup(r => r.GetUsersByAudienceAsync("renters")).ReturnsAsync(renters);

            List<ApplicationUserNotification>? captured = null;
            Repo.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .Callback<List<ApplicationUserNotification>>(lst => captured = lst)
                .ReturnsAsync(true);

            var dto = new UpdateNotificationDTO
            {
                Id = 22,
                Title = "ToRenters",
                Content = "Body",
                Type = "warn",
                Audience = "renters"
            };

            // Act
            var result = await Svc.UpdateNotificationAsync(dto);

            // Assert
            Assert.AreEqual(22, result.Id);
            Assert.AreEqual("ToRenters", result.Title);
            Assert.IsNotNull(captured);
            CollectionAssert.AreEquivalent(new[] { 10, 11 }, captured!.Select(x => x.UserId).ToArray());

            Repo.Verify(r => r.GetNotificationByIdAsync(22), Times.Once);
            Repo.Verify(r => r.DeleteUserNotificationsByNotificationIdAsync(22), Times.Once);
            Repo.Verify(r => r.UpdateNotificationAsync(It.Is<Notification>(n =>
                n.Id == 22 && n.Title == "ToRenters" && n.Audience == "renters"
            )), Times.Once);
            Repo.Verify(r => r.GetUsersByAudienceAsync("renters"), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Update_SpecificAudience_EmptyIds_NoUserNotificationsCreated()
        {
            // Arrange
            var existing = new Notification { Id = 23, Title = "Old", Content = "OldC", Type = "info", Audience = "specific" };
            Repo.Setup(r => r.GetNotificationByIdAsync(23)).ReturnsAsync(existing);
            Repo.Setup(r => r.DeleteUserNotificationsByNotificationIdAsync(23)).ReturnsAsync(true);
            Repo.Setup(r => r.UpdateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync((Notification n) => n);

            // Empty IDs -> repo returns empty users
            Repo.Setup(r => r.GetUsersByIdsAsync(
                    It.Is<List<int>>(ids => ids.Count == 0)
                ))
                .ReturnsAsync(new List<ApplicationUser>());

            var dto = new UpdateNotificationDTO
            {
                Id = 23,
                Title = "NoIds",
                Content = "C",
                Type = "info",
                Audience = "specific",
                SpecificUserIds = new List<int>() // empty
            };

            // Act
            var result = await Svc.UpdateNotificationAsync(dto);

            // Assert
            Assert.AreEqual(23, result.Id);
            Assert.AreEqual("NoIds", result.Title);

            Repo.Verify(r => r.GetNotificationByIdAsync(23), Times.Once);
            Repo.Verify(r => r.DeleteUserNotificationsByNotificationIdAsync(23), Times.Once);
            Repo.Verify(r => r.UpdateNotificationAsync(It.Is<Notification>(n =>
                n.Id == 23 && n.Audience == "specific"
            )), Times.Once);
            Repo.Verify(r => r.GetUsersByIdsAsync(
                It.Is<List<int>>(ids => ids.SequenceEqual(dto.SpecificUserIds)) // [] == []
            ), Times.Once);
            Repo.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Never);
            Repo.VerifyNoOtherCalls();
        }

    }
}
