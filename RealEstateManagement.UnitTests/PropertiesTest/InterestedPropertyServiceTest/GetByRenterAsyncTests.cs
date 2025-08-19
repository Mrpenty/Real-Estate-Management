using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class GetByRenterAsyncTests
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
        public async Task Returns_Empty_When_No_Data()
        {
            _repoMock.Setup(r => r.GetByRenterAsync(1)).ReturnsAsync(new List<InterestedProperty>());

            var result = await _service.GetByRenterAsync(1);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Returns_List_When_Data_Exists()
        {
            var list = new List<InterestedProperty>
            {
                new InterestedProperty { Id = 1, RenterId = 1, PropertyId = 10, Status = InterestedStatus.WaitingForLandlordReply },
                new InterestedProperty { Id = 2, RenterId = 1, PropertyId = 20, Status = InterestedStatus.WaitingForRenterReply }
            };
            _repoMock.Setup(r => r.GetByRenterAsync(1)).ReturnsAsync(list);

            var result = await _service.GetByRenterAsync(1);

            Assert.AreEqual(2, result.Count());
        }
    }
}
