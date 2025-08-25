using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;

namespace RealEstateManagement.UnitTests.OwnerTest
{
    public abstract class OwnerPropertyTestBase
    {
        protected Mock<IOwnerPropertyRepository> Repo = null!;
        protected Mock<RentalDbContext> Db = null!;
        protected OwnerPropertyService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            Repo = new Mock<IOwnerPropertyRepository>(MockBehavior.Strict);

            // Tạo Mock DbContext, KHÔNG dùng InMemory
            Db = new Mock<RentalDbContext>(new DbContextOptions<RentalDbContext>()) { CallBase = false };

            Svc = new OwnerPropertyService(Repo.Object, Db.Object);
        }
    }
}
