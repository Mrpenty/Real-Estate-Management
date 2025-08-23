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
    public class RemoveInterestAsyncTests
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
        public async Task Returns_False_When_NotFound()
        {
            _repoMock.Setup(r => r.GetByRenterAndPropertyAsync(1, 2)).ReturnsAsync((InterestedProperty)null);

            var result = await _service.RemoveInterestAsync(1, 1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Deletes_When_Exists()
        {
            var ip = new InterestedProperty { Id = 1, RenterId = 1, PropertyId = 1 };
            _repoMock.Setup(r => r.GetByRenterAndPropertyAsync(1, 1)).ReturnsAsync(ip);

            var result = await _service.RemoveInterestAsync(1, 1);

            Assert.IsTrue(result);
            _repoMock.Verify(r => r.DeleteAsync(ip), Times.Once);
        }
    }

}
