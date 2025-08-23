using Moq;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
    [TestClass]
    public class UpdateStatusAsyncTests
    {
        private Mock<IInterestedPropertyRepository> _repoMock;
        private InterestedPropertyService _service;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IInterestedPropertyRepository>();
            _service = new InterestedPropertyService(_repoMock.Object, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task Throws_When_NotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((InterestedProperty)null);

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
