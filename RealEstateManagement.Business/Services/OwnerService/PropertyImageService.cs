using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstateManagement.Data.Entity.PropertyEntity.PropertyPost;

namespace RealEstateManagement.Business.Services.OwnerService
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IPropertyImageRepository _imageRepo;
        private readonly IPropertyPostRepository _postRepo;

        public PropertyImageService(IPropertyImageRepository imageRepo, IPropertyPostRepository postRepo)
        {
            _imageRepo = imageRepo;
            _postRepo = postRepo;
        }

        public async Task<PropertyImage> AddImageAsync(int propertyId, PropertyImageCreateDto dto)
        {
            // ✅ Kiểm tra property có tồn tại không
            var propertyExists = await _imageRepo.PropertyExistsAsync(propertyId);
            if (!propertyExists)
                throw new ArgumentException("Không tìm thấy bất động sản với ID đã cung cấp.");

            // ✅ Validate dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(dto.Url))
                throw new ArgumentException("Ảnh phải có đường dẫn (URL).");

            if (dto.Url.Length > 500)
                throw new ArgumentException("Đường dẫn ảnh quá dài (tối đa 500 ký tự).");

            if (dto.Order < 0)
                throw new ArgumentException("Thứ tự ảnh không hợp lệ (phải lớn hơn hoặc bằng 0).");

            // ✅ Tạo entity
            var image = new PropertyImage
            {
                PropertyId = propertyId,
                Url = dto.Url,
                IsPrimary = dto.IsPrimary,
                Order = dto.Order
            };

            var savedImage = await _imageRepo.AddImageAsync(image);

            // ✅ Nếu bài đăng đang ở trạng thái Nháp => chuyển sang Chờ duyệt
            var post = await _postRepo.GetByPropertyIdAsync(propertyId);
            if (post != null && post.Status == PropertyPostStatus.Draft)
            {
                post.Status = PropertyPostStatus.Pending;
                await _postRepo.UpdateAsync(post);
            }

            return savedImage;
        }

    }

}
