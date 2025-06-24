using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateManagement.Business.Repositories.AddressRepo;

namespace RealEstateManagement.Business.Services.OwnerService
{
    public class PropertyPostService : IPropertyPostService
    {
        private readonly IPropertyPostRepository _postRepository;
        private readonly IAddressRepository _addressRepository; // Inject IAddressRepository

        public PropertyPostService(IPropertyPostRepository postRepository, IAddressRepository addressRepository)
        {
            _postRepository = postRepository;
            _addressRepository = addressRepository;
        }

        public async Task<int> CreatePropertyPostAsync(PropertyCreateRequestDto dto, int landlordId)
        {
            // 1. Validate data
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Tiêu đề không được để trống.");
            if (dto.Area <= 0 || dto.Price <= 0)
                throw new ArgumentException("Diện tích và giá phải lớn hơn 0.");
            if (dto.ProvinceId <= 0 || dto.WardId <= 0 || string.IsNullOrWhiteSpace(dto.Street))
                throw new ArgumentException("Vui lòng chọn đầy đủ thông tin địa chỉ.");

            // 2. Get or create the Address
            var address = await _addressRepository.GetByDetailsAsync(dto.ProvinceId, dto.WardId, dto.Street);
            if (address == null)
            {
                address = new Address
                {
                    ProvinceId = dto.ProvinceId,
                    WardId = dto.WardId,
                    DetailedAddress = dto.Street
                };
                await _addressRepository.AddAsync(address);
                await _addressRepository.SaveChangesAsync();
            }

            // 3. Initialize Property with the AddressId
            var property = new Property
            {
                Title = dto.Title,
                Description = dto.Description,
                AddressId = address.Id, // Use the found or created address Id
                Type = dto.Type,
                Area = dto.Area,
                Bedrooms = dto.Bedrooms,
                Price = dto.Price,
                LandlordId = landlordId,
                Status = "active",
                IsPromoted = false, // Giả định giá trị mặc định
                IsVerified = false,
                ViewsCount = 0, // Giá trị mặc định
                Location = dto.Location, // Gán Location từ dto
                CreatedAt = DateTime.UtcNow
            };

            // 4. Initialize the post
            var post = new PropertyPost
            {
                LandlordId = landlordId,
                Status = PropertyPost.PropertyPostStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            // 5. Call Repository to save the post, property, and amenities
            return await _postRepository.CreatePropertyPostAsync(property, post, dto.AmenityIds);
        }
    }
}
