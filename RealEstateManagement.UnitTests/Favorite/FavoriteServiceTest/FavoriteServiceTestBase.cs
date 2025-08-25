using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Repositories.FavoriteRepository;
using RealEstateManagement.Business.Services.Favorite;

namespace RealEstateManagement.UnitTests.Favorite.FavoriteServiceTest
{
    public abstract class FavoriteServiceTestBase
    {
        protected Mock<IFavoriteRepository> Repo = null!;
        protected FavoriteService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            Repo = new Mock<IFavoriteRepository>(MockBehavior.Strict);
            Svc = new FavoriteService(Repo.Object);
        }
    }
}
