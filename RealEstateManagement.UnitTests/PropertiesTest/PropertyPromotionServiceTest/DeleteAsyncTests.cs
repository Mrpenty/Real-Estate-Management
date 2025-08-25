using Microsoft.EntityFrameworkCore;
using Moq;
using RealEstateManagement.Business.Repositories.Package;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyPromotionServiceTest
{
    [TestClass]
    public class DeleteAsyncTests
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
        public async Task Returns_False_When_NotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((PropertyPromotion?)null);

            // Act
            var result = await _service.DeleteAsync(99);

            // Assert
            Assert.IsFalse(result);
            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task Returns_True_When_Found_And_Deleted()
        {
            // Arrange
            var existing = new PropertyPromotion { Id = 1, PropertyId = 10, PackageId = 20 };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.IsTrue(result);
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task Calls_DeleteAsync_With_Correct_Id()
        {
            // Arrange
            var existing = new PropertyPromotion { Id = 5, PropertyId = 50, PackageId = 60 };
            _mockRepo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(5);

            // Assert
            _mockRepo.Verify(r => r.DeleteAsync(5), Times.Once);
        }
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task DeleteAsync_Throws_Exception_When_RepoFails()
        {
            // Arrange
            var existing = new PropertyPromotion { Id = 10, PropertyId = 100, PackageId = 200 };
            _mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(existing);
            _mockRepo.Setup(r => r.DeleteAsync(10)).ThrowsAsync(new Exception("Repo failed"));

            // Act
            await _service.DeleteAsync(10);

            // Assert -> Expect Exception
        }

        [TestMethod]
        public async Task DeleteAsync_Does_Not_SaveChanges_When_NotFound()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((PropertyPromotion?)null);

            // Act
            var result = await _service.DeleteAsync(123);

            // Assert
            Assert.IsFalse(result);
            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

    }
}
