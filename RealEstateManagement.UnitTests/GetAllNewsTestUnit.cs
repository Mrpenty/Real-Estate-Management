using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Business.Repositories.NewsRepository;
using RealEstateManagement.Business.Services.NewsService;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestClass]
public class GetAllNewsTestUnit
{
    private Mock<INewsRepository> _mockRepo;
    private NewsService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<INewsRepository>();
        _service = new NewsService(_mockRepo.Object);
    }

    [TestMethod]
    public async Task GetAllAsync_WhenCalled_ReturnsAllNewsItems()
    {
        var newsList = new List<News>
        {
            new News
            {
                Id = 1,
                Title = "News 1",
                Content = "Content 1",
                Summary = "Summary 1",
                AuthorName = "Author 1",
                Source = "Source 1",
                CreatedAt = DateTime.UtcNow,
                IsPublished = true,
                Slug = "news-1",
                Images = new List<NewsImage> { new NewsImage { Id = 1, ImageUrl = "url1.jpg", IsPrimary = true } }
            },
            new News
            {
                Id = 2,
                Title = "News 2",
                Content = "Content 2",
                Summary = "Summary 2",
                AuthorName = "Author 2",
                Source = "Source 2",
                CreatedAt = DateTime.UtcNow,
                IsPublished = false,
                Slug = "news-2",
                Images = new List<NewsImage>()
            }
        };

        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(newsList);

        var result = await _service.GetAllAsync();

        Assert.IsNotNull(result);
        var resultList = result.ToList();
        Assert.AreEqual(2, resultList.Count);
        Assert.AreEqual(newsList[0].Title, resultList[0].Title);
        Assert.AreEqual(newsList[1].AuthorName, resultList[1].AuthorName);
        Assert.IsNotNull(resultList[0].Images);
        Assert.AreEqual(1, resultList[0].Images.Count);
    }

    [TestMethod]
    public async Task GetAllAsync_WhenNoNewsExists_ReturnsEmptyCollection()
    {
        var newsList = new List<News>();
        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(newsList);

        var result = await _service.GetAllAsync();

        Assert.IsNotNull(result);
        Assert.IsFalse(result.Any());
    }
}
