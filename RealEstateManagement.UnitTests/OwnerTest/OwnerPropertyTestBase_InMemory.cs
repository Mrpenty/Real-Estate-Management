using Microsoft.EntityFrameworkCore;
using Moq;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest
{
    public abstract class OwnerPropertyTestBase_InMemory
    {
        protected Mock<IOwnerPropertyRepository> Repo = null!;
        protected Mock<IRentalContractRepository> RentalContractRepo = null!;
        protected RentalDbContext Db = null!;
        protected OwnerPropertyService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            Repo = new Mock<IOwnerPropertyRepository>(MockBehavior.Strict);
            RentalContractRepo = new Mock<IRentalContractRepository>(MockBehavior.Strict);

            var options = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            Db = new RentalDbContext(options);
            Svc = new OwnerPropertyService(Repo.Object, Db, RentalContractRepo.Object);
        }
    }
}
