using FluentValidation;
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
namespace RealEstateManagement.UnitTests.NewsTest.NewsServiceTest
{
    [TestClass]
    public class GetPublishedNews
    {
        private Mock<INewsRepository> _mockRepo;
        private NewsService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<INewsRepository>();

            IValidator<NewsCreateDto> validator = new InlineValidator<NewsCreateDto>();

            _service = new NewsService(_mockRepo.Object, validator);
        }

        [TestMethod]
        public async Task GetPublishedAsync_WhenCalled_ReturnsOnlyPublishedNewsItems()
        {
            var publishedNewsList = new List<News>
        {
            new News
            {
                Id = 1,
                Title = "Published News 1",
                Content = "Content 1",
                IsPublished = true,
                Slug = "published-news-1",
                PublishedAt = DateTime.UtcNow,
                Images = new List<NewsImage> { new NewsImage { Id = 1, ImageUrl = "url1.jpg", IsPrimary = true } }
            },
            new News
            {
                Id = 3,
                Title = "Published News 2",
                Content = "Content 3",
                IsPublished = true,
                Slug = "published-news-2",
                PublishedAt = DateTime.UtcNow.AddHours(-1),
                Images = new List<NewsImage>()
            }
        };

            _mockRepo.Setup(repo => repo.GetPublishedAsync()).ReturnsAsync(publishedNewsList);

            var result = await _service.GetPublishedAsync();

            Assert.IsNotNull(result);
            var resultList = result.ToList();
            Assert.AreEqual(2, resultList.Count);
            Assert.IsTrue(resultList.All(n => n.IsPublished));
            Assert.AreEqual(publishedNewsList[0].Title, resultList[0].Title);
            Assert.AreEqual(publishedNewsList[1].Slug, resultList[1].Slug);
        }

        [TestMethod]
        public async Task GetPublishedAsync_WhenNoPublishedNewsExists_ReturnsEmptyCollection()
        {
            var publishedNewsList = new List<News>();
            _mockRepo.Setup(repo => repo.GetPublishedAsync()).ReturnsAsync(publishedNewsList);

            var result = await _service.GetPublishedAsync();

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }
        [TestMethod]
        public async Task GetPublishedAsync_WhenRepoOutOfOrder_SortsByPublishedAtDesc()
        {
            var now = DateTime.UtcNow;
            var list = new List<News>
    {
        new News { Id = 1, Title = "Older", IsPublished = true, Slug = "older", PublishedAt = now.AddHours(-1) },
        new News { Id = 2, Title = "Newer", IsPublished = true, Slug = "newer", PublishedAt = now }
    };
            _mockRepo.Setup(r => r.GetPublishedAsync()).ReturnsAsync(list);

            var result = await _service.GetPublishedAsync();
            var arr = result.ToList();

            Assert.IsNotNull(arr);
            Assert.AreEqual(2, arr.Count);

            // Kỳ vọng: sắp xếp giảm dần theo PublishedAt
            // => Nếu service chưa sắp xếp, test này sẽ FAIL như bảng.
            Assert.IsTrue(arr[0].PublishedAt >= arr[1].PublishedAt);
        }

        [TestMethod]
        public async Task GetPublishedAsync_PublishedWithNullImages_MapsToEmptyList()
        {
            var list = new List<News>
    {
        new News { Id = 3, Title = "No Images", IsPublished = true, Slug = "no-images", PublishedAt = DateTime.UtcNow, Images = null }
    };
            _mockRepo.Setup(r => r.GetPublishedAsync()).ReturnsAsync(list);

            var result = await _service.GetPublishedAsync();
            var item = result.Single();

            Assert.IsNotNull(item);
            Assert.IsNotNull(item.Images);
            Assert.AreEqual(0, item.Images.Count);
        }

    }
}