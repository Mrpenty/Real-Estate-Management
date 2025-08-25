using Microsoft.EntityFrameworkCore;
using Moq;
using RealEstateManagement.Business.Repositories.FavoriteRepository;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.UnitTests.PropertiesTest
{
    public abstract class PropertyTestBase
    {
        protected Mock<IPropertyRepository> PropertyRepo = null!;
        protected Mock<IFavoriteRepository> FavoriteRepo = null!;
        protected RentalDbContext DbContext = null!;
        protected PropertyService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            PropertyRepo = new Mock<IPropertyRepository>(MockBehavior.Strict);
            FavoriteRepo = new Mock<IFavoriteRepository>(MockBehavior.Strict);

            // Tạo DbContext in-memory
            var options = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            DbContext = new RentalDbContext(options);

            Svc = new PropertyService(PropertyRepo.Object, FavoriteRepo.Object, DbContext);
        }
    }
}
