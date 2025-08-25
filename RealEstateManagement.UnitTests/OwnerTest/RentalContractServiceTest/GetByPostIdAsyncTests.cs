// OwnerTest/RentalContractServiceTest/GetByPostIdAsyncTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.RentalContractServiceTest
{
    [TestClass]
    public class GetByPostIdAsyncTests : RentalContractTestBase
    {
        [TestMethod]
        public async Task Throws_When_NotFound()
        {
            ContractRepo.Setup(r => r.GetByPostIdAsync(99)).ReturnsAsync((RentalContract)null!);

            await Assert.ThrowsExceptionAsync<Exception>(() => Svc.GetByPostIdAsync(99));

            ContractRepo.Verify(r => r.GetByPostIdAsync(99), Times.Once);
            ContractRepo.VerifyNoOtherCalls();
            PostRepo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Maps_Entity_To_Dto_When_Found()
        {
            var rc = new RentalContract
            {
                Id = 10,
                PropertyPostId = 7,
                DepositAmount = 100,
                MonthlyRent = 200,
                ContractDurationMonths = 12,
                PaymentCycle = RentalContract.PaymentCycleType.Monthly,
                PaymentDayOfMonth = 5,
                StartDate = DateTime.UtcNow.Date,
                //EndDate = DateTime.UtcNow.Date.AddMonths(12),
                PaymentMethod = "Cash",
                ContactInfo = "0123",
                Status = RentalContract.ContractStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ConfirmedAt = null
            };
            ContractRepo.Setup(r => r.GetByPostIdAsync(7)).ReturnsAsync(rc);

            RentalContractViewDto dto = await Svc.GetByPostIdAsync(7);

            Assert.AreEqual(10, dto.Id);
            Assert.AreEqual(200, dto.MonthlyRent);

            ContractRepo.Verify(r => r.GetByPostIdAsync(7), Times.Once);
            ContractRepo.VerifyNoOtherCalls();
            PostRepo.VerifyNoOtherCalls();
        }
        [TestMethod]
        public async Task GetByPostIdAsync_ReturnsDto_WhenExists()
        {
            var contract = new RentalContract { Id = 1, PropertyPostId = 10, MonthlyRent = 1000 };
            ContractRepo.Setup(r => r.GetByPostIdAsync(10)).ReturnsAsync(contract);

            var result = await Svc.GetByPostIdAsync(10);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(1000, result.MonthlyRent);
            ContractRepo.Verify(r => r.GetByPostIdAsync(10), Times.Once);
        }
    }
}
