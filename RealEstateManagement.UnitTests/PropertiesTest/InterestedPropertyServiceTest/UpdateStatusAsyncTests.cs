using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Repositories.Chat.Messages;       // chỉnh theo namespace thực tế của bạn
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity.User;              // InterestedProperty ở đây

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
    [TestClass]
    public class UpdateStatusAsyncTests
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
        [ExpectedException(typeof(Exception))]
        public async Task Throws_When_NotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(2))
                     .ReturnsAsync((InterestedProperty)null);

            await _service.UpdateStatusAsync(1, InterestedStatus.WaitingForRenterReply);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Throws_When_Setting_Invalid_Status()
        {
            var ip = new InterestedProperty { Id = 1, Status = InterestedStatus.WaitingForRenterReply };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ip);

            await _service.UpdateStatusAsync(1, InterestedStatus.DealSuccess);
        }

        [TestMethod]
        public async Task Updates_Status_When_Valid()
        {
            var ip = new InterestedProperty { Id = 1, Status = InterestedStatus.WaitingForRenterReply };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(ip);

            await _service.UpdateStatusAsync(1, InterestedStatus.WaitingForLandlordReply);

            Assert.AreEqual(InterestedStatus.WaitingForLandlordReply, ip.Status);
        }
    }
}
