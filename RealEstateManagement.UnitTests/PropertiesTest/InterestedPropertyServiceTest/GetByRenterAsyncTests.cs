using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Repositories.Chat.Messages;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
    [TestClass]
    public class GetById1AsyncTests
    {
        private Mock<IInterestedPropertyRepository> _interestedRepoMock;
        private Mock<IPropertyPostRepository> _propertyPostRepoMock;
        private Mock<IMessageRepository> _messageRepoMock;
        private Mock<IRentalContractRepository> _contractRepoMock;

        private InterestedPropertyService _service;

        [TestInitialize]
        public void Setup()
        {
            _interestedRepoMock = new Mock<IInterestedPropertyRepository>();
            _propertyPostRepoMock = new Mock<IPropertyPostRepository>();
            _messageRepoMock = new Mock<IMessageRepository>();
            _contractRepoMock = new Mock<IRentalContractRepository>();

            _service = new InterestedPropertyService(
                _interestedRepoMock.Object,
                _propertyPostRepoMock.Object,
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

            _interestedRepoMock.Setup(r => r.GetByIdAsync(5))
                               .ReturnsAsync(ip);

            var result = await _service.GetByIdAsync(5);

            Assert.IsNotNull(result);
            Assert.AreEqual(99, result.PropertyId);
        }
    }
}
