using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.Reviews;
using RealEstateManagement.Business.Repositories.Reviews;
using RealEstateManagement.Data.Entity.Reviews;
using System.Threading.Tasks;
using System;

namespace RealEstateManagement.UnitTests.Reviews.ReviewServiceTest
{
    [TestClass]
    public class EditReplyAsyncTests
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
        public async Task Fail_When_Reply_NotFound()
        {
            _repoMock.Setup(r => r.GetReplyByReviewIdAsync(5)).ReturnsAsync((ReviewReply)null);

            var (ok, msg) = await _service.EditReplyAsync(5, 10, "new");

            Assert.IsFalse(ok);
            Assert.AreEqual("Không tìm thấy trả lời hoặc không có quyền.", msg);
        }

        [TestMethod]
        public async Task Fail_When_NotOwner()
        {
            var reply = new ReviewReply { Id = 5, LandlordId = 99, CreatedAt = DateTime.Now };
            _repoMock.Setup(r => r.GetReplyByReviewIdAsync(5)).ReturnsAsync(reply);

            var (ok, msg) = await _service.EditReplyAsync(5, 10, "new");

            Assert.IsFalse(ok);
        }

        [TestMethod]
        public async Task Fail_When_TooLate()
        {
            var reply = new ReviewReply { Id = 5, LandlordId = 10, CreatedAt = DateTime.Now.AddMinutes(-20) };
            _repoMock.Setup(r => r.GetReplyByReviewIdAsync(5)).ReturnsAsync(reply);

            var (ok, msg) = await _service.EditReplyAsync(5, 10, "new");

            Assert.IsFalse(ok);
            Assert.AreEqual("Chỉ được sửa trong 5 phút đầu.", msg);
        }

        [TestMethod]
        public async Task Success_UpdateReply()
        {
            var reply = new ReviewReply { Id = 5, LandlordId = 10, CreatedAt = DateTime.Now };
            _repoMock.Setup(r => r.GetReplyByReviewIdAsync(5)).ReturnsAsync(reply);
            _repoMock.Setup(r => r.UpdateReplyAsync(reply)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var (ok, msg) = await _service.EditReplyAsync(5, 10, "Updated");

            Assert.IsTrue(ok);
            Assert.AreEqual("Cập nhật trả lời thành công", msg);
            Assert.AreEqual("Updated", reply.ReplyContent);
        }
    }
}
