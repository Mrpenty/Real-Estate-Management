using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyPostServiceTest
{
    [TestClass]
    public class UpdatePostStatusAsync_InvalidStatusTests : PropertyPostTestBase
    {
        [TestMethod]
        public async Task Throws_When_Status_Invalid()
        {
            var post = new PropertyPost { Id = 9 };
            PostRepo.Setup(r => r.GetByIdAsync(9)).ReturnsAsync(post);

            await Assert.ThrowsExceptionAsync<System.ArgumentException>(() =>
                Svc.UpdatePostStatusAsync(9, "not-a-valid-status"));

            PostRepo.Verify(r => r.GetByIdAsync(9), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }
    }
}
