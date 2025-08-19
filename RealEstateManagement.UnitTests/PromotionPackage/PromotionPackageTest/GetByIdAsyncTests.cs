using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PromotionPackage.PromotionPackageTest
{
    [TestClass]
    public class GetByIdAsyncTests : PromotionPackageTestBase
    {
        [TestMethod]
        public async Task Returns_DTO_When_Found()
        {
            var entity = new RealEstateManagement.Data.Entity.Payment.PromotionPackage
            {
                Id = 5,
                Name = "Pro",
                Price = 200,
                DurationInDays = 60,
                Level = 2,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            Repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(entity);

            var result = await Svc.GetByIdAsync(5);

            Assert.IsNotNull(result);
            Assert.AreEqual("Pro", result!.Name);
        }

        [TestMethod]
        public async Task Returns_Null_When_NotFound()
        {
            Repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((RealEstateManagement.Data.Entity.Payment.PromotionPackage?)null);

            var result = await Svc.GetByIdAsync(99);

            Assert.IsNull(result);
        }
    }

}
