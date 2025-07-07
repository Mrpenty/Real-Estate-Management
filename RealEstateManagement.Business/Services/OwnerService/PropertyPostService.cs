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
using Microsoft.EntityFrameworkCore;

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

        public async Task<PropertyPost> GetPostByIdAsync(int postId)
        {
            return await _postRepository.GetByIdAsync(postId);
        }

        public async Task<object> GetPostDetailForAdminAsync(int postId)
        {
            var post = await _postRepository.GetAll()
                .Include(p => p.Property)
                    .ThenInclude(prop => prop.Address)
                        .ThenInclude(addr => addr.Province)
                .Include(p => p.Property)
                    .ThenInclude(prop => prop.Address)
                        .ThenInclude(addr => addr.Ward)
                .Include(p => p.Property)
                    .ThenInclude(prop => prop.Address)
                        .ThenInclude(addr => addr.Street)
                .Include(p => p.Property)
                    .ThenInclude(prop => prop.Images)
                .Include(p => p.Property)
                    .ThenInclude(prop => prop.PropertyAmenities)
                        .ThenInclude(pa => pa.Amenity)
                .Include(p => p.Landlord) // Include landlord information
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null) return null;

            return new
            {
                post.Id,
                Status = post.Status.ToString(),
                StatusDisplay = GetStatusDisplayName(post.Status),
                post.CreatedAt,
                post.LandlordId,
                Landlord = post.Landlord == null ? null : new
                {
                    post.Landlord.Id,
                    post.Landlord.UserName,
                    post.Landlord.Email,
                    post.Landlord.PhoneNumber,
                    post.Landlord.Name,
                },
                Property = post.Property == null ? null : new
                {
                    post.Property.Id,
                    post.Property.Title,
                    post.Property.Description,
                    post.Property.Area,
                    post.Property.Price,
                    post.Property.Type,
                    post.Property.Bedrooms,
                    post.Property.Location,
                    post.Property.Status,
                    post.Property.IsPromoted,
                    post.Property.IsVerified,
                    post.Property.ViewsCount,
                    post.Property.CreatedAt,
                    Address = post.Property.Address == null ? null : new
                    {
                        post.Property.Address.Id,
                        post.Property.Address.DetailedAddress,
                        Province = post.Property.Address.Province == null ? null : new
                        {
                            post.Property.Address.Province.Id,
                            post.Property.Address.Province.Name
                        },
                        Ward = post.Property.Address.Ward == null ? null : new
                        {
                            post.Property.Address.Ward.Id,
                            post.Property.Address.Ward.Name
                        },
                        Street = post.Property.Address.Street == null ? null : new
                        {
                            post.Property.Address.Street.Id,
                            post.Property.Address.Street.Name
                        }
                    },
                    Images = post.Property.Images?.Select(img => new
                    {
                        img.Id,
                        img.IsPrimary,
                        img.Url,
                        img.Order,
                    }).ToList(),
                    Amenities = post.Property.PropertyAmenities?.Select(pa => new
                    {
                        pa.Amenity.Id,
                        pa.Amenity.Name,
                        pa.Amenity.Description
                    }).ToList()
                }
            };
        }

      

        public async Task<object> GetPostsByStatusAsync(string status, int page, int pageSize)
        {
            var statusEnum = Enum.Parse<PropertyPost.PropertyPostStatus>(status, true);
            var query = _postRepository.GetAll()
                .Include(p => p.Property)
                .Include(p => p.Landlord)
                .Where(p => p.Status == statusEnum);
            int total = await query.CountAsync();
            var posts = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new {
                    p.Id,
                    
                    p.Status,
                    p.CreatedAt,
                    p.Landlord.Name,
                    Property = p.Property == null ? null : new {
                        p.Property.Id,
                        p.Property.Title,
                        p.Property.Address,
                        p.Property.Area,
                        p.Property.Price,
                        p.Property.Description,
                    }
                })
                .ToListAsync();

            int totalPages = (int)Math.Ceiling((double)total / pageSize);
            return new {
                total,
                page,
                pageSize,
                totalPages,
                posts
            };
        }

        public async Task<bool> UpdatePostStatusAsync(int id, string status)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null) return false;
            post.Status = Enum.Parse<PropertyPost.PropertyPostStatus>(status, true);
           
            await _postRepository.UpdateAsync(post);
            return true;
        }

        private string GetStatusDisplayName(PropertyPost.PropertyPostStatus status)
        {
            return status switch
            {
                PropertyPost.PropertyPostStatus.Draft => "Bản nháp",
                PropertyPost.PropertyPostStatus.Pending => "Chờ duyệt",
                PropertyPost.PropertyPostStatus.Approved => "Đã duyệt",
                PropertyPost.PropertyPostStatus.Rejected => "Đã từ chối",
                _ => status.ToString()
            };
        }
    }
}
