using Microsoft.EntityFrameworkCore;
using Moq;
using RealEstateManagement.Business.Repositories.AddressRepo;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest
{
    public abstract class PropertyPostTestBase
    {
        protected Mock<IPropertyPostRepository> PostRepo = null!;
        protected Mock<IAddressRepository> AddressRepo = null!;
        protected Mock<IPropertyImageRepository> ImageRepo = null!;
        protected Mock<RentalDbContext> Db = null!;
        protected PropertyPostService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            PostRepo = new Mock<IPropertyPostRepository>(MockBehavior.Strict);
            AddressRepo = new Mock<IAddressRepository>(MockBehavior.Strict);
            ImageRepo = new Mock<IPropertyImageRepository>(MockBehavior.Strict);

            var options = new DbContextOptionsBuilder<RentalDbContext>().Options;
            Db = new Mock<RentalDbContext>(options) { CallBase = false };

            Svc = new PropertyPostService(PostRepo.Object, AddressRepo.Object, ImageRepo.Object, Db.Object);
        }
    }
}
