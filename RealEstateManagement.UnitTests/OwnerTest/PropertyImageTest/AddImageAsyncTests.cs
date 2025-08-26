using Moq;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyImageTest
{
    [TestClass]
    public class AddImageAsyncTests : PropertyImageTestBase
    {
        [TestMethod]
        public async Task Returns_Image_When_Valid()
        {
            var dto = new PropertyImageCreateDto { Url = "http://img.jpg", IsPrimary = true, Order = 1 };

            ImageRepo.Setup(r => r.PropertyExistsAsync(1)).ReturnsAsync(true);
            ImageRepo.Setup(r => r.AddImageAsync(It.IsAny<PropertyImage>()))
                     .ReturnsAsync(new PropertyImage { Id = 99, PropertyId = 1, Url = dto.Url });

            var result = await Svc.AddImageAsync(1, dto);

            Assert.IsNotNull(result);
            Assert.AreEqual(99, result.Id);
            ImageRepo.Verify(r => r.PropertyExistsAsync(1), Times.Once);
            ImageRepo.Verify(r => r.AddImageAsync(It.IsAny<PropertyImage>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Throws_When_Property_NotFound()
        {
            var dto = new PropertyImageCreateDto { Url = "http://img.jpg", IsPrimary = false, Order = 0 };
            ImageRepo.Setup(r => r.PropertyExistsAsync(5)).ReturnsAsync(false);

            await Svc.AddImageAsync(5, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task Throws_When_Url_Invalid()
        {
            var dto = new PropertyImageCreateDto { Url = "", IsPrimary = false, Order = 0 };
            ImageRepo.Setup(r => r.PropertyExistsAsync(1)).ReturnsAsync(true);

            await Svc.AddImageAsync(1, dto);
        }
    }

}
