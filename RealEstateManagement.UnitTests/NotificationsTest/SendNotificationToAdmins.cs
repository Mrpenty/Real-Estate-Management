using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Business.Repositories.NotificationRepository;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Data.Entity.Notification;
using System.Threading.Tasks;
namespace RealEstateManagement.UnitTests.NotificationsTest
{
    [TestClass]
    public class SendNotificationToAdmins
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
        public async Task SendNotificationToAdminsAsync_CallsCreateNotificationWithAudienceAdmins()
        {
            var createDto = new CreateNotificationDTO
            {
                Title = "Admin Notification",
                Content = "This is for all admins."
            };

            var createdNotification = new Notification
            {
                Id = 1,
                Title = createDto.Title,
                Content = createDto.Content,
                Audience = "admin"
            };

            _mockNotificationRepository.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(createdNotification);
            _mockNotificationRepository.Setup(r => r.GetUsersByAudienceAsync("admin"))
                .ReturnsAsync(new List<RealEstateManagement.Data.Entity.User.ApplicationUser>());
            _mockNotificationRepository.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<RealEstateManagement.Data.Entity.Notification.ApplicationUserNotification>>()))
                .Returns((Task<bool>)Task.CompletedTask);

            var result = await _notificationService.SendNotificationToAdminsAsync(createDto);

            Assert.IsTrue(result);
            Assert.AreEqual("admin", createDto.Audience);
            _mockNotificationRepository.Verify(r => r.CreateNotificationAsync(It.Is<Notification>(n => n.Audience == "admin")), Times.Once);
        }
    }
}