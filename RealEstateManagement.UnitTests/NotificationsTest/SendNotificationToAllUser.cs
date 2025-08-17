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
    public class SendNotificationToAllUser
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
        public async Task SendNotificationToAllUsersAsync_CallsCreateNotificationWithAudienceAll()
        {
            var createDto = new CreateNotificationDTO
            {
                Title = "All Users Notification",
                Content = "This is for everyone."
            };

            var createdNotification = new Notification
            {
                Id = 1,
                Title = createDto.Title,
                Content = createDto.Content,
                Audience = "all"
            };

            _mockNotificationRepository.Setup(r => r.CreateNotificationAsync(It.IsAny<Notification>()))
                .ReturnsAsync(createdNotification);
            _mockNotificationRepository.Setup(r => r.GetUsersByAudienceAsync("all"))
                .ReturnsAsync(new List<RealEstateManagement.Data.Entity.User.ApplicationUser>());
            _mockNotificationRepository.Setup(r => r.CreateUserNotificationsAsync(It.IsAny<List<RealEstateManagement.Data.Entity.Notification.ApplicationUserNotification>>()))
                .Returns((Task<bool>)Task.CompletedTask);

            var result = await _notificationService.SendNotificationToAllUsersAsync(createDto);

            Assert.IsTrue(result);
            Assert.AreEqual("all", createDto.Audience);
            _mockNotificationRepository.Verify(r => r.CreateNotificationAsync(It.Is<Notification>(n => n.Audience == "all")), Times.Once);
        }
    }
}