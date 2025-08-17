using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.Favorite;
using RealEstateManagement.Business.Repositories.FavoriteRepository;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Favorite
{
    [TestClass]
    public class AddtofavoriteUnitTest
    {
        private Mock<IFavoriteRepository> _mockFavoriteRepository;
        private FavoriteService _favoriteService;

        [TestInitialize]
        public void Setup()
        {
            _mockFavoriteRepository = new Mock<IFavoriteRepository>();
            _favoriteService = new FavoriteService(_mockFavoriteRepository.Object);
        }
        [TestMethod]
        public async Task AddToFavoriteAsync_RepositoryThrows_PropagatesException()
        {
            _mockFavoriteRepository
                .Setup(r => r.AddFavoritePropertyAsync(1, 101))
                .ThrowsAsync(new InvalidOperationException("DB error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _favoriteService.AddToFavoriteAsync(1, 101));
        }
        [TestMethod]
        public async Task AddToFavoriteAsync_ValidRequest_ReturnsTrue()
        {
            var userId = 1;
            var propertyId = 101;

            _mockFavoriteRepository.Setup(r => r.AddFavoritePropertyAsync(userId, propertyId)).ReturnsAsync(true);

            var result = await _favoriteService.AddToFavoriteAsync(userId, propertyId);

            Assert.IsTrue(result);
            _mockFavoriteRepository.Verify(r => r.AddFavoritePropertyAsync(userId, propertyId), Times.Once);
        }

        [TestMethod]
        public async Task AddToFavoriteAsync_AlreadyInFavorites_ReturnsFalse()
        {
            var userId = 1;
            var propertyId = 101;

            _mockFavoriteRepository.Setup(r => r.AddFavoritePropertyAsync(userId, propertyId)).ReturnsAsync(false);

            var result = await _favoriteService.AddToFavoriteAsync(userId, propertyId);

            Assert.IsFalse(result);
            _mockFavoriteRepository.Verify(r => r.AddFavoritePropertyAsync(userId, propertyId), Times.Once);
        }

        [TestMethod]
        public async Task AddToFavoriteAsync_InvalidUserId_ReturnsFalse()
        {
            var userId = 999;
            var propertyId = 101;

            _mockFavoriteRepository.Setup(r => r.AddFavoritePropertyAsync(userId, propertyId)).ReturnsAsync(false);

            var result = await _favoriteService.AddToFavoriteAsync(userId, propertyId);

            Assert.IsFalse(result);
            _mockFavoriteRepository.Verify(r => r.AddFavoritePropertyAsync(userId, propertyId), Times.Once);
        }

        [TestMethod]
        public async Task AddToFavoriteAsync_InvalidPropertyId_ReturnsFalse()
        {
            var userId = 1;
            var propertyId = 999;

            _mockFavoriteRepository.Setup(r => r.AddFavoritePropertyAsync(userId, propertyId)).ReturnsAsync(false);

            var result = await _favoriteService.AddToFavoriteAsync(userId, propertyId);

            Assert.IsFalse(result);
            _mockFavoriteRepository.Verify(r => r.AddFavoritePropertyAsync(userId, propertyId), Times.Once);
        }
    }
}