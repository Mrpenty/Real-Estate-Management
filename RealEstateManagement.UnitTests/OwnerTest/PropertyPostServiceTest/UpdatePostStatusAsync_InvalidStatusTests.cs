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
        [TestMethod]
        public async Task Updates_Status_To_Approved()
        {
            var post = new PropertyPost { Id = 10, Status = PropertyPost.PropertyPostStatus.Pending };
            PostRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(post);
            PostRepo.Setup(r => r.UpdateAsync(post)).Returns(Task.CompletedTask);

            await Svc.UpdatePostStatusAsync(10, "Approved");

            Assert.AreEqual(PropertyPost.PropertyPostStatus.Approved, post.Status);
            PostRepo.Verify(r => r.UpdateAsync(post), Times.Once);
            PostRepo.Verify(r => r.GetByIdAsync(10), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }
        [TestMethod]
        public async Task Throws_When_Post_NotFound()
        {
            PostRepo.Setup(r => r.GetByIdAsync(404)).ReturnsAsync((PropertyPost)null!);

            await Assert.ThrowsExceptionAsync<Exception>(() => Svc.UpdatePostStatusAsync(404, "Approved"));

            PostRepo.Verify(r => r.GetByIdAsync(404), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }
        [TestMethod]
        public async Task NoUpdate_When_Status_Unchanged()
        {
            var post = new PropertyPost { Id = 11, Status = PropertyPost.PropertyPostStatus.Approved };
            PostRepo.Setup(r => r.GetByIdAsync(11)).ReturnsAsync(post);

            await Svc.UpdatePostStatusAsync(11, "Approved");

            // Kỳ vọng không gọi Update vì không có thay đổi
            PostRepo.Verify(r => r.UpdateAsync(It.IsAny<PropertyPost>()), Times.Never); // ❌ sẽ fail nếu service vẫn gọi
            PostRepo.Verify(r => r.GetByIdAsync(11), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }


    }
}
