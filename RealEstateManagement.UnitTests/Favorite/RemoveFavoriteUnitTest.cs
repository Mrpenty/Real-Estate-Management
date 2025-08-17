using Moq;
using RealEstateManagement.Business.Repositories.FavoriteRepository;
using RealEstateManagement.Business.Services.Favorite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Favorite
{
    [TestClass]
    public class RemoveFavoriteUnitTest
    {
        private Mock<IFavoriteRepository> _mockRepo = null!;
        private FavoriteService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IFavoriteRepository>();
            _service = new FavoriteService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task RemoveFavoritePropertyAsync_ValidRequest_ReturnsTrue()
        {
            var userId = 1;
            var propertyId = 101;

            _mockRepo.Setup(r => r.RemoveFavoritePropertyAsync(userId, propertyId))
                     .ReturnsAsync(true);

            var ok = await _service.RemoveFavoritePropertyAsync(userId, propertyId);

            Assert.IsTrue(ok);
            _mockRepo.Verify(r => r.RemoveFavoritePropertyAsync(userId, propertyId), Times.Once);
        }

        [TestMethod]
        public async Task RemoveFavoritePropertyAsync_NotInFavorites_ReturnsFalse()
        {
            var userId = 1;
            var propertyId = 101;

            _mockRepo.Setup(r => r.RemoveFavoritePropertyAsync(userId, propertyId))
                     .ReturnsAsync(false);

            var ok = await _service.RemoveFavoritePropertyAsync(userId, propertyId);

            Assert.IsFalse(ok);
            _mockRepo.Verify(r => r.RemoveFavoritePropertyAsync(userId, propertyId), Times.Once);
        }

        [TestMethod]
        public async Task RemoveFavoritePropertyAsync_InvalidUserId_ReturnsFalse()
        {
            var userId = 999;
            var propertyId = 101;

            _mockRepo.Setup(r => r.RemoveFavoritePropertyAsync(userId, propertyId))
                     .ReturnsAsync(false);

            var ok = await _service.RemoveFavoritePropertyAsync(userId, propertyId);

            Assert.IsFalse(ok);
            _mockRepo.Verify(r => r.RemoveFavoritePropertyAsync(userId, propertyId), Times.Once);
        }

        [TestMethod]
        public async Task RemoveFavoritePropertyAsync_InvalidPropertyId_ReturnsFalse()
        {
            var userId = 1;
            var propertyId = 999;

            _mockRepo.Setup(r => r.RemoveFavoritePropertyAsync(userId, propertyId))
                     .ReturnsAsync(false);

            var ok = await _service.RemoveFavoritePropertyAsync(userId, propertyId);

            Assert.IsFalse(ok);
            _mockRepo.Verify(r => r.RemoveFavoritePropertyAsync(userId, propertyId), Times.Once);
        }

        [TestMethod]
        public async Task RemoveFavoritePropertyAsync_RepositoryThrows_PropagatesException()
        {
            var userId = 1;
            var propertyId = 101;

            _mockRepo.Setup(r => r.RemoveFavoritePropertyAsync(userId, propertyId))
                     .ThrowsAsync(new InvalidOperationException("DB error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _service.RemoveFavoritePropertyAsync(userId, propertyId));
        }
    }
}
