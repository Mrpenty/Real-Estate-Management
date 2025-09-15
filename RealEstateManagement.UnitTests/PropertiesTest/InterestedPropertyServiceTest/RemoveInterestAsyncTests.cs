using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Repositories.Chat.Messages;       // chỉnh theo namespace thực tế
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Services.User;              // InterestedProperty ở đây

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
    [TestClass]
    public class RemoveInterestAsyncTests
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
        public async Task Returns_False_When_NotFound()
        {
            // cố tình trả null để RemoveInterestAsync(1,1) trả false
            _interestedRepoMock.Setup(r => r.GetByRenterAndPropertyAsync(1, 1))
                     .ReturnsAsync((InterestedProperty)null);

            var result = await _service.RemoveInterestAsync(1, 1);

            Assert.IsFalse(result);
            _interestedRepoMock.Verify(r => r.DeleteAsync(It.IsAny<InterestedProperty>()), Times.Never);
        }

        [TestMethod]
        public async Task Deletes_When_Exists()
        {
            var ip = new InterestedProperty { Id = 1, RenterId = 1, PropertyId = 1 };

            _interestedRepoMock.Setup(r => r.GetByRenterAndPropertyAsync(1, 1))
                     .ReturnsAsync(ip);

            var result = await _service.RemoveInterestAsync(1, 1);

            Assert.IsTrue(result);
            _interestedRepoMock.Verify(r => r.DeleteAsync(ip), Times.Once);
        }
    }
}
