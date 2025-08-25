using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.RentalContractServiceTest
{
    [TestClass]
    public class RentalContractService_Delete_Tests : RentalContractTestBase
    {
        [TestMethod]
        public async Task DeleteAsync_CallsRepo()
        {
            ContractRepo.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

            await Svc.DeleteAsync(1);

            ContractRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_Allows_Delete_NonExisting()
        {
            // Repo vẫn được gọi, không throw (tùy repo implement)
            ContractRepo.Setup(r => r.DeleteAsync(99)).Returns(Task.CompletedTask);

            await Svc.DeleteAsync(99);

            ContractRepo.Verify(r => r.DeleteAsync(99), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public async Task DeleteAsync_Throws_WhenRepoThrows()
        {
            ContractRepo.Setup(r => r.DeleteAsync(1)).ThrowsAsync(new System.Exception("DB error"));

            await Svc.DeleteAsync(1);
        }
    }
}
