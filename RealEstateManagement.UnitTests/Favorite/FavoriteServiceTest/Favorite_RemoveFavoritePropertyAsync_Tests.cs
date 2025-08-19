using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace RealEstateManagement.UnitTests.Favorite.FavoriteServiceTest
{
    [TestClass]
    public class Favorite_RemoveFavoritePropertyAsync_Tests : FavoriteServiceTestBase
    {
        [TestMethod]
        public async Task ReturnsTrue_WhenRepositoryReturnsTrue()
        {
            Repo.Setup(r => r.RemoveFavoritePropertyAsync(1, 101)).ReturnsAsync(true);

            var ok = await Svc.RemoveFavoritePropertyAsync(1, 101);

            Assert.IsTrue(ok);
            Repo.Verify(r => r.RemoveFavoritePropertyAsync(1, 101), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsFalse_WhenRepositoryReturnsFalse()
        {
            Repo.Setup(r => r.RemoveFavoritePropertyAsync(1, 101)).ReturnsAsync(false);

            var ok = await Svc.RemoveFavoritePropertyAsync(1, 101);

            Assert.IsFalse(ok);
            Repo.Verify(r => r.RemoveFavoritePropertyAsync(1, 101), Times.Once);
        }

        [TestMethod]
        public async Task PropagatesException_WhenRepositoryThrows()
        {
            Repo.Setup(r => r.RemoveFavoritePropertyAsync(1, 101))
                .ThrowsAsync(new InvalidOperationException("DB error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.RemoveFavoritePropertyAsync(1, 101));
        }
    }
}
