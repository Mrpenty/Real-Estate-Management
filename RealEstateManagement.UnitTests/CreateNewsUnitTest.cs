using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.NewsService;
using RealEstateManagement.Business.Repositories.NewsRepository;
using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Data.Entity;
using System.Threading.Tasks;
using System;

[TestClass]
public class CreateNewsUnitTest
{
    private Mock<INewsRepository> _mockNewsRepository;
    private NewsService _newsService;

    [TestInitialize]
    public void Setup()
    {
        _mockNewsRepository = new Mock<INewsRepository>();
        _newsService = new NewsService(_mockNewsRepository.Object);
    }

    [TestMethod]
    public async Task CreateAsync_ValidNewsDto_ReturnsNewId()
    {
        var newsCreateDto = new NewsCreateDto
        {
            Title = "A new news title",
            Content = "The content of the new news.",
            Summary = "A summary of the news.",
            AuthorName = "Test Author",
            Source = "Test Source"
        };
        var newsEntity = new News { Id = 1, Title = newsCreateDto.Title };
        _mockNewsRepository.Setup(r => r.AddAsync(It.IsAny<News>())).ReturnsAsync(newsEntity);

        var result = await _newsService.CreateAsync(newsCreateDto);

        Assert.AreEqual(1, result);
        _mockNewsRepository.Verify(r => r.AddAsync(It.Is<News>(n => n.Title == newsCreateDto.Title && n.IsPublished == false)), Times.Once);
    }

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
    public async Task CreateAsync_RepositoryThrowsException_ThrowsException()
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
}