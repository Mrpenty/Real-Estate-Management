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
        [TestMethod]
        public async Task Updates_Status_To_Approved()
        {
            var post = new PropertyPost { Id = 10, Status = PropertyPost.PropertyPostStatus.Pending };
            PostRepo.Setup(r => r.GetByPropertyIdAsync(300)).ReturnsAsync(post);
            PostRepo.Setup(r => r.UpdateAsync(post)).Returns(Task.CompletedTask);

            await Svc.UpdatePostStatusByPropertyIdAsync(300, "Approved");

            Assert.AreEqual(PropertyPost.PropertyPostStatus.Approved, post.Status);
            PostRepo.Verify(r => r.UpdateAsync(post), Times.Once);
            PostRepo.Verify(r => r.GetByPropertyIdAsync(300), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }
        [TestMethod]
        public async Task Throws_When_Post_NotFound()
        {
            PostRepo.Setup(r => r.GetByPropertyIdAsync(404)).ReturnsAsync((PropertyPost)null!);

            await Assert.ThrowsExceptionAsync<Exception>(() =>
                Svc.UpdatePostStatusByPropertyIdAsync(404, "Approved"));

            PostRepo.Verify(r => r.GetByPropertyIdAsync(404), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task NoUpdate_When_Status_Unchanged()
        {
            var post = new PropertyPost { Id = 11, Status = PropertyPost.PropertyPostStatus.Approved };
            PostRepo.Setup(r => r.GetByPropertyIdAsync(400)).ReturnsAsync(post);

            await Svc.UpdatePostStatusByPropertyIdAsync(400, "Approved");

            PostRepo.Verify(r => r.UpdateAsync(It.IsAny<PropertyPost>()), Times.Never); 
            PostRepo.Verify(r => r.GetByPropertyIdAsync(400), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }

    }
}
