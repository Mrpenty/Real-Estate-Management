using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System;
using FluentValidation;
using Moq;
using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Business.Repositories.NewsRepository;
using RealEstateManagement.Business.Services.NewsService;
using RealEstateManagement.Data.Entity;
using FluentValidation.Results;
namespace RealEstateManagement.UnitTests.NewsTest.NewsServiceTest
{
    [TestClass]
    public class GenerateSlugViaServiceTests
    {
        private Mock<INewsRepository> _mockRepo = null!;
        private Mock<IValidator<NewsCreateDto>> _mockValidator = null!;
        private NewsService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<INewsRepository>();
            _mockValidator = new Mock<IValidator<NewsCreateDto>>();

            // Cho qua validate mặc định
            _mockValidator
                .Setup(v => v.ValidateAsync(It.IsAny<NewsCreateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            _mockValidator
                .Setup(v => v.Validate(It.IsAny<NewsCreateDto>()))
                .Returns(new ValidationResult());

            _service = new NewsService(_mockRepo.Object, _mockValidator.Object);
        }

        private async Task<News> CreateAndCaptureAsync(string? title)
        {
            News captured = null!;
            _mockRepo
                .Setup(r => r.AddAsync(It.IsAny<News>()))
                .Callback<News>(n => captured = n)
                .ReturnsAsync((News n) => { n.Id = n.Id == 0 ? 1 : n.Id; return n; });

            var dto = new NewsCreateDto
            {
                Title = title,
                Content = "C",
                Summary = "S",
                AuthorName = "A",
                Source = "Src"
            };

            await _service.CreateAsync(dto);
            Assert.IsNotNull(captured, "Repository chưa nhận entity.");
            return captured;
        }

        [TestMethod]
        public async Task GenerateSlug_WithVietnameseTitle_ReturnsCorrectSlug()
        {
            var news = await CreateAndCaptureAsync("Bất động sản Việt Nam và những thách thức");
            Assert.AreEqual("bat-dong-san-viet-nam-va-nhung-thach-thuc", news.Slug);
        }

        [TestMethod]
        public async Task GenerateSlug_WithSpecialCharactersAndSpaces_ReturnsCorrectSlug()
        {
            var news = await CreateAndCaptureAsync("   BĐS: Giá nhà tăng 10% !? ");
            Assert.AreEqual("bds-gia-nha-tang-10", news.Slug);
        }

        [TestMethod]
        public async Task GenerateSlug_WithMultipleHyphens_ReturnsCorrectSlug()
        {
            var news = await CreateAndCaptureAsync("đầu---tư---bds");
            Assert.AreEqual("dau-tu-bds", news.Slug);
        }

        [TestMethod]
        public async Task GenerateSlug_WithLeadingAndTrailingHyphens_ReturnsCorrectSlug()
        {
            var news = await CreateAndCaptureAsync("-bán nhà--giá tốt-");
            Assert.AreEqual("ban-nha-gia-tot", news.Slug);
        }

        [TestMethod]
        public async Task GenerateSlug_WithEmptyString_ReturnsEmptyString()
        {
            var news = await CreateAndCaptureAsync("");
            Assert.AreEqual(string.Empty, news.Slug);
        }

        [TestMethod]
        public async Task GenerateSlug_WithNullInput_ReturnsEmptyString()
        {
            var news = await CreateAndCaptureAsync(null);
            Assert.AreEqual(string.Empty, news.Slug);
        }
    }
}