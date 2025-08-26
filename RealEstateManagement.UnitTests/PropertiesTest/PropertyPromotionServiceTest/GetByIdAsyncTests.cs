using Microsoft.EntityFrameworkCore;
using Moq;
using RealEstateManagement.Business.Repositories.Package;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyPromotionServiceTest
{
    [TestClass]
    public class GetByIdAsyncTests
    {
        private Mock<IPropertyPromotionRepository> _mockRepo;
        private RentalDbContext _dbContext;
        private PropertyPromotionService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IPropertyPromotionRepository>();
            var options = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new RentalDbContext(options);
            _service = new PropertyPromotionService(_mockRepo.Object, _dbContext);
        }

        [TestMethod]
        public async Task Returns_Null_When_NotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((PropertyPromotion)null);

            var result = await _service.GetByIdAsync(99);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Returns_DTO_When_Found()
        {
            var entity = new PropertyPromotion { Id = 1, PropertyId = 10, PackageId = 2, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(5) };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(entity);

            var result = await _service.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.PropertyId);
        }
    }

}
