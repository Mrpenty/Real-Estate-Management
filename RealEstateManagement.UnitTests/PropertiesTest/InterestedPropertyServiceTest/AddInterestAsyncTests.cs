using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PropertiesTest.InterestedPropertyServiceTest
{
    [TestClass]
    public class AddInterestAsyncTests
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
        public async Task Adds_New_Interest_When_Not_Exists()
        {
            _repoMock.Setup(r => r.GetByRenterAndPropertyAsync(1, 1)).ReturnsAsync((InterestedProperty)null);
            _repoMock.Setup(r => r.AddAsync(It.IsAny<InterestedProperty>()))
                     .ReturnsAsync(new InterestedProperty { Id = 99, RenterId = 1, PropertyId = 1 });

            var result = await _service.AddInterestAsync(1, 1);

            Assert.AreEqual(99, result.Id);
            Assert.AreEqual(1, result.RenterId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Throws_When_LandlordRejected()
        {
            _repoMock.Setup(r => r.GetByRenterAndPropertyAsync(1, 1))
                     .ReturnsAsync(new InterestedProperty { Status = InterestedStatus.LandlordRejected });

            await _service.AddInterestAsync(1, 1);
        }

        [TestMethod]
        public async Task Reopen_When_Status_None()
        {
            var ip = new InterestedProperty { Id = 5, RenterId = 1, PropertyId = 1, Status = InterestedStatus.None };
            _repoMock.Setup(r => r.GetByRenterAndPropertyAsync(1, 1)).ReturnsAsync(ip);

            var result = await _service.AddInterestAsync(1, 1);

            Assert.AreEqual(InterestedStatus.WaitingForRenterReply, ip.Status);
        }
    }
}
