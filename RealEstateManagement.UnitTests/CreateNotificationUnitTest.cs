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
public class CreateNotificationUnitTest
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
    public async Task CreateNotificationAsync_WithValidData_ReturnsNotificationDTO()
    {
        var createDto = new CreateNotificationDTO
        {
            Title = "New Title",
            Content = "New Content",
            Type = "Announcement",
            Audience = "all"
        };

        var users = new List<ApplicationUser>
        {
            new ApplicationUser { Id = 1 },
            new ApplicationUser { Id = 2 }
        };

        var createdNotification = new Notification
        {
            Id = 1,
            Title = createDto.Title,
            Content = createDto.Content,
            Type = createDto.Type,
            Audience = createDto.Audience,
            CreatedAt = DateTime.UtcNow
        };

        _mockNotificationRepository.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
            .ReturnsAsync(createdNotification);
        _mockNotificationRepository.Setup(r => r.GetUsersByAudienceAsync("all"))
            .ReturnsAsync(users);
        _mockNotificationRepository.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
            .Returns((Task<bool>)Task.CompletedTask);

        var result = await _notificationService.CreateNotificationAsync(createDto);

        Assert.IsNotNull(result);
        Assert.AreEqual(createDto.Title, result.Title);
        Assert.AreEqual(2, result.RecipientCount);
        _mockNotificationRepository.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
        _mockNotificationRepository.Verify(r => r.GetUsersByAudienceAsync("all"), Times.Once);
        _mockNotificationRepository.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
    }

    [TestMethod]
    public async Task CreateNotificationAsync_WithSpecificUsers_CreatesNotificationsCorrectly()
    {
        var specificUserIds = new List<int> { 1, 2 };
        var createDto = new CreateNotificationDTO
        {
            Title = "Specific Users",
            Content = "Content for specific users",
            Type = "Alert",
            Audience = "specific",
            SpecificUserIds = specificUserIds
        };

        var users = specificUserIds.Select(id => new ApplicationUser { Id = id }).ToList();

        var createdNotification = new Notification
        {
            Id = 1,
            Title = createDto.Title,
            Content = createDto.Content,
            Type = createDto.Type,
            Audience = createDto.Audience,
            CreatedAt = DateTime.UtcNow
        };

        _mockNotificationRepository.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
            .ReturnsAsync(createdNotification);
        _mockNotificationRepository.Setup(r => r.GetUsersByIdsAsync(specificUserIds))
            .ReturnsAsync(users);
        _mockNotificationRepository.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
            .Returns((Task<bool>)Task.CompletedTask);

        var result = await _notificationService.CreateNotificationAsync(createDto);

        Assert.IsNotNull(result);
        Assert.AreEqual("specific", result.Audience);
        Assert.AreEqual(specificUserIds.Count, result.RecipientCount);
        _mockNotificationRepository.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
        _mockNotificationRepository.Verify(r => r.GetUsersByIdsAsync(specificUserIds), Times.Once);
        _mockNotificationRepository.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Once);
    }

    [TestMethod]
    public async Task CreateNotificationAsync_WithEmptyAudience_DoesNotCreateUserNotifications()
    {
        var createDto = new CreateNotificationDTO
        {
            Title = "No Audience",
            Content = "No one to notify",
            Type = "Info",
            Audience = "invalid_audience"
        };

        var createdNotification = new Notification
        {
            Id = 1,
            Title = createDto.Title,
            Content = createDto.Content,
            Type = createDto.Type,
            Audience = createDto.Audience,
            CreatedAt = DateTime.UtcNow
        };

        _mockNotificationRepository.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
            .ReturnsAsync(createdNotification);
        _mockNotificationRepository.Setup(r => r.GetUsersByAudienceAsync(It.IsAny<string>()))
            .ReturnsAsync(new List<ApplicationUser>());
        _mockNotificationRepository.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()))
            .Returns((Task<bool>)Task.CompletedTask);

        var result = await _notificationService.CreateNotificationAsync(createDto);

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.RecipientCount);
        _mockNotificationRepository.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
        _mockNotificationRepository.Verify(r => r.CreateUserNotificationsAsync(It.IsAny<List<ApplicationUserNotification>>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateNotificationAsync_RepositoryThrowsException_ThrowsException()
    {
        var createDto = new CreateNotificationDTO
        {
            Title = "Error Test",
            Content = "This should fail",
            Audience = "all"
        };

        _mockNotificationRepository.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
            .ThrowsAsync(new InvalidOperationException("DB error"));

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _notificationService.CreateNotificationAsync(createDto));

        _mockNotificationRepository.Verify(r => r.CreateNotificationAsync(It.IsAny<Notification>()), Times.Once);
        _mockNotificationRepository.Verify(r => r.GetUsersByAudienceAsync(It.IsAny<string>()), Times.Never);
    }
}
