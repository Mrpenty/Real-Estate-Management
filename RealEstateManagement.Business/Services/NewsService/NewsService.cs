using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Business.Repositories.NewsRepository;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using FluentValidation;

namespace RealEstateManagement.Business.Services.NewsService
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _repo;
        private readonly IValidator<NewsCreateDto> _validator;

        public NewsService(INewsRepository repo, IValidator<NewsCreateDto> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        public async Task<int> CreateAsync(NewsCreateDto dto)
        {
            var vr = await _validator.ValidateAsync(dto);
            if (!vr.IsValid) throw new ValidationException(vr.Errors);
            var news = new News
            {
                Title = dto.Title,
                Content = dto.Content,
                Summary = dto.Summary,
                AuthorName = dto.AuthorName,
                Source = dto.Source,
                CreatedAt = DateTime.UtcNow,
                IsPublished = false,
                Slug = GenerateSlug(dto.Title),
            };
            var created = await _repo.AddAsync(news);
            return created.Id;
        }

        public async Task<NewsViewDto?> GetByIdAsync(int id)
        {
            var news = await _repo.GetByIdAsync(id);
            if (news == null) return null;

            return new NewsViewDto
            {
                Id = news.Id,
                Title = news.Title,
                Slug = news.Slug,
                Content = news.Content,
                Summary = news.Summary,
                AuthorName = news.AuthorName,
                Source = news.Source,
                PublishedAt = news.PublishedAt,
                IsPublished = news.IsPublished,
                CreatedAt = news.CreatedAt,
                UpdatedAt = news.UpdatedAt,
                Images = news.Images?.Select(img => new NewsImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    IsPrimary = img.IsPrimary,
                    Order = img.Order
                }).ToList() ?? new List<NewsImageDto>()
            };
        }

        public async Task<NewsViewDto?> GetBySlugAsync(string slug)
        {
            var news = await _repo.GetBySlugAsync(slug);
            if (news == null) return null;

            return new NewsViewDto
            {
                Id = news.Id,
                Title = news.Title,
                Slug = news.Slug,
                Content = news.Content,
                Summary = news.Summary,
                AuthorName = news.AuthorName,
                Source = news.Source,
                PublishedAt = news.PublishedAt,
                IsPublished = news.IsPublished,
                CreatedAt = news.CreatedAt,
                UpdatedAt = news.UpdatedAt,
                Images = news.Images?.Select(img => new NewsImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    IsPrimary = img.IsPrimary,
                    Order = img.Order
                }).ToList() ?? new List<NewsImageDto>()
            };
        }

        public async Task<IEnumerable<NewsViewDto>> GetAllAsync()
        {
            var all = await _repo.GetAllAsync();
            return all.Select(news => new NewsViewDto
            {
                Id = news.Id,
                Title = news.Title,
                Slug = news.Slug,
                Content = news.Content,
                Summary = news.Summary,
                AuthorName = news.AuthorName,
                Source = news.Source,
                PublishedAt = news.PublishedAt,
                IsPublished = news.IsPublished,
                CreatedAt = news.CreatedAt,
                UpdatedAt = news.UpdatedAt,
                Images = news.Images?.Select(img => new NewsImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    IsPrimary = img.IsPrimary,
                    Order = img.Order
                }).ToList() ?? new List<NewsImageDto>()
            });
        }

        public async Task<IEnumerable<NewsViewDto>> GetPublishedAsync()
        {
            var published = await _repo.GetPublishedAsync();
            return published.Select(news => new NewsViewDto
            {
                Id = news.Id,
                Title = news.Title,
                Slug = news.Slug,
                Content = news.Content,
                Summary = news.Summary,
                AuthorName = news.AuthorName,
                Source = news.Source,
                PublishedAt = news.PublishedAt,
                IsPublished = news.IsPublished,
                CreatedAt = news.CreatedAt,
                UpdatedAt = news.UpdatedAt,
                Images = news.Images?.Select(img => new NewsImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    IsPrimary = img.IsPrimary,
                    Order = img.Order
                }).ToList() ?? new List<NewsImageDto>()
            });
        }

        public async Task<bool> UpdateAsync(NewsUpdateDto dto)
        {
            var news = await _repo.GetByIdAsync(dto.Id);
            if (news == null) return false;

            news.Title = dto.Title;
            news.Content = dto.Content;
            news.Summary = dto.Summary;
            news.AuthorName = dto.AuthorName;
            news.Source = dto.Source;
            news.UpdatedAt = DateTime.UtcNow;
            news.Slug = GenerateSlug(dto.Title);
            news.IsPublished = dto.IsPublished;
            if (dto.IsPublished && news.PublishedAt == null)
                news.PublishedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(news);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<bool> PublishAsync(int id)
        {
            return await _repo.PublishAsync(id);
        }

        private static string GenerateSlug(string title)
        {
            if (string.IsNullOrEmpty(title)) return "";

            // 1. Bỏ dấu tiếng Việt
            string slug = RemoveDiacritics(title).ToLowerInvariant();

            // 2. Thay khoảng trắng và dấu đặc biệt bằng -
            slug = Regex.Replace(slug, @"\s+", "-"); // Thay mọi khoảng trắng thành dấu -
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", ""); // Loại bỏ ký tự không hợp lệ

            // 3. Xóa bớt dấu gạch liên tiếp
            slug = Regex.Replace(slug, @"\-{2,}", "-");

            // 4. Xóa đầu/cuối là dấu -
            slug = slug.Trim('-');

            return slug;
        }

        // Hàm bỏ dấu tiếng Việt (có thể copy vào project)
        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalized.Length);

            foreach (var ch in normalized)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (cat != UnicodeCategory.NonSpacingMark)
                    sb.Append(ch);
            }

            var noMarks = sb.ToString().Normalize(NormalizationForm.FormC);

            // Quan trọng: đ/Đ không phải NonSpacingMark => phải map thủ công
            noMarks = noMarks.Replace('đ', 'd').Replace('Đ', 'D');
            return noMarks;
        }

    }

}
