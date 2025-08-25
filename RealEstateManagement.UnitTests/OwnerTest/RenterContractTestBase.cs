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
    public abstract class RentalContractTestBase
    {
        protected Mock<IRentalContractRepository> ContractRepo = null!;
        protected Mock<IPropertyPostRepository> PostRepo = null!;
        protected RentalContractService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            ContractRepo = new Mock<IRentalContractRepository>(MockBehavior.Strict);
            PostRepo = new Mock<IPropertyPostRepository>(MockBehavior.Strict);
            Svc = new RentalContractService(ContractRepo.Object, PostRepo.Object);
        }
    }
}
