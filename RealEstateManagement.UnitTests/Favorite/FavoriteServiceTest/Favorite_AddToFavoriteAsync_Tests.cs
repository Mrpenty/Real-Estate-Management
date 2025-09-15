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
            var propertyId = 102;

            Repo.Setup(r => r.AddFavoritePropertyAsync(userId, propertyId)).ReturnsAsync(false);

            var ok = await Svc.AddToFavoriteAsync(userId, propertyId);

            Assert.IsFalse(ok);
            Repo.Verify(r => r.AddFavoritePropertyAsync(userId, propertyId), Times.Once);
        }

        [TestMethod]
        public async Task ThrowsInvalidOperation_WhenPropertyAlreadyFavorited()
        {
            var userId = 1;
            var propertyId = 200;

            Repo.Setup(r => r.AddFavoritePropertyAsync(userId, propertyId))
                .ReturnsAsync(false); // repo báo đã tồn tại

            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.AddToFavoriteAsync(userId, propertyId));

            StringAssert.Contains(ex.Message, "already in favorites");
            Repo.Verify(r => r.AddFavoritePropertyAsync(userId, propertyId), Times.Once);
        }


    }
}
