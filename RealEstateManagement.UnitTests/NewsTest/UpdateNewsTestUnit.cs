using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.NewsService;
using RealEstateManagement.Business.Repositories.NewsRepository;
using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Data.Entity;
using System.Threading.Tasks;
using System;
using FluentValidation;
using FluentValidation.Results;
namespace RealEstateManagement.UnitTests.NewsTest
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
            // Nếu UpdateAsync trả về Task<News>: dùng .ReturnsAsync((News n) => n);

            // Act
            var ok = await _service.UpdateAsync(dto);

            // Assert
            Assert.IsTrue(ok);
            Assert.IsNotNull(captured);
            Assert.AreEqual(dto.Title, captured.Title);
            Assert.AreEqual(dto.Content, captured.Content);
            Assert.AreEqual(dto.Summary, captured.Summary);
            Assert.AreEqual(dto.AuthorName, captured.AuthorName);
            Assert.AreEqual(dto.Source, captured.Source);
            Assert.AreEqual(true, captured.IsPublished);
            Assert.AreEqual("tieu-de-moi", captured.Slug); // slug từ tiêu đề mới
            Assert.AreEqual(existing.CreatedAt, captured.CreatedAt); // không đổi CreatedAt
            Assert.IsNotNull(captured.PublishedAt);                  // vừa publish thì set
            Assert.IsTrue(captured.UpdatedAt <= DateTime.UtcNow);
            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<News>()), Times.Once);
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

        [TestMethod]
        public async Task UpdateAsync_Unpublish_DoesNotClearPublishedAt()
        {
            // Theo logic hiện tại: unpublish KHÔNG xoá PublishedAt
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
            Assert.AreEqual(publishedAt, captured.PublishedAt);
        }

        [TestMethod]
        public async Task UpdateAsync_TitleWhitespace_SetsEmptySlug_ByCurrentServiceBehavior()
        {
            // Với NewsService hiện tại: luôn GenerateSlug(dto.Title) => whitespace => slug = ""
            var id = 1;
            var existing = new News { Id = id, Title = "Old", Slug = "old" };
            var dto = new NewsUpdateDto { Id = id, Title = "   " };

            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

            News captured = null!;
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync((News n) => n);

            await _service.UpdateAsync(dto);

            Assert.AreEqual(string.Empty, captured.Slug);
            Assert.AreEqual("   ", captured.Title); // service gán thẳng Title = dto.Title
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

            Assert.IsTrue(captured.UpdatedAt >= before && captured.UpdatedAt <= after);
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