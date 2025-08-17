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

        //[TestMethod]
        //public async Task CreateAsync_ValidNewsDto_ReturnsNewId()
        //{
        //    var newsCreateDto = new NewsCreateDto
        //    {
        //        Title = "A new news title",
        //        Content = "The content of the new news.",
        //        Summary = "A summary of the news.",
        //        AuthorName = "Test Author",
        //        Source = "Test Source"
        //    };
        //    var newsEntity = new News { Id = 1, Title = newsCreateDto.Title };
        //    _mockNewsRepository.Setup(r => r.AddAsync(It.IsAny<News>())).ReturnsAsync(newsEntity);

        //    var result = await _newsService.CreateAsync(newsCreateDto);

        //    Assert.AreEqual(1, result);
        //    _mockNewsRepository.Verify(r => r.AddAsync(It.Is<News>(n => n.Title == newsCreateDto.Title && n.IsPublished == false)), Times.Once);
        //}

        [TestMethod]
        public async Task CreateAsync_TitleWithSpecialCharacters_GeneratesCorrectSlug()
        {
            var newsCreateDto = new NewsCreateDto
            {
                Title = "Tiêu đề tin tức đặc biệt @!#$%",
                Content = "Content",
                Summary = "Summary",
                AuthorName = "Author",
                Source = "Source"
            };
            var newsEntity = new News { Id = 2, Title = newsCreateDto.Title };
            _mockNewsRepository.Setup(r => r.AddAsync(It.IsAny<News>())).ReturnsAsync(newsEntity);

            var result = await _newsService.CreateAsync(newsCreateDto);

            _mockNewsRepository.Verify(r => r.AddAsync(It.Is<News>(n => n.Slug == "tieu-de-tin-tuc-dac-biet")), Times.Once);
        }

        [TestMethod]
        public async Task CreateAsync_mockRepositorysitoryThrowsException_ThrowsException()
        {
            var newsCreateDto = new NewsCreateDto
            {
                Title = "Title",
                Content = "Content",
                Summary = "Summary",
                AuthorName = "Author",
                Source = "Source"
            };
            _mockNewsRepository.Setup(r => r.AddAsync(It.IsAny<News>())).ThrowsAsync(new InvalidOperationException("Database error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _newsService.CreateAsync(newsCreateDto));
        }
        [TestMethod]
        public async Task CreateAsync_EmptyTitle_GeneratesEmptySlug()
        {
            // Arrange
            var dto = new NewsCreateDto { Title = "", Content = "C", Summary = "S", AuthorName = "A", Source = "Src" };
            News captured = null!;
            _mockNewsRepository.Setup(r => r.AddAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync(new News { Id = 10 });

            // Act
            var id = await _newsService.CreateAsync(dto);

            // Assert
            Assert.AreEqual(10, id);
            Assert.IsNotNull(captured);
            Assert.AreEqual(string.Empty, captured.Slug);
        }

        [TestMethod]
        public async Task CreateAsync_VietnameseDiacritics_ConvertsToAsciiSlug()
        {
            var dto = new NewsCreateDto { Title = "Đường đến Hà Nội", Content = "C", Summary = "S", AuthorName = "A", Source = "Src" };
            News captured = null!;
            _mockNewsRepository.Setup(r => r.AddAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync(new News { Id = 11 });

            await _newsService.CreateAsync(dto);

            Assert.IsNotNull(captured);
            Assert.AreEqual("duong-den-ha-noi", captured.Slug);
        }

        [TestMethod]
        public async Task CreateAsync_MultipleSpacesAndPunctuation_NormalizesHyphens()
        {
            var dto = new NewsCreateDto { Title = "Hello   ---   World!!!", Content = "C", Summary = "S", AuthorName = "A", Source = "Src" };
            News captured = null!;
            _mockNewsRepository.Setup(r => r.AddAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync(new News { Id = 12 });

            await _newsService.CreateAsync(dto);

            Assert.IsNotNull(captured);
            Assert.AreEqual("hello-world", captured.Slug);
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
                Source = "Nguồn"
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