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
namespace RealEstateManagement.UnitTests.NewsTest.NewsServiceTest
{
    [TestClass]
    public class CreateNewsUnitTest
    {
        private Mock<INewsRepository> _mockNewsRepository;
        private NewsService _newsService;
        private Mock<IValidator<NewsCreateDto>> _mockValidator;
        [TestInitialize]
        public void Setup()
        {
            _mockNewsRepository = new Mock<INewsRepository>();

            _mockValidator = new Mock<IValidator<NewsCreateDto>>();
            // Cho hợp lệ mặc định
            _mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<NewsCreateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockValidator
                .Setup(v => v.Validate(It.IsAny<NewsCreateDto>()))
                .Returns(new ValidationResult());

            _newsService = new NewsService(_mockNewsRepository.Object, _mockValidator.Object);
        }



        [TestMethod]
        public async Task CreateAsync_mockRepositorysitoryThrowsException_ThrowsException()
        {
            var newsCreateDto = new NewsCreateDto
            {
                Title = "Title",
                Content = "Content",
                Summary = "Summary",
                AuthorName = "A",
                Source = "Source"
            };
            _mockNewsRepository.Setup(r => r.AddAsync(It.IsAny<News>())).ThrowsAsync(new InvalidOperationException("Database error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _newsService.CreateAsync(newsCreateDto));
        }
        [TestMethod]
        public async Task CreateAsync_SetsIsPublishedFalseAndCreatedAtUtc()
        {
            var dto = new NewsCreateDto { Title = "Title", Content = "C", Summary = "S", AuthorName = "A", Source = "Src" };
            News captured = null!;
            _mockNewsRepository.Setup(r => r.AddAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync(new News { Id = 13 });

            var before = DateTime.UtcNow;
            await _newsService.CreateAsync(dto);
            var after = DateTime.UtcNow;

            Assert.IsNotNull(captured);
            Assert.IsFalse(captured.IsPublished);
            Assert.IsTrue(captured.CreatedAt >= before && captured.CreatedAt <= after, "CreatedAt phải nằm trong khoảng thời gian thực thi.");
        }

        [TestMethod]
        public async Task CreateAsync_MapsAllFields_FromDtoToEntity()
        {
            var dto = new NewsCreateDto
            {
                Title = "Bản tin",
                Content = "Nội dung",
                Summary = "Tóm tắt",
                AuthorName = "Tác giả",
                Source = "Src"
            };
            News captured = null!;
            _mockNewsRepository.Setup(r => r.AddAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync(new News { Id = 14 });

            await _newsService.CreateAsync(dto);

            Assert.IsNotNull(captured);
            Assert.AreEqual(dto.Title, captured.Title);
            Assert.AreEqual(dto.Content, captured.Content);
            Assert.AreEqual(dto.Summary, captured.Summary);
            Assert.AreEqual(dto.AuthorName, captured.AuthorName);
            Assert.AreEqual(dto.Source, captured.Source);
        }

        [TestMethod]
        public async Task CreateAsync_CallsRepositoryOnce()
        {
            var dto = new NewsCreateDto { Title = "Title", Content = "C", Summary = "S", AuthorName = "A", Source = "Src" };
            _mockNewsRepository.Setup(r => r.AddAsync(It.IsAny<News>()))
                .ReturnsAsync(new News { Id = 15 });

            await _newsService.CreateAsync(dto);

            _mockNewsRepository.Verify(r => r.AddAsync(It.IsAny<News>()), Times.Once);
        }
    }
}