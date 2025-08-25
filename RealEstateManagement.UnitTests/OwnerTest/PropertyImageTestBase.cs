using Microsoft.Extensions.Logging;
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
    public abstract class PropertyImageTestBase
    {
        protected Mock<IPropertyImageRepository> ImageRepo = null!;
        protected Mock<IPropertyPostRepository> PostRepo = null!;
        protected Mock<ILogger<PropertyImageService>> Logger = null!;
        protected PropertyImageService Svc = null!;

        [TestInitialize]
        public void Init()
        {
            ImageRepo = new Mock<IPropertyImageRepository>(MockBehavior.Strict);
            PostRepo = new Mock<IPropertyPostRepository>(MockBehavior.Strict);
            Logger = new Mock<ILogger<PropertyImageService>>();
            Svc = new PropertyImageService(ImageRepo.Object, PostRepo.Object, Logger.Object);
        }
    }
}
