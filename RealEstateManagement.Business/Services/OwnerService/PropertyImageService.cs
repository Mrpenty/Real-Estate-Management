using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstateManagement.Data.Entity.PropertyEntity.PropertyPost;
using Microsoft.Extensions.Logging;

namespace RealEstateManagement.Business.Services.OwnerService
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IPropertyImageRepository _imageRepo;
        private readonly IPropertyPostRepository _postRepo;
        private readonly ILogger<PropertyImageService> _logger;

        public PropertyImageService(IPropertyImageRepository imageRepo, IPropertyPostRepository postRepo, ILogger<PropertyImageService> logger)
        {
            _imageRepo = imageRepo;
            _postRepo = postRepo;
            _logger = logger;
        }

        public async Task<PropertyImage> AddImageAsync(int propertyId, PropertyImageCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Starting AddImageAsync for property {PropertyId}", propertyId);
                
                // ✅ Kiểm tra property có tồn tại không
                var propertyExists = await _imageRepo.PropertyExistsAsync(propertyId);
                if (!propertyExists)
                {
                    _logger.LogError("Property {PropertyId} not found", propertyId);
                    throw new ArgumentException("Không tìm thấy bất động sản với ID đã cung cấp.");
                }

                _logger.LogInformation("Property {PropertyId} exists", propertyId);

                // ✅ Validate dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(dto.Url))
                {
                    _logger.LogError("URL is null or empty for property {PropertyId}", propertyId);
                    throw new ArgumentException("Ảnh phải có đường dẫn (URL).");
                }

                if (dto.Url.Length > 500)
                {
                    _logger.LogError("URL too long for property {PropertyId}: {UrlLength}", propertyId, dto.Url.Length);
                    throw new ArgumentException("Đường dẫn ảnh quá dài (tối đa 500 ký tự).");
                }

                if (dto.Order < 0)
                {
                    _logger.LogError("Invalid order for property {PropertyId}: {Order}", propertyId, dto.Order);
                    throw new ArgumentException("Thứ tự ảnh không hợp lệ (phải lớn hơn hoặc bằng 0).");
                }

                _logger.LogInformation("Validation passed for property {PropertyId}", propertyId);

                // ✅ Tạo entity
                var image = new PropertyImage
                {
                    PropertyId = propertyId,
                    Url = dto.Url,
                    IsPrimary = dto.IsPrimary,
                    Order = dto.Order
                };

                _logger.LogInformation("Created PropertyImage entity for property {PropertyId}, URL: {Url}", propertyId, dto.Url);

                var savedImage = await _imageRepo.AddImageAsync(image);

                _logger.LogInformation("Image saved to database for property {PropertyId}, ImageId: {ImageId}", propertyId, savedImage.Id);

                // ✅ Nếu bài đăng đang ở trạng thái Nháp => chuyển sang Chờ duyệt
                var post = await _postRepo.GetByPropertyIdAsync(propertyId);
                if (post != null && post.Status == PropertyPostStatus.Draft)
                {
                    _logger.LogInformation("Updating post status from Draft to Pending for property {PropertyId}", propertyId);
                    post.Status = PropertyPostStatus.Pending;
                    await _postRepo.UpdateAsync(post);
                }

                return savedImage;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddImageAsync for property {PropertyId}", propertyId);
                throw;
            }
        }
        public async Task<PropertyImage> UpdateImageAsync(PropertyImage updatedImage)
        {
            return await _imageRepo.UpdateImageAsync(updatedImage);
        }

    }

}
