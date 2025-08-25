using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.OwnerPropertyTest
{
    [TestClass]
    public class DeletePropertyAsyncTests : OwnerPropertyTestBase
    {
        [TestMethod]
        public async Task Throws_When_NotFound()
        {
            Repo.Setup(r => r.GetByIdAsync(6, 2))
                .ReturnsAsync((Property)null!);

            await Assert.ThrowsExceptionAsync<System.Exception>(() =>
                Svc.DeletePropertyAsync(6, 2));

            Repo.Verify(r => r.GetByIdAsync(6, 2), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Calls_Delete_When_Found()
        {
            var property = new Property { Id = 6 };
            Repo.Setup(r => r.GetByIdAsync(6, 2)).ReturnsAsync(property);
            Repo.Setup(r => r.DeleteAsync(property)).Returns(Task.CompletedTask);

            await Svc.DeletePropertyAsync(6, 2);

            Repo.Verify(r => r.GetByIdAsync(6, 2), Times.Once);
            Repo.Verify(r => r.DeleteAsync(property), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
