using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyPostServiceTest
{
    [TestClass]
    public class UpdatePostStatusByPropertyIdAsync_InvalidStatusTests : PropertyPostTestBase
    {
        [TestMethod]
        public async Task Throws_When_Status_Invalid()
        {
            var post = new PropertyPost { Id = 2 };
            PostRepo.Setup(r => r.GetByPropertyIdAsync(200)).ReturnsAsync(post);

            await Assert.ThrowsExceptionAsync<System.ArgumentException>(() =>
                Svc.UpdatePostStatusByPropertyIdAsync(200, "invalid"));

            PostRepo.Verify(r => r.GetByPropertyIdAsync(200), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }
    }
}
