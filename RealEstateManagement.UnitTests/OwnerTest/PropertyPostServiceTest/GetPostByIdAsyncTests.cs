using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyPostServiceTest
{
    [TestClass]
    public class GetPostByIdAsyncTests : PropertyPostTestBase
    {
        [TestMethod]
        public async Task Returns_Post_From_Repo()
        {
            var post = new PropertyPost { Id = 42 };
            PostRepo.Setup(r => r.GetByIdAsync(42)).ReturnsAsync(post);

            var got = await Svc.GetPostByIdAsync(42);

            Assert.AreEqual(42, got.Id);
            PostRepo.Verify(r => r.GetByIdAsync(42), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }
        [TestMethod]
        public async Task Returns_Null_When_Not_Found()
        {
            PostRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((PropertyPost)null!);

            var got = await Svc.GetPostByIdAsync(99);

            Assert.IsNull(got);
            PostRepo.Verify(r => r.GetByIdAsync(99), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Calls_Repo_Exactly_Once()
        {
            var post = new PropertyPost { Id = 77 };
            PostRepo.Setup(r => r.GetByIdAsync(77)).ReturnsAsync(post);

            var got = await Svc.GetPostByIdAsync(77);

            Assert.IsNotNull(got);
            Assert.AreEqual(77, got.Id);

            // ✅ Xác nhận repo chỉ được gọi đúng 1 lần
            PostRepo.Verify(r => r.GetByIdAsync(77), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }
    }
}
