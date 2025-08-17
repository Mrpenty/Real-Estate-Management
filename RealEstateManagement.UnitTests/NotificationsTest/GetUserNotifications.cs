using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Business.Repositories.NotificationRepository;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Data.Entity.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RealEstateManagement.UnitTests.NotificationsTest
{
    [TestClass]
    public class GetUserNotifications
    {
        private Mock<INotificationRepository> _mockNotificationRepository;
        private NotificationService _notificationService;

        [TestInitialize]
        public void Setup()
        {
            _mockNotificationRepository = new Mock<INotificationRepository>();
            _notificationService = new NotificationService(_mockNotificationRepository.Object);
        }

        [TestMethod]
        public async Task GetUserNotificationsAsync_WithExistingUser_ReturnsCorrectNotifications()
        {
            var userId = 1;
            var userNotificationsFromDb = new List<ApplicationUserNotification>
        {
            new ApplicationUserNotification
            {
                NotificationId = 1,
                UserId = userId,
                IsRead = false,
                Notification = new Notification { Id = 1, Title = "Notification 1", Content = "Content 1", CreatedAt = DateTime.UtcNow }
            },
            new ApplicationUserNotification
            {
                NotificationId = 2,
                UserId = userId,
                IsRead = true,
                Notification = new Notification { Id = 2, Title = "Notification 2", Content = "Content 2", CreatedAt = DateTime.UtcNow }
            }
        };

            _mockNotificationRepository.Setup(r => r.GetUserNotificationsAsync(userId))
                .ReturnsAsync(userNotificationsFromDb);

            var result = (await _notificationService.GetUserNotificationsAsync(userId)).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Notification 1", result[0].Title);
            Assert.AreEqual(userId, userNotificationsFromDb[0].UserId);
            _mockNotificationRepository.Verify(r => r.GetUserNotificationsAsync(userId), Times.Once);
        }

        [TestMethod]
        public async Task GetUserNotificationsAsync_WithNoNotifications_ReturnsEmptyList()
        {
            var userId = 99;
            var userNotificationsFromDb = new List<ApplicationUserNotification>();

            _mockNotificationRepository.Setup(r => r.GetUserNotificationsAsync(userId))
                .ReturnsAsync(userNotificationsFromDb);

            var result = (await _notificationService.GetUserNotificationsAsync(userId)).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            _mockNotificationRepository.Verify(r => r.GetUserNotificationsAsync(userId), Times.Once);
        }
    }
}