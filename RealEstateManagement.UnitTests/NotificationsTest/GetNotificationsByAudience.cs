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
    public class GetNotificationsByAudience
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
        public async Task GetNotificationsByAudienceAsync_WithExistingAudience_ReturnsCorrectNotifications()
        {
            var audience = "renters";
            var notificationsFromDb = new List<Notification>
        {
            new Notification { Id = 1, Title = "Renter Alert", Audience = "renters", CreatedAt = DateTime.UtcNow },
            new Notification { Id = 2, Title = "Renter Notice", Audience = "renters", CreatedAt = DateTime.UtcNow },
        };

            _mockNotificationRepository.Setup(r => r.GetNotificationsByAudienceAsync(audience))
                .ReturnsAsync(notificationsFromDb);

            var result = (await _notificationService.GetNotificationsByAudienceAsync(audience)).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(n => n.Audience == audience));
            _mockNotificationRepository.Verify(r => r.GetNotificationsByAudienceAsync(audience), Times.Once);
        }

        [TestMethod]
        public async Task GetNotificationsByAudienceAsync_WithNonExistingAudience_ReturnsEmptyList()
        {
            var audience = "non-existent";
            var notificationsFromDb = new List<Notification>();

            _mockNotificationRepository.Setup(r => r.GetNotificationsByAudienceAsync(audience))
                .ReturnsAsync(notificationsFromDb);

            var result = (await _notificationService.GetNotificationsByAudienceAsync(audience)).ToList();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            _mockNotificationRepository.Verify(r => r.GetNotificationsByAudienceAsync(audience), Times.Once);
        }
    }
}