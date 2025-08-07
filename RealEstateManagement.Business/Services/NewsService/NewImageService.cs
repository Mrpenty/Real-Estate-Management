using Microsoft.Extensions.Logging;
using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.NewsRepository;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.NewsService
{
    public class NewImageService: INewImageService
    {
        private readonly INewsImageRepository _imageRepo;
        private readonly INewsRepository _newRepo;
        private readonly ILogger<NewImageService> _logger;

        public NewImageService(INewsImageRepository imageRepo, INewsRepository newRepo, ILogger<NewImageService> logger)
        {
            _imageRepo = imageRepo;
            _newRepo = newRepo;
            _logger = logger;
        }
        public async Task<NewsImage> AddImageAsync(int newId, NewImageCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Starting AddImageAsync for new {newId}", newId);

                // ✅ Kiểm tra new có tồn tại không
                var newExists = await _imageRepo.NewsExistsAsync(newId);
                if (!newExists)
                {
                    _logger.LogError("new {newId} not found", newId);
                    throw new ArgumentException("Không tìm thấy tin tức với ID đã cung cấp.");
                }

                _logger.LogInformation("New {id} exists", newId);

                // ✅ Validate dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(dto.ImageUrl))
                {
                    _logger.LogError("URL is null or empty for new {newId}", newId);
                    throw new ArgumentException("Ảnh phải có đường dẫn (URL).");
                }

                if (dto.ImageUrl.Length > 500)
                {
                    _logger.LogError("URL too long for New {newId}: {UrlLength}", newId, dto.ImageUrl.Length);
                    throw new ArgumentException("Đường dẫn ảnh quá dài (tối đa 500 ký tự).");
                }

                if (dto.Order < 0)
                {
                    _logger.LogError("Invalid order for New {newId}: {Order}", newId, dto.Order);
                    throw new ArgumentException("Thứ tự ảnh không hợp lệ (phải lớn hơn hoặc bằng 0).");
                }

                _logger.LogInformation("Validation passed for New {newId}", newId);

                // ✅ Tạo entity
                var image = new NewsImage
                {
                    NewsId = newId,
                    ImageUrl = dto.ImageUrl,
                    IsPrimary = dto.IsPrimary,
                    Order = dto.Order
                };

                _logger.LogInformation("Created NewsImage entity for new {newId}, URL: {ImageUrl}", newId, dto.ImageUrl);

                var savedImage = await _imageRepo.AddImageAsync(image);

                _logger.LogInformation("Image saved to database for new {newId}, ImageId: {Id}", newId, savedImage.Id);

                return savedImage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddImageAsync for new {newId}", newId);
                throw;
            }
        }
        public async Task<NewsImage> UpdateImageAsync(NewsImage updatedImage)
        {
            return await _imageRepo.UpdateImageAsync(updatedImage);
        }

        public async Task<IEnumerable<NewsImageDto>> GetImagesByNewsIdAsync(int newId)
        {
            try
            {
                _logger.LogInformation("Getting images for new {newId}", newId);
                
                var images = await _imageRepo.GetImagesByNewsIdAsync(newId);
                
                return images.Select(img => new NewsImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    IsPrimary = img.IsPrimary,
                    Order = img.Order
                }).OrderBy(img => img.Order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetImagesByNewsIdAsync for new {newId}", newId);
                throw;
            }
        }
    }
}
