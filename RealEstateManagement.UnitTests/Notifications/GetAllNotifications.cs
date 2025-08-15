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

[TestClass]
public class GetAllNotifications
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
    public async Task GetAllNotificationsAsync_WithNotifications_ReturnsCorrectCountAndData()
    {
        var notificationsFromDb = new List<Notification>
        {
            new Notification { Id = 1, Title = "Title 1", Content = "Content 1", CreatedAt = DateTime.UtcNow },
            new Notification { Id = 2, Title = "Title 2", Content = "Content 2", CreatedAt = DateTime.UtcNow },
        };

        _mockNotificationRepository.Setup(r => r.GetAllNotificationsAsync())
            .ReturnsAsync(notificationsFromDb);

        var result = (await _notificationService.GetAllNotificationsAsync()).ToList();

        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Title 1", result[0].Title);
        Assert.AreEqual("Title 2", result[1].Title);
        _mockNotificationRepository.Verify(r => r.GetAllNotificationsAsync(), Times.Once);
    }

    [TestMethod]
    public async Task GetAllNotificationsAsync_WithNoNotifications_ReturnsEmptyList()
    {
        var notificationsFromDb = new List<Notification>();

        _mockNotificationRepository.Setup(r => r.GetAllNotificationsAsync())
            .ReturnsAsync(notificationsFromDb);

        var result = (await _notificationService.GetAllNotificationsAsync()).ToList();

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
        _mockNotificationRepository.Verify(r => r.GetAllNotificationsAsync(), Times.Once);
    }
}
