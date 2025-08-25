using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Repositories.Chat.Messages;       // chỉnh đúng namespace dự án bạn
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity.User;              // InterestedProperty ở đây

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
    [TestClass]
    public class GetByIdAsyncTests
    {
        private Mock<IInterestedPropertyRepository> _interestedRepoMock;
        private Mock<IPropertyPostRepository> _postRepoMock;
        private Mock<IMessageRepository> _messageRepoMock;
        private Mock<IRentalContractRepository> _contractRepoMock;

        private InterestedPropertyService _service;

        [TestInitialize]
        public void Setup()
        {
            _interestedRepoMock = new Mock<IInterestedPropertyRepository>();
            _postRepoMock = new Mock<IPropertyPostRepository>();
            _messageRepoMock = new Mock<IMessageRepository>();
            _contractRepoMock = new Mock<IRentalContractRepository>();

            _service = new InterestedPropertyService(
                _interestedRepoMock.Object,
                _postRepoMock.Object,
                _messageRepoMock.Object,
                _contractRepoMock.Object
            );
        }

        [TestMethod]
        public async Task Returns_Null_When_NotFound()
        {
            _interestedRepoMock.Setup(r => r.GetByIdAsync(99))
                               .ReturnsAsync((InterestedProperty)null);

            var result = await _service.GetByIdAsync(99);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Returns_Entity_When_Found()
        {
            var ip = new InterestedProperty { Id = 5, RenterId = 1, PropertyId = 99 };
            _interestedRepoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(ip);

            var result = await _service.GetByIdAsync(5);

            Assert.IsNotNull(result);
            Assert.AreEqual(99, result.PropertyId);
        }
    }
}
