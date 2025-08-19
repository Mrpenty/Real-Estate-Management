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
    public class GetAllAsyncTests
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
        public async Task Returns_List_When_Found()
        {
            _mockRepo.Setup(r => r.GetAsync()).ReturnsAsync(new List<PropertyPromotion>
        {
            new PropertyPromotion { Id = 1, PropertyId = 10, PackageId = 5, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) }
        });

            var result = await _service.GetAllAsync();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(10, result.First().PropertyId);
        }

        [TestMethod]
        public async Task Returns_Empty_When_None()
        {
            _mockRepo.Setup(r => r.GetAsync()).ReturnsAsync(new List<PropertyPromotion>());

            var result = await _service.GetAllAsync();

            Assert.AreEqual(0, result.Count());
        }
    }
}
