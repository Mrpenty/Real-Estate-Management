using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.Reviews;
using RealEstateManagement.Business.Repositories.Reviews;
using RealEstateManagement.Data.Entity.Reviews;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity;
using System.Threading.Tasks;
using System;

namespace RealEstateManagement.UnitTests.Reviews.ReviewServiceTest
{
    [TestClass]
    public class AddReviewAsyncTests
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
        public async Task Fail_When_Contract_NotCompleted()
        {
            _repoMock.Setup(r => r.GetCompletedContractAsync(1, 99)).ReturnsAsync((RentalContract)null);

            var (ok, msg) = await _service.AddReviewAsync(1, 99, "Good", 5);

            Assert.IsFalse(ok);
            Assert.AreEqual("Bạn không đủ điều kiện review: hợp đồng chưa hoàn thành!", msg);
        }

        [TestMethod]
        public async Task Fail_When_AlreadyReviewed()
        {
            var contract = new RentalContract
            {
                Id = 10,
                PropertyPost = new PropertyPost { PropertyId = 55 },
                Status = RentalContract.ContractStatus.Confirmed
            };
            _repoMock.Setup(r => r.GetCompletedContractAsync(1, 99)).ReturnsAsync(contract);
            _repoMock.Setup(r => r.GetReviewByContractAsync(10, 99)).ReturnsAsync(new Review { Id = 5 });

            var (ok, msg) = await _service.AddReviewAsync(1, 99, "Good", 5);

            Assert.IsFalse(ok);
            Assert.AreEqual("Bạn đã đánh giá hợp đồng này rồi.", msg);
        }

        [TestMethod]
        public async Task Success_CreatesReview()
        {
            var contract = new RentalContract
            {
                Id = 10,
                PropertyPost = new PropertyPost { PropertyId = 55 },
                Status = RentalContract.ContractStatus.Confirmed
            };
            _repoMock.Setup(r => r.GetCompletedContractAsync(1, 99)).ReturnsAsync(contract);
            _repoMock.Setup(r => r.GetReviewByContractAsync(10, 99)).ReturnsAsync((Review)null);

            Review savedReview = null;
            _repoMock.Setup(r => r.AddReviewAsync(It.IsAny<Review>()))
                     .Callback<Review>(rev => savedReview = rev)
                     .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var (ok, msg) = await _service.AddReviewAsync(1, 99, "Awesome!", 5);

            Assert.IsTrue(ok);
            Assert.AreEqual("Thành công", msg);
            Assert.IsNotNull(savedReview);
            Assert.AreEqual(55, savedReview.PropertyId);
            Assert.AreEqual(99, savedReview.RenterId);
            Assert.AreEqual("Awesome!", savedReview.ReviewText);
        }
    }
}
