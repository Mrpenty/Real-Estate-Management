using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Repositories.Chat.Messages;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
    [TestClass]
    public class AddInterestAsyncTests
    {
        private Mock<IInterestedPropertyRepository> _interestedRepo = null!;
        private Mock<IPropertyPostRepository> _propertyPostRepo = null!;
        private Mock<IMessageRepository> _messageRepo = null!;
        private Mock<IRentalContractRepository> _contractRepo = null!;
        private Mock<INotificationService> _notificationService = null!;
        private Mock<IPropertyRepository> _propertyRepo = null!;
        private Mock<IMailService> _mailService = null!;
        private Mock<IProfileService> _profileService = null!;
        private InterestedPropertyService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _interestedRepo = new Mock<IInterestedPropertyRepository>();
            _propertyPostRepo = new Mock<IPropertyPostRepository>();
            _messageRepo = new Mock<IMessageRepository>();
            _contractRepo = new Mock<IRentalContractRepository>();
            _notificationService = new Mock<INotificationService>();
            _propertyRepo = new Mock<IPropertyRepository>();
            _mailService = new Mock<IMailService>();
            _profileService = new Mock<IProfileService>();

            // Nếu service có await UpdateAsync(...), set up trả về Task Completed
            _interestedRepo.Setup(r => r.UpdateAsync(It.IsAny<InterestedProperty>()))
                           .Returns(Task.CompletedTask);

            _service = new InterestedPropertyService(
                _interestedRepo.Object,
                _propertyPostRepo.Object,
                _messageRepo.Object,
                _contractRepo.Object,
                _notificationService.Object,
                _propertyRepo.Object,
                _mailService.Object,
                _profileService.Object
            );
        }

        [TestMethod]
        public async Task Adds_New_Interest_When_Not_Exists()
        {
            _interestedRepo.Setup(r => r.GetByRenterAndPropertyAsync(1, 1))
                           .ReturnsAsync((InterestedProperty)null!);

            _interestedRepo.Setup(r => r.AddAsync(It.IsAny<InterestedProperty>()))
                           .ReturnsAsync(new InterestedProperty { Id = 99, RenterId = 1, PropertyId = 1 });

            var result = await _service.AddInterestAsync(1, 1);

            Assert.AreEqual(99, result.Id);
            Assert.AreEqual(1, result.RenterId);
        }

        [TestMethod]
        public async Task Throws_When_LandlordRejected()
        {
            _interestedRepo.Setup(r => r.GetByRenterAndPropertyAsync(1, 1))
                           .ReturnsAsync(new InterestedProperty { Status = InterestedStatus.LandlordRejected });

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _service.AddInterestAsync(1, 1));
        }

        [TestMethod]
        public async Task Reopen_When_Status_None()
        {
            var ip = new InterestedProperty { Id = 5, RenterId = 1, PropertyId = 1, Status = InterestedStatus.None };

            _interestedRepo.Setup(r => r.GetByRenterAndPropertyAsync(1, 1)).ReturnsAsync(ip);
            _interestedRepo.Setup(r => r.UpdateAsync(It.IsAny<InterestedProperty>()))
                           .Returns(Task.CompletedTask);

            var result = await _service.AddInterestAsync(1, 1);

            Assert.AreEqual(InterestedStatus.WaitingForRenterReply, ip.Status);

            // ✅ service hiện tại gọi 2 lần
            _interestedRepo.Verify(r => r.UpdateAsync(It.Is<InterestedProperty>(x => x == ip)), Times.Exactly(2));
        }

    }
}
