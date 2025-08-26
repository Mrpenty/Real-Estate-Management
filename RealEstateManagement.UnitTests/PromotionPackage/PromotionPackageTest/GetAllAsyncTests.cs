using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PromotionPackage.PromotionPackageTest
{
    [TestClass]
    public class GetAllAsyncTests : PromotionPackageTestBase
    {
        [TestMethod]
        public async Task Returns_All_When_Exists()
        {
            var entities = new List<Data.Entity.Payment.PromotionPackage>
        {
            new() { Id = 1, Name = "Basic", Price = 100, DurationInDays = 30, Level = 1, CreatedAt = DateTime.UtcNow, IsActive = true }
        };
            Repo.Setup(r => r.GetAsync()).ReturnsAsync(entities);

            var result = await Svc.GetAllAsync();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Basic", result.First().Name);
        }

        [TestMethod]
        public async Task Returns_Empty_When_None()
        {
            Repo.Setup(r => r.GetAsync()).ReturnsAsync(new List<Data.Entity.Payment.PromotionPackage>());

            var result = await Svc.GetAllAsync();

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Maps_Entity_To_DTO_Correctly()
        {
            var entity = new Data.Entity.Payment.PromotionPackage
            {
                Id = 2,
                Name = "Pro",
                Description = "Desc",
                Price = 200,
                DurationInDays = 60,
                Level = 2,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            Repo.Setup(r => r.GetAsync()).ReturnsAsync(new List<Data.Entity.Payment.PromotionPackage> { entity });

            var result = (await Svc.GetAllAsync()).First();

            Assert.AreEqual(entity.Name, result.Name);
            Assert.AreEqual(entity.Description, result.Description);
            Assert.AreEqual(entity.Price, result.Price);
        }
    }


}
