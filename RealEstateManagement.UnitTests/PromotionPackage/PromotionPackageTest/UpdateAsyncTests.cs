using Moq;
using RealEstateManagement.Business.DTO.PromotionPackageDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PromotionPackage.PromotionPackageTest
{
    [TestClass]
    public class UpdateAsyncTests : PromotionPackageTestBase
    {
        [TestMethod]
        public async Task Updates_And_Returns_DTO()
        {
            var existing = new Data.Entity.Payment.PromotionPackage { Id = 7, Name = "Old", Price = 100, DurationInDays = 30, Level = 1, CreatedAt = DateTime.UtcNow, IsActive = true };
            Repo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);
            Repo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            var dto = new UpdatePromotionPackageDTO { Name = "Updated", Description = "New Desc", Price = 150, DurationInDays = 40, Level = 2, IsActive = false };

            var result = await Svc.UpdateAsync(7, dto);

            Assert.IsNotNull(result);
            Assert.AreEqual("Updated", result!.Name);
            Assert.AreEqual(150, result.Price);
            Assert.IsFalse(result.IsActive);
        }

        [TestMethod]
        public async Task Returns_Null_When_NotFound()
        {
            Repo.Setup(r => r.GetByIdAsync(77)).ReturnsAsync((Data.Entity.Payment.PromotionPackage?)null);

            var dto = new UpdatePromotionPackageDTO { Name = "X", Price = 1, DurationInDays = 1, Level = 1, IsActive = true };

            var result = await Svc.UpdateAsync(77, dto);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Updates_UpdatedAt_Field()
        {
            var existing = new Data.Entity.Payment.PromotionPackage { Id = 8, Name = "Old", CreatedAt = DateTime.UtcNow.AddDays(-5), UpdatedAt = null };
            Repo.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(existing);
            Repo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            var dto = new UpdatePromotionPackageDTO { Name = "New", Description = "Desc", Price = 500, DurationInDays = 50, Level = 5, IsActive = true };

            var result = await Svc.UpdateAsync(8, dto);

            Assert.IsNotNull(existing.UpdatedAt);
            Assert.IsTrue(existing.UpdatedAt!.Value <= DateTime.UtcNow);
        }
    }

}
