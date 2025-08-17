using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.PromotionPackageDTO;
using RealEstateManagement.Business.Repositories.Package;
using RealEstateManagement.Business.Services.PromotionPackages;
using RealEstateManagement.Data.Entity.Payment;
using System;
using System.Threading.Tasks;
namespace RealEstateManagement.UnitTests.PromotionPakageTest
{
    [TestClass]
    public class GetByIdTestUnit
    {
        private const string V = "Level C";
        private Mock<IPromotionPackageRepository> _mockRepo;
        private PromotionPackageService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IPromotionPackageRepository>();
            _service = new PromotionPackageService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task CreateAsync_WithValidDto_ReturnsCreatedPackage(int v)
        {
            var createDto = new CreatePromotionPackageDTO
            {
                Name = "New Package",
                Description = "New Description",
                Price = 50000,
                DurationInDays = 15,
                Level = v,
                IsActive = true
            };

            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<PromotionPackage>()))
                     .Returns(Task.CompletedTask)
                     .Callback<PromotionPackage>(entity => entity.Id = 3);

            var result = await _service.CreateAsync(createDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Id);
            Assert.AreEqual(createDto.Name, result.Name);
            Assert.AreEqual(createDto.Price, result.Price);
            Assert.AreEqual(createDto.DurationInDays, result.DurationInDays);
            Assert.AreEqual(createDto.Level, result.Level);
            Assert.AreEqual(createDto.IsActive, result.IsActive);

            _mockRepo.Verify(repo => repo.AddAsync(It.Is<PromotionPackage>(p =>
                p.Name == createDto.Name &&
                p.Description == createDto.Description &&
                p.Price == createDto.Price &&
                p.DurationInDays == createDto.DurationInDays &&
                p.Level == createDto.Level &&
                p.IsActive == createDto.IsActive
            )), Times.Once);
        }
    }
}