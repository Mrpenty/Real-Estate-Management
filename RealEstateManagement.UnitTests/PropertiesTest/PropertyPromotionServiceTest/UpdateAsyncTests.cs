using Microsoft.EntityFrameworkCore;
using Moq;
using RealEstateManagement.Business.DTO.PromotionPackageDTO;
using RealEstateManagement.Business.Repositories.Package;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyPromotionServiceTest
{
    [TestClass]
    public class UpdateAsyncTests
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
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((PropertyPromotion?)null);

            var dto = new UpdatePropertyPromotionDTO
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30)
            };

            // Act
            var result = await _service.UpdateAsync(99, dto);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Updates_Successfully_When_Found()
        {
            // Arrange
            var existing = new PropertyPromotion
            {
                Id = 1,
                PropertyId = 55,
                PackageId = 10,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<PropertyPromotion>())).Returns(Task.CompletedTask);

            var dto = new UpdatePropertyPromotionDTO
            {
                StartDate = DateTime.UtcNow.AddDays(1),
                EndDate = DateTime.UtcNow.AddDays(40)
            };

            // Act
            var result = await _service.UpdateAsync(1, dto);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existing.Id, result.Id);
            Assert.AreEqual(dto.StartDate, result.StartDate);
            Assert.AreEqual(dto.EndDate, result.EndDate);

            _mockRepo.Verify(r => r.UpdateAsync(It.Is<PropertyPromotion>(
                p => p.Id == existing.Id && p.StartDate == dto.StartDate && p.EndDate == dto.EndDate
            )), Times.Once);
        }
        [TestMethod]
        public async Task UpdateAsync_ReturnsNull_When_NotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((PropertyPromotion?)null);

            var dto = new UpdatePropertyPromotionDTO { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(5) };

            // Act
            var result = await _service.UpdateAsync(1, dto);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task UpdateAsync_Updates_Dates_Correctly()
        {
            // Arrange
            var existing = new PropertyPromotion { Id = 1, PropertyId = 99, PackageId = 11, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            var newStart = DateTime.UtcNow.AddDays(2);
            var newEnd = DateTime.UtcNow.AddDays(10);

            var dto = new UpdatePropertyPromotionDTO { StartDate = newStart, EndDate = newEnd };

            // Act
            var result = await _service.UpdateAsync(1, dto);

            // Assert
            Assert.AreEqual(newStart, result.StartDate);
            Assert.AreEqual(newEnd, result.EndDate);
        }

        [TestMethod]
        public async Task UpdateAsync_Calls_RepoUpdateAsync_Once()
        {
            // Arrange
            var existing = new PropertyPromotion { Id = 2, PropertyId = 88, PackageId = 22, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow };
            _mockRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            var dto = new UpdatePropertyPromotionDTO { StartDate = DateTime.UtcNow.AddDays(1), EndDate = DateTime.UtcNow.AddDays(5) };

            // Act
            await _service.UpdateAsync(2, dto);

            // Assert
            _mockRepo.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

    }
}
