using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Repositories.Chat.Messages;       // chỉnh theo namespace thực tế của bạn
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Services.User;              // InterestedProperty ở đây

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
    [TestClass]
    public class UpdateStatusAsyncTests
    {
        private Mock<IInterestedPropertyRepository> _interestedRepoMock;
        private Mock<IPropertyPostRepository> _postRepoMock;
        private Mock<IMessageRepository> _messageRepoMock;
        private Mock<IRentalContractRepository> _contractRepoMock;
        private Mock<INotificationService> _notiSvcMock;
        private Mock<IPropertyRepository> _propertyRepoMock;
        private Mock<IMailService> _mailSvcMock;
        private Mock<IProfileService> _profileSvcMock;

        private InterestedPropertyService _service;

        [TestInitialize]
        public void Setup()
        {
            _interestedRepoMock = new Mock<IInterestedPropertyRepository>(MockBehavior.Loose);
            _postRepoMock = new Mock<IPropertyPostRepository>(MockBehavior.Loose);
            _messageRepoMock = new Mock<IMessageRepository>(MockBehavior.Loose);
            _contractRepoMock = new Mock<IRentalContractRepository>(MockBehavior.Loose);
            _notiSvcMock = new Mock<INotificationService>(MockBehavior.Loose);
            _propertyRepoMock = new Mock<IPropertyRepository>(MockBehavior.Loose);
            _mailSvcMock = new Mock<IMailService>(MockBehavior.Loose);
            _profileSvcMock = new Mock<IProfileService>(MockBehavior.Loose);

            _service = new InterestedPropertyService(
                _interestedRepoMock.Object,
                _postRepoMock.Object,
                _messageRepoMock.Object,
                _contractRepoMock.Object,
                _notiSvcMock.Object,
                _propertyRepoMock.Object,
                _mailSvcMock.Object,
                _profileSvcMock.Object
            );
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Throws_When_NotFound()
        {
            _interestedRepoMock.Setup(r => r.GetByIdAsync(2))
                     .ReturnsAsync((InterestedProperty)null);

            await _service.UpdateStatusAsync(1, InterestedStatus.WaitingForRenterReply);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Throws_When_Setting_Invalid_Status()
        {
            var ip = new InterestedProperty { Id = 1, Status = InterestedStatus.WaitingForRenterReply };
            _interestedRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ip);

            await _service.UpdateStatusAsync(1, InterestedStatus.DealSuccess);
        }

        [TestMethod]
        public async Task Updates_Status_When_Valid()
        {
            var ip = new InterestedProperty { Id = 1, Status = InterestedStatus.WaitingForRenterReply };
            _interestedRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ip);

            await _service.UpdateStatusAsync(1, InterestedStatus.WaitingForLandlordReply);

            Assert.AreEqual(InterestedStatus.WaitingForLandlordReply, ip.Status);
        }
    }
}
