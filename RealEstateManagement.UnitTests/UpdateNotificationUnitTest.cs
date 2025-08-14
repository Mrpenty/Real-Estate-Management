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

[TestClass]
public class UpdateNotificationUnitTest
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
    public async Task UpdateNotificationAsync_WithValidData_ReturnsUpdatedNotificationDTO()
    {
        var updateDto = new UpdateNotificationDTO
        {
            Id = 1,
            Title = "Updated Title",
            Content = "Updated Content",
            Type = "Alert",
            Audience = "landlords"
        };

        var existingNotification = new Notification
        {
            Id = 1,
            Title = "Old Title",
            Content = "Old Content",
            Type = "Announcement",
            Audience = "all"
        };

        var users = new List<ApplicationUser> { new ApplicationUser { Id = 3 }, new ApplicationUser { Id = 4 } };

        _mockNotificationRepository.Setup(r => r.GetNotificationByIdAsync(updateDto.Id))
            .ReturnsAsync(existingNotification);
        _mockNotificationRepository.Setup(r => r.DeleteUserNotificationsByNotificationIdAsync(updateDto.Id))
            .Returns((Task<bool>)Task.CompletedTask);
        _mockNotificationRepository.Setup(r => r.UpdateNotificationAsync(It.IsAny<Notification>()))
            .ReturnsAsync(existingNotification);
        _mockNotificationRepository.Setup(r => r.GetUsersByAudienceAsync("landlords"))
            .ReturnsAsync(users);
        _mockNotificationRepository.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
            .Returns((Task<bool>)Task.CompletedTask);

        var result = await _notificationService.UpdateNotificationAsync(updateDto);

        Assert.IsNotNull(result);
        Assert.AreEqual(updateDto.Title, result.Title);
        Assert.AreEqual(updateDto.Audience, result.Audience);
        Assert.AreEqual(2, result.RecipientCount);

        _mockNotificationRepository.Verify(r => r.GetNotificationByIdAsync(updateDto.Id), Times.Once);
        _mockNotificationRepository.Verify(r => r.DeleteUserNotificationsByNotificationIdAsync(updateDto.Id), Times.Once);
        _mockNotificationRepository.Verify(r => r.UpdateNotificationAsync(It.IsAny<Notification>()), Times.Once);
        _mockNotificationRepository.Verify(r => r.GetUsersByAudienceAsync("landlords"), Times.Once);
        _mockNotificationRepository.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
    }

    [TestMethod]
    public async Task UpdateNotificationAsync_NotificationNotFound_ThrowsArgumentException()
    {
        var updateDto = new UpdateNotificationDTO
        {
            Id = 99,
            Title = "Updated Title"
        };

        _mockNotificationRepository.Setup(r => r.GetNotificationByIdAsync(updateDto.Id))
            .ReturnsAsync((Notification)null);

        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _notificationService.UpdateNotificationAsync(updateDto));

        _mockNotificationRepository.Verify(r => r.GetNotificationByIdAsync(updateDto.Id), Times.Once);
        _mockNotificationRepository.Verify(r => r.UpdateNotificationAsync(It.IsAny<Notification>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateNotificationAsync_RepositoryFailsOnUpdate_ThrowsException()
    {
        var updateDto = new UpdateNotificationDTO
        {
            Id = 1,
            Title = "Updated Title"
        };

        var existingNotification = new Notification { Id = 1 };

        _mockNotificationRepository.Setup(r => r.GetNotificationByIdAsync(updateDto.Id))
            .ReturnsAsync(existingNotification);
        _mockNotificationRepository.Setup(r => r.UpdateNotificationAsync(It.IsAny<Notification>()))
            .ThrowsAsync(new InvalidOperationException("DB error"));

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _notificationService.UpdateNotificationAsync(updateDto));

        _mockNotificationRepository.Verify(r => r.GetNotificationByIdAsync(updateDto.Id), Times.Once);
        _mockNotificationRepository.Verify(r => r.DeleteUserNotificationsByNotificationIdAsync(updateDto.Id), Times.Once);
        _mockNotificationRepository.Verify(r => r.UpdateNotificationAsync(It.IsAny<Notification>()), Times.Once);
    }
}
