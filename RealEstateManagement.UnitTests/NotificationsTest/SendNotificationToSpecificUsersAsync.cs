using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Business.Repositories.NotificationRepository;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Data.Entity.Notification;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace RealEstateManagement.UnitTests.NotificationsTest
{
    [TestClass]
    public class SendNotificationToSpecificUsersAsync
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
        public async Task SendNotificationToSpecificUsersAsync_WithValidIds_ReturnsTrueAndCreatesNotification()
        {
            var specificUserIds = new List<int> { 1, 2 };
            var createDto = new CreateNotificationDTO
            {
                Title = "Specific User Notification",
                Content = "This is for specific users.",
                SpecificUserIds = specificUserIds
            };
            var usersFromDb = specificUserIds.Select(id => new ApplicationUser { Id = id }).ToList();

            _mockNotificationRepository.Setup(r => r.GetUsersByIdsAsync(specificUserIds))
                .ReturnsAsync(usersFromDb);
            _mockNotificationRepository.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(new Notification { Id = 1 });
            _mockNotificationRepository.Setup(r => r.GetUsersByIdsAsync(specificUserIds))
                .ReturnsAsync(usersFromDb);
            _mockNotificationRepository.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
                .Returns((Task<bool>)Task.CompletedTask);

            var result = await _notificationService.SendNotificationToSpecificUsersAsync(createDto);

            Assert.IsTrue(result);
            _mockNotificationRepository.Verify(r => r.GetUsersByIdsAsync(specificUserIds), Times.Once);
            _mockNotificationRepository.Verify(r => r.CreateNotificationAsync(It.Is<Notification>(n => n.Audience == "specific")), Times.Once);
        }

        [TestMethod]
        public async Task SendNotificationToSpecificUsersAsync_WithNullIds_ThrowsArgumentException()
        {
            var createDto = new CreateNotificationDTO
            {
                Title = "Specific User Notification",
                Content = "This is for specific users.",
                SpecificUserIds = null
            };

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _notificationService.SendNotificationToSpecificUsersAsync(createDto));

            _mockNotificationRepository.Verify(r => r.GetUsersByIdsAsync(It.IsAny<List<int>>()), Times.Never);
            _mockNotificationRepository.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Never);
        }

        [TestMethod]
        public async Task SendNotificationToSpecificUsersAsync_WithNonExistingIds_ThrowsArgumentException()
        {
            var specificUserIds = new List<int> { 1, 2, 99 };
            var createDto = new CreateNotificationDTO
            {
                Title = "Specific User Notification",
                Content = "This is for specific users.",
                SpecificUserIds = specificUserIds
            };
            var usersFromDb = new List<ApplicationUser> { new ApplicationUser { Id = 1 }, new ApplicationUser { Id = 2 } };

            _mockNotificationRepository.Setup(r => r.GetUsersByIdsAsync(specificUserIds))
                .ReturnsAsync(usersFromDb);

            var exception = await Assert.ThrowsExceptionAsync<ArgumentException>(() => _notificationService.SendNotificationToSpecificUsersAsync(createDto));

            Assert.IsTrue(exception.Message.Contains("User IDs not found: 99"));
            _mockNotificationRepository.Verify(r => r.GetUsersByIdsAsync(specificUserIds), Times.Once);
            _mockNotificationRepository.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Never);
        }
    }
}