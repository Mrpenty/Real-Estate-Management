using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.OwnerService
{
    public class PropertyPostService : IPropertyPostService
    {
        private readonly IPropertyPostRepository _repository;

        public PropertyPostService(IPropertyPostRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreatePropertyPostAsync(PropertyCreateRequestDto dto, int landlordId)
        {
            // 1. Kiểm tra dữ liệu bắt buộc
            if (string.IsNullOrWhiteSpace(dto.Title) )
                throw new ArgumentException("Tiêu đề và địa chỉ không được để trống.");

            if (dto.Area <= 0 || dto.Price <= 0)
                throw new ArgumentException("Diện tích và giá phải lớn hơn 0.");

            if (dto.AmenityIds == null || !dto.AmenityIds.Any())
                throw new ArgumentException("Vui lòng chọn ít nhất một tiện nghi.");

            // 2. Khởi tạo đối tượng Property
            var property = new Property
            {
                Title = dto.Title,
                Description = dto.Description,
                AddressId = dto.AddressID,
                Type = dto.Type,
                Area = dto.Area,
                Bedrooms = dto.Bedrooms,
                Price = dto.Price,
                Location = dto.Location,
                LandlordId = landlordId,
                Status = "active",
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            // 3. Khởi tạo bài đăng (post)
            var post = new PropertyPost
            {
                LandlordId = landlordId,
                Status = PropertyPost.PropertyPostStatus.Draft, // ban đầu là bản nháp
                CreatedAt = DateTime.UtcNow
            };

            // 4. Gọi Repository để lưu vào cơ sở dữ liệu
            return await _repository.CreatePropertyPostAsync(property, post, dto.AmenityIds);
        }

    }

}
