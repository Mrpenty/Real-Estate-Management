using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Data.Entity;
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
            var property = new Property
            {
                Title = dto.Title,
                Description = dto.Description,
                Address = dto.Address,
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

            var post = new PropertyPost
            {
                LandlordId = landlordId,
                Status = "pending",
                CreatedAt = DateTime.UtcNow
            };

            return await _repository.CreatePropertyPostAsync(property, post, dto.AmenityIds);
        }
    }

}
