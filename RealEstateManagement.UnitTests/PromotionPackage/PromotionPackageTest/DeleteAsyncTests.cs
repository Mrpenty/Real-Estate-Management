using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PromotionPackage.PromotionPackageTest
{
    [TestClass]
    public class DeleteAsyncTests : PromotionPackageTestBase
    {
        [TestMethod]
        public async Task Returns_True_When_Deleted()
        {
            var entity = new Data.Entity.Payment.PromotionPackage { Id = 9 };
            Repo.Setup(r => r.GetByIdAsync(9)).ReturnsAsync(entity);
            Repo.Setup(r => r.DeleteAsync(9)).Returns(Task.CompletedTask);

            var result = await Svc.DeleteAsync(9);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Returns_False_When_NotFound()
        {
            Repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Data.Entity.Payment.PromotionPackage?)null);

            var result = await Svc.DeleteAsync(99);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Calls_DeleteAsync_When_Entity_Exists()
        {
            var entity = new Data.Entity.Payment.PromotionPackage { Id = 11 };
            Repo.Setup(r => r.GetByIdAsync(11)).ReturnsAsync(entity);
            Repo.Setup(r => r.DeleteAsync(11)).Returns(Task.CompletedTask).Verifiable();

            var result = await Svc.DeleteAsync(11);

            Assert.IsTrue(result);
            Repo.Verify(r => r.DeleteAsync(11), Times.Once);
        }
    }

}
