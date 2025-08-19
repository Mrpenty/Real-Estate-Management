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
    public class GetByIdAsyncTests
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
        public async Task Returns_Null_When_NotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((InterestedProperty)null);

            var result = await _service.GetByIdAsync(99);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Returns_Entity_When_Found()
        {
            var ip = new InterestedProperty { Id = 5, RenterId = 1, PropertyId = 99 };
            _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(ip);

            var result = await _service.GetByIdAsync(5);

            Assert.IsNotNull(result);
            Assert.AreEqual(99, result.PropertyId);
        }
    }

}
