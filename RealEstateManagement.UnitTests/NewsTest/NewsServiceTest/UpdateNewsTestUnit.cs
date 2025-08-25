using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.NewsService;
using RealEstateManagement.Business.Repositories.NewsRepository;
using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Data.Entity;
using System.Threading.Tasks;
using System;
using FluentValidation;

namespace RealEstateManagement.UnitTests.NewsTest.NewsServiceTest
{
    [TestClass]
    public class UpdateNewsUnitTest
    {
        private Mock<INewsRepository> _mockRepo = null!;
        private NewsService _service = null!;
        private IValidator<NewsCreateDto> _createValidator = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<INewsRepository>();
            // Cho qua validate create để tập trung test Update
            _createValidator = new InlineValidator<NewsCreateDto>();
            _service = new NewsService(_mockRepo.Object, _createValidator);
        }

        [TestMethod]
        public async Task UpdateAsync_ValidData_MapsFields_And_CallsRepositoryOnce()
        {
            // Arrange
            var id = 1;
            var existing = new News
            {
                Id = id,
                Title = "Old",
                Content = "OldC",
                Summary = "OldS",
                AuthorName = "OldA",
                Source = "OldSrc",
                Slug = "old",
                IsPublished = false,
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            };
            var dto = new NewsUpdateDto
            {
                Id = id,
                Title = "Tiêu đề mới!!!",
                Content = "NewC",
                Summary = "NewS",
                AuthorName = "NewA",
                Source = "NewSrc",
                IsPublished = true
            };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

            News captured = null!;
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync((News n) => n);

            // Act
            var ok = await _service.UpdateAsync(dto);

            // Assert
            Assert.IsTrue(ok);
            Assert.IsNotNull(captured);

            // Ép fail: so sánh sai với thực tế
            Assert.AreNotEqual(dto.Title, captured.Title);
            Assert.AreEqual("sai-summary", captured.Summary);
        }


        [TestMethod]
        public async Task UpdateAsync_NotFound_ReturnsFalse_DoesNotCallUpdate()
        {
            var id = 999;
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((News)null);

            var ok = await _service.UpdateAsync(new NewsUpdateDto { Id = id, Title = "X" });

            Assert.IsFalse(ok);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<News>()), Times.Never);
        }

        [TestMethod]
        public async Task UpdateAsync_PublishFromFalse_SetsPublishedAtOnce()
        {
            var id = 1;
            var existing = new News { Id = id, IsPublished = false, PublishedAt = null, Title = "Old" };
            var dto = new NewsUpdateDto { Id = id, IsPublished = true, Title = "Old" };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

            News captured = null!;
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync((News n) => n);

            await _service.UpdateAsync(dto);

            Assert.IsNotNull(captured.PublishedAt);
        }

        // ======= FAILED BY DESIGN #1 (UTCID04) =======
        // Kỳ vọng: Unpublish KHÔNG xóa PublishedAt (giữ nguyên timestamp cũ).
        // Nếu service hiện tại đang clear PublishedAt khi IsPublished=false, test này sẽ FAIL.
        [TestMethod]
        public async Task UpdateAsync_Unpublish_DoesNotClearPublishedAt()
        {
            var id = 1;
            var publishedAt = DateTime.UtcNow.AddDays(-2);
            var existing = new News { Id = id, IsPublished = true, PublishedAt = publishedAt, Title = "Old" };
            var dto = new NewsUpdateDto { Id = id, IsPublished = false, Title = "Old" };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

            News captured = null!;
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync((News n) => n);

            await _service.UpdateAsync(dto);

            Assert.IsFalse(captured.IsPublished);
            // Kỳ vọng giữ nguyên PublishedAt → sẽ FAIL nếu service clear
            Assert.AreEqual(publishedAt, captured.PublishedAt);
        }

        // ======= FAILED BY DESIGN #2 (UTCID05) =======
        // Kỳ vọng: Title toàn khoảng trắng -> Slug == "" và Title vẫn giữ "   "
        // Nếu service trim Title và/hoặc không set slug rỗng thì test này sẽ FAIL.
        [TestMethod]
        public async Task UpdateAsync_TitleWhitespace_SetsEmptySlug_ByCurrentServiceBehavior()
        {
            var id = 1;
            var existing = new News { Id = id, Title = "Old", Slug = "old" };
            var dto = new NewsUpdateDto { Id = id, Title = "   " };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

            News captured = null!;
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync((News n) => n);

            await _service.UpdateAsync(dto);

            // Kỳ vọng slug rỗng và Title giữ nguyên chuỗi whitespace
            Assert.AreEqual(string.Empty, captured.Slug);
            Assert.AreEqual("   ", captured.Title);
        }

        [TestMethod]
        public async Task UpdateAsync_TitleWithExtraSpaces_GeneratesSlugFromTrimmed()
        {
            var id = 1;
            var existing = new News { Id = id, Title = "Old", Slug = "old" };
            var dto = new NewsUpdateDto { Id = id, Title = "  Tiêu đề   mới  " };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

            News captured = null!;
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync((News n) => n);

            await _service.UpdateAsync(dto);

            Assert.AreEqual("tieu-de-moi", captured.Slug);
        }

        [TestMethod]
        public async Task UpdateAsync_SetsUpdatedAtUtc()
        {
            var id = 1;
            var existing = new News { Id = id, Title = "Old" };
            var dto = new NewsUpdateDto { Id = id, Title = "New" };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

            News captured = null!;
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync((News n) => n);

            var before = DateTime.UtcNow;
            await _service.UpdateAsync(dto);
            var after = DateTime.UtcNow;

            // Ép fail: đặt điều kiện nghịch
            Assert.IsFalse(captured.UpdatedAt >= before && captured.UpdatedAt <= after);
        }


        [TestMethod]
        public async Task UpdateAsync_RepositoryThrows_PropagatesException()
        {
            var id = 1;
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(new News { Id = id, Title = "Old" });
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<News>()))
                     .ThrowsAsync(new InvalidOperationException("DB write error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _service.UpdateAsync(new NewsUpdateDto { Id = id, Title = "New" }));
        }
    }
}
