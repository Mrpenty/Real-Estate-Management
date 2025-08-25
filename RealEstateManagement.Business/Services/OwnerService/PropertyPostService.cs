using Microsoft.EntityFrameworkCore;
using Nest;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.AddressRepo;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity.AddressEnity;
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
        private readonly IPropertyPostRepository _postRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IPropertyImageRepository _imageRepository;
        private readonly RentalDbContext _context;

        public PropertyPostService(IPropertyPostRepository postRepository, IAddressRepository addressRepository, IPropertyImageRepository imageRepository, RentalDbContext context)
        {
            _postRepository = postRepository;
            _addressRepository = addressRepository;
            _imageRepository = imageRepository;
            _context = context;
        }

        //Landlord tạo 1 bài đăng mới với status Draft
        public async Task<int> CreatePropertyPostAsync(PropertyCreateRequestDto dto, int landlordId)
        {
            // 1. Validate data
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Tiêu đề không được để trống.");
            if (dto.Area <= 0 || dto.Price <= 0)
                throw new ArgumentException("Diện tích và giá phải lớn hơn 0.");
            if (dto.ProvinceId <= 0 || dto.WardId <= 0 || dto.StreetId <= 0)
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
                PropertyTypeId = dto.PropertyTypeId,
                Area = dto.Area,
                Bedrooms = dto.Bedrooms,
                Bathrooms = dto.Bathrooms,
                Floors = dto.Floors,
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
                CreatedAt = DateTime.UtcNow,
                ArchiveDate = DateTime.UtcNow.AddDays(90)
            };

            // 5. Call Repository to save the post, property, and amenities
            // Repository now returns property ID
            var propertyId = await _postRepository.CreatePropertyPostAsync(property, post, dto.AmenityIds);

            // 6. Cập nhật lại PropertyId cho Address
            address.PropertyId = propertyId;
            await _addressRepository.SaveChangesAsync();

            return propertyId;
        }

        //Hàm lấy PropertyPostId để thêm ảnh
        public async Task<PropertyPost> GetPostByIdAsync(int postId)
        {
            return await _postRepository.GetByIdAsync(postId);
        }

        public async Task<object?> GetPostDetailForAdminAsync(int postId)
        {
            var post = await _postRepository.GetPostDetailForAdminAsync(postId);
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
                    post.Property.PropertyType.Name,
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
            // 1. Xử lý input status
            PropertyPost.PropertyPostStatus? statusEnum = null;
            if (!string.IsNullOrEmpty(status))
                statusEnum = Enum.Parse<PropertyPost.PropertyPostStatus>(status, true);

            // 2. Gọi repo để lấy dữ liệu
            int total = await _postRepository.CountByStatusAsync(statusEnum);
            var posts = await _postRepository.GetPostsByStatusAsync(statusEnum, page, pageSize);

            // 3. Mapping kết quả trả ra
            var mappedPosts = posts.Select(p => new
            {
                p.Id,
                p.Status,
                p.CreatedAt,
                LandlordName = p.Landlord?.Name,
                Property = p.Property == null ? null : new
                {
                    p.Property.Id,
                    p.Property.Title,
                    p.Property.Address,
                    p.Property.Area,
                    p.Property.Price,
                    p.Property.Description,
                }
            }).ToList();

            int totalPages = (int)Math.Ceiling((double)total / pageSize);

            return new
            {
                total,
                page,
                pageSize,
                totalPages,
                posts = mappedPosts
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
        public async Task<bool> UpdatePostStatusByPropertyIdAsync(int propertyId, string status)
        {
            var post = await _postRepository.GetByPropertyIdAsync(propertyId);
            if (post == null) return false;
            post.Status = Enum.Parse<PropertyPost.PropertyPostStatus>(status, true);
            await _postRepository.UpdateAsync(post);
            return true;
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
            property.PropertyTypeId = dto.PropertyTypeId;
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

        //Admin Ban và UnBan PropertyPost
        public async Task<bool> BanPropertyPost(int propertyId, string action, int adminId, string? adminNote)
        {
            var post = await _postRepository.GetPostWithPropertyAsync(propertyId);
            if (post == null || post.Property == null)
                return false;

            var reports = await _postRepository.GetReportsForPostAsync(propertyId);

            if (action.ToLower() == "ban")
            {
                post.Property.Status = "inactive";
                post.ArchiveDate = DateTime.UtcNow;

                foreach (var report in reports.Where(r => r.Status == "Pending"))
                {
                    report.Status = "Resolved";
                    report.ResolvedAt = DateTime.UtcNow;
                    report.ResolvedByUserId = adminId;
                    report.AdminNote = adminNote ?? "Bài đăng bị ban.";
                }
            }
            else if (action.ToLower() == "unban")
            {
                post.Property.Status = "active";
                post.ArchiveDate = null;

                foreach (var report in reports.Where(r => r.Status == "Resolved"))
                {
                    report.Status = "Ignored";
                    report.ResolvedAt = DateTime.UtcNow;
                    report.ResolvedByUserId = adminId;
                    report.AdminNote = adminNote ?? "Bài đăng đã được phục hồi.";
                }
            }
            else
            {
                throw new ArgumentException("Hành động không hợp lệ. Chỉ hỗ trợ 'ban' và 'unban'.");
            }

            await _postRepository.SaveChangesAsync();
            return true;
        }


    }
}