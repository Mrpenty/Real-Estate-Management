using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace RealEstateManagement.UnitTests.Favorite.FavoriteServiceTest
{
    [TestClass]
    public class Favorite_AddToFavoriteAsync_Tests : FavoriteServiceTestBase
    {
        [TestMethod]
        public async Task ReturnsTrue_WhenRepositoryReturnsTrue()
        {
            var userId = 1;
            var propertyId = 101;

            Repo.Setup(r => r.AddFavoritePropertyAsync(userId, propertyId)).ReturnsAsync(true);

            var ok = await Svc.AddToFavoriteAsync(userId, propertyId);

            Assert.IsTrue(ok);
            Repo.Verify(r => r.AddFavoritePropertyAsync(userId, propertyId), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsFalse_WhenRepositoryReturnsFalse()
        {
            var userId = 1;
            var propertyId = 101;

            Repo.Setup(r => r.AddFavoritePropertyAsync(userId, propertyId)).ReturnsAsync(false);

            var ok = await Svc.AddToFavoriteAsync(userId, propertyId);

            Assert.IsFalse(ok);
            Repo.Verify(r => r.AddFavoritePropertyAsync(userId, propertyId), Times.Once);
        }

        [TestMethod]
        public async Task PropagatesException_WhenRepositoryThrows()
        {
            Repo.Setup(r => r.AddFavoritePropertyAsync(1, 101))
                .ThrowsAsync(new InvalidOperationException("DB error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.AddToFavoriteAsync(1, 101));
        }
    }
}
