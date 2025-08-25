using Moq;
using RealEstateManagement.Business.Repositories.Package;
using RealEstateManagement.Business.Services.PromotionPackages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PromotionPackage
{
    public abstract class PromotionPackageTestBase
    {
        protected Mock<IPromotionPackageRepository> Repo = null!;
        protected PromotionPackageService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            Repo = new Mock<IPromotionPackageRepository>(MockBehavior.Strict);
            Svc = new PromotionPackageService(Repo.Object);
        }
    }
}
