using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.NewsService;
using RealEstateManagement.Business.Repositories.NewsRepository;
using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Data.Entity;
using System.Threading.Tasks;
using System;

[TestClass]
public class UpdateNewsTestUnit
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
    public async Task UpdateAsync_ExistingNewsWithValidData_ReturnsTrue()
    {
        var newsId = 1;
        var newsEntity = new News { Id = newsId, Title = "Old Title", IsPublished = false };
        var updateDto = new NewsUpdateDto
        {
            Id = newsId,
            Title = "New Title",
            Content = "New Content",
            Summary = "New Summary",
            AuthorName = "New Author",
            Source = "New Source",
            IsPublished = true
        };

        _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(newsEntity);
        _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).Returns((Task<News>)Task.CompletedTask);

        var result = await _newsService.UpdateAsync(updateDto);

        Assert.IsTrue(result);
        _mockNewsRepository.Verify(r => r.UpdateAsync(It.Is<News>(n => n.Id == newsId && n.Title == "New Title" && n.IsPublished == true && n.PublishedAt != null)), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_NewsNotFound_ReturnsFalse()
    {
        var newsId = 999;
        var updateDto = new NewsUpdateDto { Id = newsId, Title = "New Title" };

        _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync((News)null);

        var result = await _newsService.UpdateAsync(updateDto);

        Assert.IsFalse(result);
        _mockNewsRepository.Verify(r => r.UpdateAsync(It.IsAny<News>()), Times.Never);
    }

    [TestMethod]
    public async Task UpdateAsync_RepositoryThrowsException_ThrowsException()
    {
        var newsId = 1;
        var newsEntity = new News { Id = newsId, Title = "Old Title" };
        var updateDto = new NewsUpdateDto { Id = newsId, Title = "New Title" };

        _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(newsEntity);
        _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).ThrowsAsync(new InvalidOperationException("Database error"));

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _newsService.UpdateAsync(updateDto));
    }

    [TestMethod]
    public async Task UpdateAsync_ChangingPublishedStatus_UpdatesPublishedAt()
    {
        var newsId = 1;
        var newsEntity = new News { Id = newsId, Title = "Old Title", IsPublished = false, PublishedAt = null };
        var updateDto = new NewsUpdateDto { Id = newsId, IsPublished = true };

        _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(newsEntity);
        _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).Returns((Task<News>)Task.CompletedTask);

        await _newsService.UpdateAsync(updateDto);

        _mockNewsRepository.Verify(r => r.UpdateAsync(It.Is<News>(n => n.PublishedAt != null)), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_RevertingPublishedStatus_PublishedAtIsPreserved()
    {
        var newsId = 1;
        var oldPublishedAt = DateTime.UtcNow.AddDays(-1);
        var newsEntity = new News { Id = newsId, Title = "Old Title", IsPublished = true, PublishedAt = oldPublishedAt };
        var updateDto = new NewsUpdateDto { Id = newsId, IsPublished = false };

        _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(newsEntity);
        _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).Returns((Task<News>)Task.CompletedTask);

        await _newsService.UpdateAsync(updateDto);

        _mockNewsRepository.Verify(r => r.UpdateAsync(It.Is<News>(n => n.PublishedAt == oldPublishedAt)), Times.Once);
    }

    [TestMethod]
    public async Task UpdateAsync_SlugIsGeneratedCorrectly()
    {
        var newsId = 1;
        var newsEntity = new News { Id = newsId, Title = "Old Title", Slug = "old-title" };
        var updateDto = new NewsUpdateDto { Id = newsId, Title = "Tiêu đề mới" };

        _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(newsEntity);
        _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).Returns((Task<News>)Task.CompletedTask);

        await _newsService.UpdateAsync(updateDto);

        _mockNewsRepository.Verify(r => r.UpdateAsync(It.Is<News>(n => n.Slug == "tieu-de-moi")), Times.Once);
    }
}