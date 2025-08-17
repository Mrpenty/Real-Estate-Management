using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Business.Repositories.NotificationRepository;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Data.Entity.Notification;
using System;
using System.Threading.Tasks;
namespace RealEstateManagement.UnitTests.NotificationsTest
{
    [TestClass]
    public class GetNotificationByIdUnitTest
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
        public async Task GetNotificationByIdAsync_WithExistingId_ReturnsNotificationDTO()
        {
            var notificationId = 1;
            var notificationFromDb = new Notification
            {
                Id = notificationId,
                Title = "Test Title",
                Content = "Test Content",
                Type = "Test Type",
                Audience = "all",
                CreatedAt = DateTime.UtcNow
            };

            _mockNotificationRepository.Setup(r => r.GetNotificationByIdAsync(notificationId))
                .ReturnsAsync(notificationFromDb);

            var result = await _notificationService.GetNotificationByIdAsync(notificationId);

            Assert.IsNotNull(result);
            Assert.AreEqual(notificationId, result.Id);
            Assert.AreEqual(notificationFromDb.Title, result.Title);
            _mockNotificationRepository.Verify(r => r.GetNotificationByIdAsync(notificationId), Times.Once);
        }

        [TestMethod]
        public async Task GetNotificationByIdAsync_WithNonExistingId_ReturnsNull()
        {
            var notificationId = 99;

            _mockNotificationRepository.Setup(r => r.GetNotificationByIdAsync(notificationId))
                .ReturnsAsync((Notification)null);

            var result = await _notificationService.GetNotificationByIdAsync(notificationId);

            Assert.IsNull(result);
            _mockNotificationRepository.Verify(r => r.GetNotificationByIdAsync(notificationId), Times.Once);
        }
    }
}