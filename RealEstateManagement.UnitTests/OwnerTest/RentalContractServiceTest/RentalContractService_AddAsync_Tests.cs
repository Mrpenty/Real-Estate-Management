using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.RentalContractServiceTest
{
    [TestClass]
    public class RentalContractService_AddAsync_Tests : RentalContractTestBase
    {
        [TestMethod]
        public async Task AddAsync_CallsRepo_WhenValid()
        {
            var dto = new RentalContractCreateDto
            {
                DepositAmount = 100,
                MonthlyRent = 1000,
                ContractDurationMonths = 12,
                ContactInfo = "abc",
                PaymentMethod = "Cash",
                PaymentDayOfMonth = 5,
                PaymentCycle = RentalContract.PaymentCycleType.Monthly,
                StartDate = DateTime.UtcNow
            };

            var post = new PropertyPost { Id = 10 };

            PostRepo.Setup(r => r.GetPropertyPostByIdAsync(10, 1)).ReturnsAsync(post);
            PostRepo.Setup(r => r.UpdateAsync(post)).Returns(Task.CompletedTask);
            ContractRepo.Setup(r => r.AddAsync(It.IsAny<RentalContract>(), 1, 10)).Returns(Task.CompletedTask);

            await Svc.AddAsync(dto, 1, 10);

            PostRepo.Verify(r => r.UpdateAsync(It.IsAny<PropertyPost>()), Times.Once);
            ContractRepo.Verify(r => r.AddAsync(It.IsAny<RentalContract>(), 1, 10), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task AddAsync_Throws_WhenInvalidMonthlyRent()
        {
            var dto = new RentalContractCreateDto
            {
                DepositAmount = 100,
                MonthlyRent = 0, // ❌ invalid
                ContractDurationMonths = 12,
                ContactInfo = "abc",
                PaymentMethod = "Cash",
                PaymentDayOfMonth = 5,
                PaymentCycle = RentalContract.PaymentCycleType.Monthly,
                StartDate = DateTime.UtcNow
            };

            await Svc.AddAsync(dto, 1, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task AddAsync_Throws_WhenPostNotFound()
        {
            var dto = new RentalContractCreateDto
            {
                DepositAmount = 100,
                MonthlyRent = 1000,
                ContractDurationMonths = 12,
                ContactInfo = "abc",
                PaymentMethod = "Cash",
                PaymentDayOfMonth = 5,
                PaymentCycle = RentalContract.PaymentCycleType.Monthly,
                StartDate = DateTime.UtcNow
            };

            PostRepo.Setup(r => r.GetPropertyPostByIdAsync(10, 1))
                    .ReturnsAsync((PropertyPost)null!); // ❌ không tìm thấy post

            await Svc.AddAsync(dto, 1, 10);
        }
    }
}
