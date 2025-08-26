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
    public class RentalContractService_UpdateStatus_Tests : RentalContractTestBase
    {
        [TestMethod]
        public async Task UpdateStatusAsync_Updates_WhenValid()
        {
            var contract = new RentalContract { Id = 1, Status = RentalContract.ContractStatus.Pending };

            ContractRepo.Setup(r => r.GetByPostIdAsync(1)).ReturnsAsync(contract);
            ContractRepo.Setup(r => r.UpdateStatusAsync(contract.Id, RentalContract.ContractStatus.Confirmed))
                        .Returns(Task.CompletedTask);

            var dto = new RentalContractStatusDto { ContractId = 1, Status = RentalContract.ContractStatus.Confirmed };

            await Svc.UpdateStatusAsync(dto);

            ContractRepo.Verify(r => r.UpdateStatusAsync(1, RentalContract.ContractStatus.Confirmed), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UpdateStatusAsync_Throws_WhenNotFound()
        {
            ContractRepo.Setup(r => r.GetByPostIdAsync(1)).ReturnsAsync((RentalContract)null!);

            var dto = new RentalContractStatusDto { ContractId = 1, Status = RentalContract.ContractStatus.Confirmed };

            await Svc.UpdateStatusAsync(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task UpdateStatusAsync_Throws_WhenInvalidStatus()
        {
            var contract = new RentalContract { Id = 1, Status = RentalContract.ContractStatus.Pending };

            ContractRepo.Setup(r => r.GetByPostIdAsync(1)).ReturnsAsync(contract);

            // giả sử cast int không hợp lệ
            var dto = new RentalContractStatusDto { ContractId = 1, Status = (RentalContract.ContractStatus)999 };

            await Svc.UpdateStatusAsync(dto);
        }
    }
}
