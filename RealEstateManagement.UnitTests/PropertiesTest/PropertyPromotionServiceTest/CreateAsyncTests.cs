using Microsoft.EntityFrameworkCore;
using Moq;
using RealEstateManagement.Business.DTO.PromotionPackageDTO;
using RealEstateManagement.Business.Repositories.Package;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateManagement.Data.Entity.PropertyEntity;
namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyPromotionServiceTest
{
    [TestClass]
    public class CreateAsyncTests
    {
        private Mock<IPropertyPromotionRepository> _mockRepo;
        private RentalDbContext _dbContext;
        private PropertyPromotionService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IPropertyPromotionRepository>();
            var options = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = new RentalDbContext(options);
            _service = new PropertyPromotionService(_mockRepo.Object, _dbContext);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Không tìm thấy ví")]
        public async Task Throws_When_Wallet_NotFound()
        {
            var dto = new CreatePropertyPromotionDTO { PropertyId = 1, PackageId = 1, StartDate = DateTime.UtcNow };
            await _service.CreateAsync(dto, 99);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Không tìm thấy gói hoặc gói không còn hoạt động")]
        public async Task Throws_When_Package_NotFound()
        {
            _dbContext.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 1, UserId = 10, Balance = 2000 });
            _dbContext.SaveChanges();

            var dto = new CreatePropertyPromotionDTO { PropertyId = 1, PackageId = 1, StartDate = DateTime.UtcNow };
            await _service.CreateAsync(dto, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Số dư ví không đủ")]
        public async Task Throws_When_NotEnough_Balance()
        {
            _dbContext.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 1, UserId = 10, Balance = 100 });
            _dbContext.promotionPackages.Add(new Data.Entity.Payment.PromotionPackage { Id = 1, Price = 200,
                Name = "Basic",                     // 👈 thêm vào
                Description = "Gói cơ bản",
                DurationInDays = 30, IsActive = true });
            _dbContext.SaveChanges();

            var dto = new CreatePropertyPromotionDTO { PropertyId = 1, PackageId = 1, StartDate = DateTime.UtcNow };
            await _service.CreateAsync(dto, 10);
        }

        [TestMethod]
        public async Task Success_Deducts_Balance_And_Creates()
        {
            var wallet = new Data.Entity.Payment.Wallet { Id = 1, UserId = 10, Balance = 2000 };
            var package = new Data.Entity.Payment.PromotionPackage
            {
                Id = 1,
                Name = "Gold",
                Description = "Gói cao cấp",   // 👈 BẮT BUỘC phải thêm
                Price = 1000,
                DurationInDays = 30,
                IsActive = true
            };
            _dbContext.Wallets.Add(wallet);
            _dbContext.promotionPackages.Add(package);
            _dbContext.SaveChanges();

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<PropertyPromotion>())).Returns(Task.CompletedTask);

            var dto = new CreatePropertyPromotionDTO { PropertyId = 55, PackageId = 1, StartDate = DateTime.UtcNow };

            var result = await _service.CreateAsync(dto, 10);

            Assert.AreEqual(1000, wallet.Balance);
            Assert.AreEqual(55, result.PropertyId);
            Assert.AreEqual(-1000, _dbContext.WalletTransactions.First().Amount);
        }

    }

}
