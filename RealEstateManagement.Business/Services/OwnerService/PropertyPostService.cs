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
        private readonly IAddressRepository _addressRepository; 
        private readonly IPropertyImageRepository _imageRepository;

        public PropertyPostService(IPropertyPostRepository postRepository, IAddressRepository addressRepository, IPropertyImageRepository imageRepository)
        {
            _postRepository = postRepository;
            _addressRepository = addressRepository;
            _imageRepository = imageRepository;
        }

        //Landlord tạo 1 bài đăng mới với status Draft
        public async Task<int> CreatePropertyPostAsync(PropertyCreateRequestDto dto, int landlordId)
        {
            // 1. Validate data
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Tiêu đề không được để trống.");
            if (dto.Area <= 0 || dto.Price <= 0)
                throw new ArgumentException("Diện tích và giá phải lớn hơn 0.");
            if (dto.ProvinceId <= 0 || dto.WardId <= 0 || dto.StreetId <=0)
                throw new ArgumentException("Vui lòng chọn đầy đủ thông tin địa chỉ.");

            // 2. Luôn tạo mới Address cho mỗi Property
            var address = new Address
            {
                ProvinceId = dto.ProvinceId,
                WardId = dto.WardId,
                StreetId = dto.StreetId,
                DetailedAddress = dto.DetailedAddress
            };
            await _addressRepository.AddAsync(address);
            await _addressRepository.SaveChangesAsync();

            // 3. Initialize Property with the AddressId
            var property = new Property
            {
                Title = dto.Title,
                Description = dto.Description,
                AddressId = address.Id, // Use the newly created address Id
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
            // Repository now returns property ID
            return await _postRepository.CreatePropertyPostAsync(property, post, dto.AmenityIds);
        }

        //Hàm lấy PropertyPostId để thêm ảnh
        public async Task<PropertyPost> GetPostByIdAsync(int postId)
        {
            return await _postRepository.GetByIdAsync(postId);
        }


        public async Task ContinueDraftPostAsync(ContinuePropertyPostDto dto, int landlordId)
        {
            // Tìm Post & Property liên quan
            var post = await _postRepository.GetPropertyPostByIdAsync(dto.PostId, landlordId);
            if (post == null)
                throw new Exception("Không tìm thấy bài đăng nháp.");
            if (post.Status != PropertyPost.PropertyPostStatus.Draft)
                throw new Exception("Bài đăng không phải trạng thái Draft.");

            var property = post.Property;

            // Update Property
            property.Title = dto.Title;
            property.Description = dto.Description;
            property.Type = dto.Type;
            property.Area = dto.Area;
            property.Bedrooms = dto.Bedrooms;
            property.Price = dto.Price;
            property.Location = dto.Location;

            // Update Address
            var address = property.Address;
            address.ProvinceId = dto.ProvinceId;
            address.WardId = dto.WardId;
            address.StreetId = dto.StreetId;
            address.DetailedAddress = dto.DetailedAddress;

            await _addressRepository.UpdateAsync(address);

            // Update Amenities
            await _postRepository.UpdatePropertyAmenities(property.Id, dto.AmenityIds);

            // Update Images
            if (dto.Images == null || !dto.Images.Any())
                throw new Exception("Phải có ít nhất 1 ảnh.");

            foreach (var imgDto in dto.Images)
            {
                // Nếu Id > 0: Cập nhật
                if (imgDto.Id > 0)
                {
                    await _imageRepository.UpdateImageAsync(new PropertyImage
                    {
                        Id = imgDto.Id,
                        PropertyId = property.Id,
                        Url = imgDto.Url,
                        IsPrimary = imgDto.IsPrimary,
                        Order = imgDto.Order
                    });
                }
                else
                {
                    // Nếu là ảnh mới: Thêm
                    await _imageRepository.AddImageAsync(new PropertyImage
                    {
                        PropertyId = property.Id,
                        Url = imgDto.Url,
                        IsPrimary = imgDto.IsPrimary,
                        Order = imgDto.Order
                    });
                }
            }

            // Kiểm tra lại: BĐS phải có ít nhất 1 ảnh
            var hasImage = await _imageRepository.HasAnyImage(property.Id);
            if (!hasImage)
                throw new Exception("BĐS phải có ít nhất 1 ảnh.");

            // Đổi Status & UpdatedAt
            post.Status = PropertyPost.PropertyPostStatus.Pending;
            post.UpdatedAt = DateTime.UtcNow;

            await _postRepository.UpdateAsync(post);
        }

        

    }
}
