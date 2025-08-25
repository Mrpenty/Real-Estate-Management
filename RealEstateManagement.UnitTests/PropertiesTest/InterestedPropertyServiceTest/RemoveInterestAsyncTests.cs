using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Repositories.Chat.Messages;       // chỉnh theo namespace thực tế
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity.User;              // InterestedProperty ở đây

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
    [TestClass]
    public class RemoveInterestAsyncTests
    {
        private Mock<IInterestedPropertyRepository> _repoMock;
        private Mock<IPropertyPostRepository> _postRepoMock;
        private Mock<IMessageRepository> _msgRepoMock;
        private Mock<IRentalContractRepository> _contractRepoMock;

        private InterestedPropertyService _service;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IInterestedPropertyRepository>();
            _postRepoMock = new Mock<IPropertyPostRepository>();
            _msgRepoMock = new Mock<IMessageRepository>();
            _contractRepoMock = new Mock<IRentalContractRepository>();

            _service = new InterestedPropertyService(
                _repoMock.Object,
                _postRepoMock.Object,
                _msgRepoMock.Object,
                _contractRepoMock.Object
            );
        }

        [TestMethod]
        public async Task Returns_False_When_NotFound()
        {
            // cố tình trả null để RemoveInterestAsync(1,1) trả false
            _repoMock.Setup(r => r.GetByRenterAndPropertyAsync(1, 1))
                     .ReturnsAsync((InterestedProperty)null);

            var result = await _service.RemoveInterestAsync(1, 1);

            Assert.IsFalse(result);
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<InterestedProperty>()), Times.Never);
        }

        [TestMethod]
        public async Task Deletes_When_Exists()
        {
            var ip = new InterestedProperty { Id = 1, RenterId = 1, PropertyId = 1 };

            _repoMock.Setup(r => r.GetByRenterAndPropertyAsync(1, 1))
                     .ReturnsAsync(ip);

            var result = await _service.RemoveInterestAsync(1, 1);

            Assert.IsTrue(result);
            _repoMock.Verify(r => r.DeleteAsync(ip), Times.Once);
        }
    }
}
