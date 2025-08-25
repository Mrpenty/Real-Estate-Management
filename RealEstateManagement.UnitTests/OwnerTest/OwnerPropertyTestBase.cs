using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest
{
    public abstract class OwnerPropertyTestBase
    {
        protected Mock<IOwnerPropertyRepository> Repo = null!;
        protected Mock<RentalDbContext> Db = null!;
        protected Mock<IRentalContractRepository> RentalContractRepo = null!; // 👈 thêm
        protected OwnerPropertyService Svc = null!;

        // Giữ 1-2 DbSet mẫu. Thêm cái bạn cần tùy logic service.
        protected Mock<DbSet<PropertyPost>> PropertyPostSet = null!;

        [TestInitialize]
        public void Init()
        {
            Repo = new Mock<IOwnerPropertyRepository>(MockBehavior.Strict);
            RentalContractRepo = new Mock<IRentalContractRepository>(MockBehavior.Strict);

            // KHÔNG dùng InMemory: mock DbContext
            Db = new Mock<RentalDbContext>(new DbContextOptions<RentalDbContext>()) { CallBase = false };

            // ===== Mock DbSet<PropertyPost> & Db.Set<PropertyPost>() =====
            var seedPosts = new List<PropertyPost>().AsQueryable();
            PropertyPostSet = CreateQueryableDbSet(seedPosts);
            Db.Setup(d => d.Set<PropertyPost>()).Returns(PropertyPostSet.Object);

            // Nếu service gọi SaveChangesAsync:
            Db
              .Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()))
              .ReturnsAsync(1);

            // ===== Cuối cùng: tạo service với đủ 3 tham số =====
            Svc = new OwnerPropertyService(Repo.Object, Db.Object, RentalContractRepo.Object);
        }

        // Helper tạo Mock<DbSet<T>> hoạt động như IQueryable (đủ cho LINQ ToObjects trong unit test)
        protected static Mock<DbSet<T>> CreateQueryableDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());

            // Nếu service có Add/Update/Remove, stub tối thiểu:
            mockSet.Setup(s => s.Add(It.IsAny<T>())).Callback<T>(_ => { /* optionally mutate a backing list */ });
            mockSet.Setup(s => s.Update(It.IsAny<T>())).Callback<T>(_ => { });
            mockSet.Setup(s => s.Remove(It.IsAny<T>())).Callback<T>(_ => { });

            return mockSet;
        }
    }
}
