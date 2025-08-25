using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.NewsService;
using RealEstateManagement.Business.Repositories.NewsRepository;
using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Data.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
namespace RealEstateManagement.UnitTests.NewsTest.NewsServiceTest
{
    [TestClass]
    public class GetNewByIdAsyncTests
    {
        private Mock<INewsRepository> _mockNewsRepository;
        private NewsService _newsService;
        private IValidator<NewsCreateDto> _validator;
        [TestInitialize]
        public void Setup()
        {
            _mockNewsRepository = new Mock<INewsRepository>();
            _newsService = new NewsService(_mockNewsRepository.Object, _validator);
            _validator = new InlineValidator<NewsCreateDto>();
        }

        [TestMethod]
        public async Task GetByIdAsync_NewsExists_ReturnsNewsViewDto()
        {
            var newsId = 1;
            var newsEntity = new News
            {
                Id = newsId,
                Title = "Test Title",
                Slug = "test-title",
                Content = "Test Content",
                Summary = "Test Summary",
                AuthorName = "Test Author",
                Source = "Test Source",
                IsPublished = true,
                PublishedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<NewsImage> { new NewsImage { Id = 1, ImageUrl = "url1", IsPrimary = true, Order = 1 } }
            };

            _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(newsEntity);

            var result = await _newsService.GetByIdAsync(newsId);

            Assert.IsNotNull(result);
            Assert.AreEqual(newsEntity.Id, result.Id);
            Assert.AreEqual(newsEntity.Title, result.Title);
            Assert.AreEqual(newsEntity.Slug, result.Slug);
            Assert.AreEqual(newsEntity.Content, result.Content);
            Assert.AreEqual(newsEntity.Summary, result.Summary);
            Assert.AreEqual(newsEntity.Images.Count, result.Images.Count);
            _mockNewsRepository.Verify(r => r.GetByIdAsync(newsId), Times.Once);
        }

        [TestMethod]
        public async Task GetByIdAsync_NewsNotFound_ReturnsNull()
        {
            var newsId = 999;
            _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync((News)null);

            var result = await _newsService.GetByIdAsync(newsId);

            Assert.IsNull(result);
            _mockNewsRepository.Verify(r => r.GetByIdAsync(newsId), Times.Once);
        }

        [TestMethod]
        public async Task GetByIdAsync_NewsHasNoImages_ReturnsNewsViewDtoWithEmptyImagesList()
        {
            var newsId = 2;
            var newsEntity = new News
            {
                Id = newsId,
                Title = "News with no images",
                Images = null
            };

            _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(newsEntity);

            var result = await _newsService.GetByIdAsync(newsId);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Images);
            Assert.AreEqual(0, result.Images.Count);
            _mockNewsRepository.Verify(r => r.GetByIdAsync(newsId), Times.Once);
        }
    }
}