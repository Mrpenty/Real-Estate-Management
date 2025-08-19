using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyImageTest
{
    [TestClass]
    public class ClearAllImagesAsyncTests : PropertyImageTestBase
    {
        [TestMethod]
        public async Task Returns_True_When_Success()
        {
            ImageRepo.Setup(r => r.PropertyExistsAsync(1)).ReturnsAsync(true);
            ImageRepo.Setup(r => r.ClearAllImagesAsync(1)).ReturnsAsync(true);

            var result = await Svc.ClearAllImagesAsync(1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Returns_False_When_Property_NotFound()
        {
            ImageRepo.Setup(r => r.PropertyExistsAsync(1)).ReturnsAsync(false);

            var result = await Svc.ClearAllImagesAsync(1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Returns_False_When_Exception()
        {
            ImageRepo.Setup(r => r.PropertyExistsAsync(1)).ThrowsAsync(new Exception("db error"));

            var result = await Svc.ClearAllImagesAsync(1);

            Assert.IsFalse(result);
        }
    }

}
