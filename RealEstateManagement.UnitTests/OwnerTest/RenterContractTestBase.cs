using Moq;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Business.Services.User;
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
        protected Mock<IProfileService> ProfileSvc = null!;
        protected Mock<IMailService> MailSvc = null!;
        protected Mock<IPropertyRepository> PropertyRepo = null!;
        protected RentalContractService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            // Nên để Loose để không cần setup những call không dùng trong test
            ContractRepo = new Mock<IRentalContractRepository>(MockBehavior.Loose);
            PostRepo = new Mock<IPropertyPostRepository>(MockBehavior.Loose);
            ProfileSvc = new Mock<IProfileService>(MockBehavior.Loose);
            MailSvc = new Mock<IMailService>(MockBehavior.Loose);
            PropertyRepo = new Mock<IPropertyRepository>(MockBehavior.Loose);

            Svc = new RentalContractService(
                ContractRepo.Object,
                PostRepo.Object,
                ProfileSvc.Object,
                MailSvc.Object,
                PropertyRepo.Object
            );
        }
    }
}
