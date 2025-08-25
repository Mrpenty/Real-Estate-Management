using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstateManagement.Data.Entity.PropertyEntity.PropertyPost;

namespace RealEstateManagement.Business.Services.OwnerService
{
    public class OwnerPropertyService : IOwnerPropertyService
    {
        private readonly IOwnerPropertyRepository _ownerPropertyRepo;
        private readonly IRentalContractRepository _rentalContractRepo;
        private readonly RentalDbContext _rentalDbContext;

        public OwnerPropertyService(IOwnerPropertyRepository ownerPropertyRepo, RentalDbContext rentalDbContext, IRentalContractRepository rentalContractRepo)
        {
            _ownerPropertyRepo = ownerPropertyRepo;
            _rentalDbContext = rentalDbContext;
            _rentalContractRepo = rentalContractRepo;
        }
        public IQueryable<OwnerPropertyDto> GetPropertiesByLandlordQueryable(int landlordId)
        {
            return _ownerPropertyRepo.GetByLandlordQueryable(landlordId)
                .Select(entity => new OwnerPropertyDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    Description = entity.Description,
                    Price = entity.Price, 
                    IsPromoted = entity.IsPromoted,
                    IsVerified = entity.IsVerified,
                    Location = entity.Location,
                    Bedrooms = entity.Bedrooms,
                    Area = entity.Area,
                    Province = entity.Address.Province.Name,
                    Ward = entity.Address.Ward.Name,
                    Street = entity.Address.Street.Name,
                    CreatedAt = entity.CreatedAt,
                    InterestNo = _rentalDbContext.InterestedProperties.Count(c => c.PropertyId == entity.Id && c.Status == Data.Entity.User.InterestedStatus.WaitingForLandlordReply),
                    Posts = entity.Posts.Select(post => new OwnerPropertyPostDto
                    {
                        Id = post.Id,
                        Status = post.Status.ToString(),
                        CreatedAt = post.CreatedAt
                    }).ToList(),
                    IsExistRenterContract = entity.Posts.Any(post => post.RentalContract != null),
                    RenterContractId = entity.Posts.Any(post => post.RentalContract != null) ? entity.Posts.FirstOrDefault().RentalContract.Id : 0,
                    Type = entity.Type,
                    DetailedAddress = entity.Address.DetailedAddress,
                    Images = entity.Images.Select(img => new OwnerPropertyImageDto
                    {
                        Id = img.Id,
                        Url = img.Url,
                        IsPrimary = img.IsPrimary
                    }).ToList()
                });
        }

        public async Task<OwnerPropertyDto> GetPropertyByIdAsync(int id, int landlordId)
        {
            var entity = await _ownerPropertyRepo.GetByIdAsync(id, landlordId);
            if (entity == null)
                throw new Exception("Property not found.");
            var interestDtos = await _rentalDbContext.InterestedProperties.Include(c => c.Renter).Where(c => c.PropertyId == id).ToListAsync();
            var dto = new OwnerPropertyDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Price = entity.Price,
                IsPromoted = entity.IsPromoted,
                IsVerified = entity.IsVerified,
                Location = entity.Location,
                Bedrooms = entity.Bedrooms,
                Area = entity.Area,
                Type = entity.Type,
                InterestedProperties = interestDtos.Select(c => new DTO.Properties.InterestedPropertyDTO
                {
                    Id = c.Id,
                    RenterId = c.RenterId,
                    RenterReplyAt = c.RenterReplyAt,
                    InterestedAt = c.InterestedAt,
                    RenterConfirmed = c.RenterConfirmed,
                    RenterName = c.Renter.Name,
                    RenterEmail = c.Renter.Email,
                    RenterPhone = c.Renter.PhoneNumber,
                    RenterUserName = c.Renter.UserName,
                    Status = (int)c.Status
                }).ToList(),
                Images = entity.Images?.Select(img => new OwnerPropertyImageDto
                {
                    Id = img.Id,
                    Url = img.Url,
                    IsPrimary = img.IsPrimary
                }).ToList(),

                Posts = entity.Posts?.Select(post => new OwnerPropertyPostDto
                {
                    Id = post.Id,
                    Status = post.Status.ToString(),
                    RentalContract = post.RentalContract == null ? null : new RentalContractViewDto
                    {
                        Id = post.RentalContract.Id,
                        PropertyPostId = post.RentalContract.PropertyPostId,
                        DepositAmount = post.RentalContract.DepositAmount,
                        MonthlyRent = post.RentalContract.MonthlyRent,
                        ContractDurationMonths = post.RentalContract.ContractDurationMonths,
                        PaymentCycle = post.RentalContract.PaymentCycle,
                        PaymentDayOfMonth = post.RentalContract.PaymentDayOfMonth,
                        StartDate = post.RentalContract.StartDate,
                        EndDate = post.RentalContract.EndDate,
                        PaymentMethod = post.RentalContract.PaymentMethod,
                        ContactInfo = post.RentalContract.ContactInfo,
                        Status = post.RentalContract.Status,
                        CreatedAt = post.RentalContract.CreatedAt,
                        ConfirmedAt = post.RentalContract.ConfirmedAt
                    }
                }).ToList(),

                Amenities = entity.PropertyAmenities?.Select(pa => pa.Amenity.Name).ToList() ?? new List<string>(),
                PrimaryImageUrl = entity.Images?.FirstOrDefault(i => i.IsPrimary)?.Url,
                ImageUrls = entity.Images?.Select(c => c.Url).ToList(),
                Street = entity.Address?.Street?.Name,
                Province = entity.Address?.Province?.Name,
                Ward = entity.Address?.Ward?.Name,
                DetailedAddress = entity.Address?.DetailedAddress,
                StreetId = entity.Address?.StreetId,
                ProvinceId = entity.Address?.ProvinceId,
                WardId = entity.Address?.WardId
            };

            return dto;
        }

        public async Task UpdatePropertyAsync(PropertyCreateRequestDto dto, int landlordId, int propertyId)
        {
            var property = await _ownerPropertyRepo.GetByIdAsync(propertyId, landlordId);
            if (property == null)
                throw new Exception("Property not found or not owned by landlord.");

            if (property.Posts.Any(post =>
    post.Status == PropertyPostStatus.Sold ||
    post.Status == PropertyPostStatus.Rented))
            {
                throw new InvalidOperationException("Bạn không thể Update bài viết này do bài viết này đã rơi vào 1 trong 2 trạng thái: Rented hoặc Sold.");
            }
            property.Title = dto.Title;
            property.Description = dto.Description;
            property.Price = dto.Price;
            property.Area = dto.Area;
            property.Bedrooms = dto.Bedrooms;
            property.Type = dto.Type;
            property.Location = dto.Location;

            if (dto.ProvinceId != null || dto.WardId != null || dto.StreetId != null || !string.IsNullOrEmpty(dto.DetailedAddress))
            {
                await _ownerPropertyRepo.UpdateAddressAsync(propertyId, dto.ProvinceId, dto.WardId, dto.StreetId, dto.DetailedAddress);
            }

            // Xử lý cập nhật Amenities nếu cần
            if (dto.AmenityIds != null && dto.AmenityIds.Any())
            {
                await _ownerPropertyRepo.UpdateAmenitiesAsync(propertyId, dto.AmenityIds);
            }

            var post = property.Posts.FirstOrDefault();
            if (post != null && post.Status != PropertyPostStatus.Draft)
            {
                post.Status = PropertyPostStatus.Pending;
            }
            await _ownerPropertyRepo.UpdateAsync(property);
        }

        public async Task DeletePropertyAsync(int id, int landlordId)
        {
            var property = await _ownerPropertyRepo.GetByIdAsync(id, landlordId);
            if (property == null)
                throw new Exception("Property not found or not owned by landlord.");

            await _ownerPropertyRepo.DeleteAsync(property);
        }

        //Gia hạn các property hết hạn
        public async Task<(bool IsSuccess, string Message)> ExtendPostAsync(int postId, int days, int landlordId)
        {
            var post = await _rentalDbContext.PropertyPosts
                .FirstOrDefaultAsync(p => p.Id == postId && p.LandlordId == landlordId);

            if (post == null)
                return (false, "Post not found or you do not have permission.");

            if (post.Status is PropertyPost.PropertyPostStatus.Rejected
                or PropertyPost.PropertyPostStatus.Sold
                or PropertyPost.PropertyPostStatus.Approved
                or PropertyPost.PropertyPostStatus.Draft
                or PropertyPost.PropertyPostStatus.Rented)
                return (false, "Cannot extend a post that is rejected, draft, sold, or rented.");

            if (days <= 0 || days > 90) // giới hạn tối đa 90 ngày
                return (false, "Invalid extension period. Max 90 days allowed.");

            // Nếu đã hết hạn → chuyển về Approved và set ArchiveDate mới
            if (post.Status == PropertyPost.PropertyPostStatus.Expired)
                post.Status = PropertyPost.PropertyPostStatus.Approved;

            post.ArchiveDate = (post.ArchiveDate ?? DateTime.UtcNow).AddDays(days);
            post.UpdatedAt = DateTime.UtcNow;

            await _rentalDbContext.SaveChangesAsync();
            return (true, $"Post extended to {post.ArchiveDate?.ToString("dd/MM/yyyy")}");
        }

        //Lấy danh sách các bài đăng bất động sản đã cho thuê
        public async Task<List<OwnerPropertyDto>> GetRentedPropertiesByLandlordIdAsync(int landlordId)
        {
            var properties = await _ownerPropertyRepo.GetRentedPropertiesByLandlordIdAsync(landlordId);

            return properties.Select(entity => new OwnerPropertyDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Price = entity.Price,
                IsPromoted = entity.IsPromoted,
                IsVerified = entity.IsVerified,
                Location = entity.Location,
                Bedrooms = entity.Bedrooms,
                Area = entity.Area,
                CreatedAt = entity.CreatedAt,
                Posts = entity.Posts
                    .Where(post => post.Status == PropertyPost.PropertyPostStatus.Rented)
                    .Select(post => new OwnerPropertyPostDto
                    {
                        Id = post.Id,
                        Status = post.Status.ToString(),
                        CreatedAt = post.CreatedAt,
                        RentalContract = post.RentalContract == null ? null : new RentalContractViewDto
                        {
                            Id = post.RentalContract.Id,
                            PropertyPostId = post.RentalContract.PropertyPostId,
                            Status = post.RentalContract.Status,
                            RenterId = post.RentalContract.RenterId,
                            DepositAmount = post.RentalContract.DepositAmount,
                            MonthlyRent = post.RentalContract.MonthlyRent,
                            ContractDurationMonths = post.RentalContract.ContractDurationMonths,
                            PaymentCycle = post.RentalContract.PaymentCycle,
                            PaymentDayOfMonth = post.RentalContract.PaymentDayOfMonth,
                            StartDate = post.RentalContract.StartDate,
                            EndDate = post.RentalContract.EndDate,
                            PaymentMethod = post.RentalContract.PaymentMethod,
                            ContactInfo = post.RentalContract.ContactInfo,
                            CreatedAt = post.RentalContract.CreatedAt,
                            ConfirmedAt = post.RentalContract.ConfirmedAt,
                            LastPaymentDate = post.RentalContract.LastPaymentDate
                        }
                    }).ToList(),
                IsExistRenterContract = entity.Posts.Any(post => post.Status == PropertyPost.PropertyPostStatus.Rented && post.RentalContract != null),
                Type = entity.Type,
            }).ToList();
        }
    }
}
