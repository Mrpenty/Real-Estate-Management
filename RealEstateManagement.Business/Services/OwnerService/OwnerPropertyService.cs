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

        public OwnerPropertyService(IOwnerPropertyRepository ownerPropertyRepo)
        {
            _ownerPropertyRepo = ownerPropertyRepo;
        }
        public IQueryable<OwnerPropertyDto> GetPropertiesByLandlordQueryable(int landlordId)
        {
            return _ownerPropertyRepo.GetByLandlordQueryable(landlordId)
                .Select(p => new OwnerPropertyDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    IsPromoted = p.IsPromoted,
                    IsVerified = p.IsVerified,
                    Location = p.Location,
                    Bedrooms = p.Bedrooms,
                    Area = p.Area,
                    Images = p.Images.Select(img => new OwnerPropertyImageDto
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
    }
}
