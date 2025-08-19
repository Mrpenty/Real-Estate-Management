using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyImageTest
{
    [TestClass]
    public class DeleteImageAsyncTests : PropertyImageTestBase
    {
        [TestMethod]
        public async Task Returns_True_When_Success()
        {
            ImageRepo.Setup(r => r.PropertyExistsAsync(1)).ReturnsAsync(true);
            ImageRepo.Setup(r => r.DeleteImageAsync(1, 99)).ReturnsAsync(true);

            var result = await Svc.DeleteImageAsync(1, 99);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Returns_False_When_Property_NotFound()
        {
            ImageRepo.Setup(r => r.PropertyExistsAsync(1)).ReturnsAsync(false);

            var result = await Svc.DeleteImageAsync(1, 10);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Returns_False_When_Delete_Fails()
        {
            ImageRepo.Setup(r => r.PropertyExistsAsync(1)).ReturnsAsync(true);
            ImageRepo.Setup(r => r.DeleteImageAsync(1, 99)).ReturnsAsync(false);

            var result = await Svc.DeleteImageAsync(1, 99);

            Assert.IsFalse(result);
        }
    }

}
