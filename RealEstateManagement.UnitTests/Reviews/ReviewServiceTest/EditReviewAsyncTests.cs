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
    public class EditReviewAsyncTests
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
            _repoMock.Setup(r => r.GetReviewByIdAsync(5)).ReturnsAsync((Review)null);

            var (ok, msg) = await _service.EditReviewAsync(5, 99, "New");

            Assert.IsFalse(ok);
            Assert.AreEqual("Không tìm thấy review.", msg);
        }

        [TestMethod]
        public async Task Fail_When_NotOwner()
        {
            var review = new Review { Id = 1, RenterId = 111, CreatedAt = DateTime.Now };
            _repoMock.Setup(r => r.GetReviewByIdAsync(1)).ReturnsAsync(review);

            var (ok, msg) = await _service.EditReviewAsync(1, 99, "New");

            Assert.IsFalse(ok);
            Assert.AreEqual("Không tìm thấy review.", msg);
        }

        [TestMethod]
        public async Task Fail_When_ReplyExists()
        {
            var review = new Review { Id = 1, RenterId = 99, CreatedAt = DateTime.Now, Reply = new ReviewReply() };
            _repoMock.Setup(r => r.GetReviewByIdAsync(1)).ReturnsAsync(review);

            var (ok, msg) = await _service.EditReviewAsync(1, 99, "New");

            Assert.IsFalse(ok);
            Assert.AreEqual("Chủ nhà đã trả lời, không thể chỉnh sửa.", msg);
        }

        [TestMethod]
        public async Task Fail_When_TooLate()
        {
            var review = new Review { Id = 1, RenterId = 99, CreatedAt = DateTime.Now.AddMinutes(-10) };
            _repoMock.Setup(r => r.GetReviewByIdAsync(1)).ReturnsAsync(review);

            var (ok, msg) = await _service.EditReviewAsync(1, 99, "New");

            Assert.IsFalse(ok);
            Assert.AreEqual("Chỉ được sửa trong 5 phút đầu.", msg);
        }

        [TestMethod]
        public async Task Success_UpdateReview()
        {
            var review = new Review { Id = 1, RenterId = 99, CreatedAt = DateTime.Now };
            _repoMock.Setup(r => r.GetReviewByIdAsync(1)).ReturnsAsync(review);
            _repoMock.Setup(r => r.UpdateReviewAsync(review)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var (ok, msg) = await _service.EditReviewAsync(1, 99, "Updated!");

            Assert.IsTrue(ok);
            Assert.AreEqual("Cập nhật thành công", msg);
            Assert.AreEqual("Updated!", review.ReviewText);
        }
    }
}
