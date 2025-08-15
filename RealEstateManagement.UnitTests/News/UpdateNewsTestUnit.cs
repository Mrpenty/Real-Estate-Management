using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.NewsService;
using RealEstateManagement.Business.Repositories.NewsRepository;
using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Data.Entity;
using System.Threading.Tasks;
using System;
using FluentValidation;

[TestClass]
public class UpdateNewsTestUnit
{
    private Mock<INewsRepository> _mockNewsRepository;
    private NewsService _newsService;

    // nếu NewsService chỉ nhận IValidator<NewsCreateDto> thì dùng cái này là đủ
    private IValidator<NewsCreateDto> _createValidator;

    // nếu NewsService của bạn cũng validate Update, thêm biến này và truyền vào ctor
    private IValidator<NewsUpdateDto> _updateValidator;

    [TestInitialize]
    public void Setup()
    {
        _mockNewsRepository = new Mock<INewsRepository>();

        // Cách đơn giản: validator “rỗng” (pass hết) để tập trung test update logic
        _createValidator = new InlineValidator<NewsCreateDto>();
        _updateValidator = new InlineValidator<NewsUpdateDto>();

        // Tùy chữ ký ctor của NewsService bạn chọn 1 trong 2 dòng dưới:

        // 1) Nếu ctor: NewsService(INewsRepository, IValidator<NewsCreateDto>)
        _newsService = new NewsService(_mockNewsRepository.Object, _createValidator);

        // 2) Nếu ctor: NewsService(INewsRepository, IValidator<NewsCreateDto>, IValidator<NewsUpdateDto>)
        // _newsService = new NewsService(_mockNewsRepository.Object, _createValidator, _updateValidator);
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
    public async Task UpdateAsync_mockRepositorysitoryThrowsException_ThrowsException()
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
    [TestMethod]
public async Task UpdateAsync_NoFieldsChanged_DoesNotRegenerateSlug_And_PreservesPublishedAt()
{
    var newsId = 1;
    var publishedAt = DateTime.UtcNow.AddDays(-2);
    var entity = new News
    {
        Id = newsId,
        Title = "Same Title",
        Content = "Same Content",
        Summary = "Same Summary",
        AuthorName = "Same Author",
        Source = "Same Source",
        Slug = "same-title",
        IsPublished = true,
        PublishedAt = publishedAt
    };
    var dto = new NewsUpdateDto
    {
        Id = newsId,
        Title = "Same Title",
        Content = "Same Content",
        Summary = "Same Summary",
        AuthorName = "Same Author",
        Source = "Same Source",
        IsPublished = true
    };

    _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(entity);
    _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).Returns((Task<News>)Task.CompletedTask);

    var ok = await _newsService.UpdateAsync(dto);

    Assert.IsTrue(ok);
    _mockNewsRepository.Verify(r => r.UpdateAsync(It.Is<News>(n =>
        n.Slug == "same-title" &&
        n.PublishedAt == publishedAt
    )), Times.Once);
}

[TestMethod]
public async Task UpdateAsync_TitleNullOrWhitespace_DoesNotChangeSlug()
{
    var newsId = 1;
    var entity = new News { Id = newsId, Title = "Old Title", Slug = "old-title" };
    var dto = new NewsUpdateDto { Id = newsId, Title = "   " }; // whitespace

    _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(entity);
    _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).Returns((Task<News>)Task.CompletedTask);

    await _newsService.UpdateAsync(dto);

    _mockNewsRepository.Verify(r => r.UpdateAsync(It.Is<News>(n => n.Slug == "old-title" && n.Title == "   ")), Times.Once);
}

[TestMethod]
public async Task UpdateAsync_TitleTrimmed_GeneratesSlugFromTrimmed()
{
    var newsId = 1;
    var entity = new News { Id = newsId, Title = "Old", Slug = "old" };
    var dto = new NewsUpdateDto { Id = newsId, Title = "  Tiêu đề   mới  " };

    _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(entity);
    _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).Returns((Task<News>)Task.CompletedTask);

    await _newsService.UpdateAsync(dto);

    _mockNewsRepository.Verify(r => r.UpdateAsync(It.Is<News>(n => n.Slug == "tieu-de-moi")), Times.Once);
}

[TestMethod]
public async Task UpdateAsync_Republish_WhenPublishedAtAlreadySet_PreservesOriginalPublishedAt()
{
    var newsId = 1;
    var original = DateTime.UtcNow.AddDays(-3);
    var entity = new News { Id = newsId, IsPublished = true, PublishedAt = original };
    var dto = new NewsUpdateDto { Id = newsId, IsPublished = true }; // vẫn publish

    _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(entity);
    _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).Returns((Task<News>)Task.CompletedTask);

    await _newsService.UpdateAsync(dto);

    _mockNewsRepository.Verify(r => r.UpdateAsync(It.Is<News>(n => n.PublishedAt == original)), Times.Once);
}

[TestMethod]
public async Task UpdateAsync_Unpublish_Then_KeepPublishedAt_NotCleared()
{
    // Tuỳ spec: nhiều hệ thống KHÔNG xoá PublishedAt khi unpublish
    var newsId = 1;
    var publishedAt = DateTime.UtcNow.AddDays(-1);
    var entity = new News { Id = newsId, IsPublished = true, PublishedAt = publishedAt };
    var dto = new NewsUpdateDto { Id = newsId, IsPublished = false };

    _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(entity);
    _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).Returns((Task<News>)Task.CompletedTask);

    await _newsService.UpdateAsync(dto);

    _mockNewsRepository.Verify(r => r.UpdateAsync(It.Is<News>(n => n.PublishedAt == publishedAt && n.IsPublished == false)), Times.Once);
}

[TestMethod]
public async Task UpdateAsync_MapsAllEditableFields()
{
    var newsId = 7;
    var entity = new News
    {
        Id = newsId,
        Title = "Old",
        Content = "OldC",
        Summary = "OldS",
        AuthorName = "OldA",
        Source = "OldSrc",
        IsPublished = false
    };
    var dto = new NewsUpdateDto
    {
        Id = newsId,
        Title = "New",
        Content = "NewC",
        Summary = "NewS",
        AuthorName = "NewA",
        Source = "NewSrc",
        IsPublished = true
    };

    _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(entity);
    _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).Returns((Task<News>)Task.CompletedTask);

    await _newsService.UpdateAsync(dto);

    _mockNewsRepository.Verify(r => r.UpdateAsync(It.Is<News>(n =>
        n.Title == "New" &&
        n.Content == "NewC" &&
        n.Summary == "NewS" &&
        n.AuthorName == "NewA" &&
        n.Source == "NewSrc" &&
        n.IsPublished == true
    )), Times.Once);
}

[TestMethod]
public async Task UpdateAsync_SetsUpdatedAtUtc()
{
    var newsId = 1;
    var entity = new News { Id = newsId, Title = "Old" };
    var dto = new NewsUpdateDto { Id = newsId, Title = "New" };

    _mockNewsRepository.Setup(r => r.GetByIdAsync(newsId)).ReturnsAsync(entity);
    News captured = null!;
    _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>()))
        .Callback<News>(n => captured = n)
        .Returns((Task<News>)Task.CompletedTask);

    var before = DateTime.UtcNow;
    await _newsService.UpdateAsync(dto);
    var after = DateTime.UtcNow;

    Assert.IsNotNull(captured);
    Assert.IsTrue(captured.UpdatedAt >= before && captured.UpdatedAt <= after, "UpdatedAt phải trong khoảng thực thi.");
}

[TestMethod]
public async Task UpdateAsync_RepositoryGetByIdThrows_PropagatesException()
{
    _mockNewsRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
        .ThrowsAsync(new InvalidOperationException("DB read error"));

    await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
        _newsService.UpdateAsync(new NewsUpdateDto { Id = 123, Title = "X" }));
}

[TestMethod]
public async Task UpdateAsync_RepositoryUpdateThrows_PropagatesException_And_GetByIdCalledOnce()
{
    var id = 1;
    _mockNewsRepository.Setup(r => r.GetByIdAsync(id))
        .ReturnsAsync(new News { Id = id, Title = "Old" });
    _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>()))
        .ThrowsAsync(new InvalidOperationException("DB write error"));

    await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
        _newsService.UpdateAsync(new NewsUpdateDto { Id = id, Title = "New" }));

    _mockNewsRepository.Verify(r => r.GetByIdAsync(id), Times.Once);
    _mockNewsRepository.Verify(r => r.UpdateAsync(It.IsAny<News>()), Times.Once);
}

[TestMethod]
public async Task UpdateAsync_WhenNotFound_DoesNotCallUpdate_AndReturnsFalse()
{
    var id = 999;
    _mockNewsRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((News)null);

    var ok = await _newsService.UpdateAsync(new NewsUpdateDto { Id = id, Title = "New" });

    Assert.IsFalse(ok);
    _mockNewsRepository.Verify(r => r.UpdateAsync(It.IsAny<News>()), Times.Never);
}

//
// (Tuỳ bạn có validator cho Update hay không)
// Nếu NewsService nhận IValidator<NewsUpdateDto>, bạn có thể thêm 2 test dưới:
//  - Sửa Setup để dùng Mock<IValidator<NewsUpdateDto>> và truyền vào ctor.
//  - Ở đây minh hoạ cách làm:
#if false
[TestMethod]
public async Task UpdateAsync_InvalidDto_ThrowsValidationException_AndDoesNotCallUpdate()
{
    var id = 1;
    var entity = new News { Id = id, Title = "Old" };
    var updateValidator = new Mock<IValidator<NewsUpdateDto>>();
    updateValidator.Setup(v => v.Validate(It.IsAny<NewsUpdateDto>()))
        .Returns(new ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Title", "Title required") }));

    _newsService = new NewsService(_mockNewsRepository.Object, _createValidator, updateValidator.Object);

    _mockNewsRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

    await Assert.ThrowsExceptionAsync<FluentValidation.ValidationException>(() =>
        _newsService.UpdateAsync(new NewsUpdateDto { Id = id, Title = "" }));

    _mockNewsRepository.Verify(r => r.UpdateAsync(It.IsAny<News>()), Times.Never);
}

[TestMethod]
public async Task UpdateAsync_ValidatorCalledOnce()
{
    var id = 2;
    var entity = new News { Id = id, Title = "Old" };
    var updateValidator = new Mock<IValidator<NewsUpdateDto>>();
    updateValidator.Setup(v => v.Validate(It.IsAny<NewsUpdateDto>()))
        .Returns(new ValidationResult()); // pass

    _newsService = new NewsService(_mockNewsRepository.Object, _createValidator, updateValidator.Object);

    _mockNewsRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
    _mockNewsRepository.Setup(r => r.UpdateAsync(It.IsAny<News>())).Returns((Task<News>)Task.CompletedTask);

    await _newsService.UpdateAsync(new NewsUpdateDto { Id = id, Title = "New" });

    updateValidator.Verify(v => v.Validate(It.IsAny<NewsUpdateDto>()), Times.Once);
}
#endif

}