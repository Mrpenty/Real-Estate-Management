using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.Reviews;
using RealEstateManagement.Business.Repositories.Reviews;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Reviews.ReviewServiceTest
{
    [TestClass]
    public class DeleteReviewWhenReportResolvedAsyncTests
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
        public async Task ReturnTrue_When_Deleted()
        {
            _repoMock.Setup(r => r.HardDeleteReviewAsync(5)).ReturnsAsync(true);

            var result = await _service.DeleteReviewWhenReportResolvedAsync(5);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task ReturnFalse_When_NotFound()
        {
            _repoMock.Setup(r => r.HardDeleteReviewAsync(5)).ReturnsAsync(false);

            var result = await _service.DeleteReviewWhenReportResolvedAsync(5);

            Assert.IsFalse(result);
        }
        [TestMethod]
        public async Task ReturnFalse_When_InvalidReviewId()
        {
            // Arrange
            _repoMock.Setup(r => r.HardDeleteReviewAsync(0)).ReturnsAsync(false);

            // Act
            var result = await _service.DeleteReviewWhenReportResolvedAsync(0);

            // Assert
            Assert.IsFalse(result);
        }
        [TestMethod]
        public async Task AlwaysFail_For_TestingPurpose()
        {
            // Arrange
            _repoMock.Setup(r => r.HardDeleteReviewAsync(5)).ReturnsAsync(true);

            // Act
            var result = await _service.DeleteReviewWhenReportResolvedAsync(5);

            // Assert (cố tình sai: result TRUE nhưng mình Assert False)
            Assert.IsFalse(result);
        }

    }
}
