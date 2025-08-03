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

namespace RealEstateManagement.Business.Services.OwnerService
{
    public class OwnerPropertyService : IOwnerPropertyService
    {
        private readonly IOwnerPropertyRepository _ownerPropertyRepo;
        private readonly RentalDbContext _rentalDbContext;

        public OwnerPropertyService(IOwnerPropertyRepository ownerPropertyRepo, RentalDbContext rentalDbContext)
        {
            _ownerPropertyRepo = ownerPropertyRepo;
            _rentalDbContext = rentalDbContext;
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
                    CreatedAt = post.CreatedAt
                }).ToList()
            };

            return dto;
        }


        public async Task UpdatePropertyAsync(OwnerUpdatePropertyDto dto, int landlordId)
        {
            var property = await _ownerPropertyRepo.GetByIdAsync(dto.Id, landlordId);
            if (property == null)
                throw new Exception("Property not found or not owned by landlord.");

            property.Title = dto.Title;
            property.Description = dto.Description;
            property.Price = dto.Price;
            property.Area = dto.Area;
            property.Bedrooms = dto.Bedrooms;
            // ...

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
                or PropertyPost.PropertyPostStatus.Rented)
                return (false, "Cannot extend a post that is rejected, sold, or rented.");

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

    }
}
