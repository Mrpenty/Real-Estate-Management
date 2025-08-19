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
    public class CreateAsyncTests : PromotionPackageTestBase
    {
        [TestMethod]
        public async Task Creates_And_Returns_DTO()
        {
            var dto = new CreatePromotionPackageDTO { Name = "Premium", Description = "Best", Price = 300, DurationInDays = 90, Level = 3, IsActive = true };

            Repo.Setup(r => r.AddAsync(It.IsAny<Data.Entity.Payment.PromotionPackage>()))
                .Returns(Task.CompletedTask)
                .Callback<Data.Entity.Payment.PromotionPackage>(p => p.Id = 10);

            var result = await Svc.CreateAsync(dto);

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Id);
            Assert.AreEqual("Premium", result.Name);
        }

        [TestMethod]
        public async Task Sets_CreatedAt_Automatically()
        {
            var dto = new CreatePromotionPackageDTO { Name = "Test", Description = "Desc", Price = 100, DurationInDays = 10, Level = 1, IsActive = true };

            Data.Entity.Payment.PromotionPackage? savedEntity = null;
            Repo.Setup(r => r.AddAsync(It.IsAny<Data.Entity.Payment.PromotionPackage>()))
                .Returns(Task.CompletedTask)
                .Callback<Data.Entity.Payment.PromotionPackage>(p => savedEntity = p);

            var result = await Svc.CreateAsync(dto);

            Assert.IsNotNull(savedEntity);
            Assert.IsTrue(savedEntity!.CreatedAt <= DateTime.UtcNow);
            Assert.IsNull(savedEntity.UpdatedAt);
        }

        [TestMethod]
        public async Task Copies_All_Fields_From_DTO()
        {
            var dto = new CreatePromotionPackageDTO { Name = "CopyTest", Description = "CopyDesc", Price = 111, DurationInDays = 22, Level = 2, IsActive = false };

            Data.Entity.Payment.PromotionPackage? savedEntity = null;
            Repo.Setup(r => r.AddAsync(It.IsAny<Data.Entity.Payment.PromotionPackage>()))
                .Returns(Task.CompletedTask)
                .Callback<Data.Entity.Payment.PromotionPackage>(p => savedEntity = p);

            var result = await Svc.CreateAsync(dto);

            Assert.AreEqual(dto.Name, savedEntity!.Name);
            Assert.AreEqual(dto.Description, savedEntity.Description);
            Assert.AreEqual(dto.Price, savedEntity.Price);
            Assert.AreEqual(dto.IsActive, savedEntity.IsActive);
        }
    }

}
