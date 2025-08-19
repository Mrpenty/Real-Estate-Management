using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyImageTest
{
    [TestClass]
    public class UpdateImageAsyncTests : PropertyImageTestBase
    {
        [TestMethod]
        public async Task Returns_Updated_Image()
        {
            var image = new PropertyImage { Id = 1, PropertyId = 2, Url = "http://old.jpg" };
            ImageRepo.Setup(r => r.UpdateImageAsync(image))
                     .ReturnsAsync(new PropertyImage { Id = 1, PropertyId = 2, Url = "http://new.jpg" });

            var result = await Svc.UpdateImageAsync(image);

            Assert.AreEqual("http://new.jpg", result.Url);
            ImageRepo.Verify(r => r.UpdateImageAsync(image), Times.Once);
        }

        [TestMethod]
        public async Task Returns_Null_When_NotFound()
        {
            var image = new PropertyImage { Id = 100 };
            ImageRepo.Setup(r => r.UpdateImageAsync(image)).ReturnsAsync((PropertyImage)null!);

            var result = await Svc.UpdateImageAsync(image);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Updates_Successfully()
        {
            var image = new PropertyImage { Id = 10, Url = "http://x.jpg" };
            ImageRepo.Setup(r => r.UpdateImageAsync(image)).ReturnsAsync(image);

            var result = await Svc.UpdateImageAsync(image);

            Assert.AreEqual(10, result.Id);
            ImageRepo.Verify(r => r.UpdateImageAsync(image), Times.Once);
        }
    }

}
