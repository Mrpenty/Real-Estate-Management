using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.Reviews;
using RealEstateManagement.Business.Repositories.Reviews;
using RealEstateManagement.Data.Entity.Reviews;
using System.Threading.Tasks;
namespace RealEstateManagement.UnitTests.Reviews.ReviewServiceTest
{
    [TestClass]
    public class AddReplyAsyncTests
    {
        private Mock<IReviewRepository> _repoMock;
        private ReviewService _service;

        [TestInitialize]
        public void Setup()
        {
            _repoMock = new Mock<IReviewRepository>();
            _service = new ReviewService(_repoMock.Object);
        }

        [TestMethod]
        public async Task Fail_When_Review_NotFound()
        {
            _repoMock.Setup(r => r.GetReviewByIdAsync(1)).ReturnsAsync((Review)null);

            var (ok, msg) = await _service.AddReplyAsync(1, 10, "Reply");

            Assert.IsFalse(ok);
            Assert.AreEqual("Không tìm thấy review.", msg);
        }

        [TestMethod]
        public async Task Fail_When_AlreadyReplied()
        {
            var review = new Review { Id = 1, Reply = new ReviewReply() };
            _repoMock.Setup(r => r.GetReviewByIdAsync(1)).ReturnsAsync(review);

            var (ok, msg) = await _service.AddReplyAsync(1, 10, "Reply");

            Assert.IsFalse(ok);
            Assert.AreEqual("Bạn chỉ được trả lời 1 lần.", msg);
        }

        [TestMethod]
        public async Task Success_AddReply()
        {
            var review = new Review { Id = 1 };
            _repoMock.Setup(r => r.GetReviewByIdAsync(1)).ReturnsAsync(review);
            _repoMock.Setup(r => r.AddReplyAsync(It.IsAny<ReviewReply>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var (ok, msg) = await _service.AddReplyAsync(1, 10, "Hello");

            Assert.IsTrue(ok);
            Assert.AreEqual("Trả lời thành công", msg);
        }
    }
}
