using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Repositories.SearchProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.SearchProperties
{
    public class SearchProService : ISearchProService
    {
        private readonly ISearchProRepo _repository;
        private readonly RentalDbContext _context;
        private readonly IPropertyRepository _repository1;
        public SearchProService(ISearchProRepo repository, RentalDbContext context, IPropertyRepository repository1)
        {
            _repository = repository;
            _context = context;
            _repository1 = repository1;
        }
        public async Task<bool> IndexPropertyAsync(PropertySearchDTO dto)
        {
            return await _repository.IndexPropertyAsync(dto);
        }

        public async Task BulkIndexPropertiesAsync()
        {
            var allProperties = await _repository1.GetAllAsync();

            var dtos = allProperties.Select(p => new PropertySearchDTO
            {
                Id = p.Id,
                PromotionPackageLevel = p.PropertyPromotions?
                    .OrderByDescending(pp => pp.PromotionPackage.Level)
                    .Select(pp => pp.PromotionPackage.Level)
                    .FirstOrDefault() ?? 0,
                Title = p.Title,
                Description = p.Description,
                Type = p.Type,
                Area = p.Area,
                AddressId = p.AddressId,
                DetailedAddress = p.Address.DetailedAddress,  // lấy từ entity Address
                Amenities = p.PropertyAmenities.Select(pa => pa.Amenity.Name).ToList()
            });

            await _repository.BulkIndexPropertiesAsync(dtos);
        }


        public async Task<IEnumerable<HomePropertyDTO>> SearchAsync(string keyword)
        {
            var ids = await _repository.SearchPropertyIdsAsync(keyword);

            if (!ids.Any()) return Enumerable.Empty<HomePropertyDTO>();

            var properties = await _context.Properties
                .Where(p => ids.Contains(p.Id))
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.PropertyPromotions).ThenInclude(pp => pp.PromotionPackage)

                .ToListAsync();

            return properties.Select(p => new HomePropertyDTO
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Type = p.Type,
                AddressID = p.AddressId,
                Area = p.Area,
                Bedrooms = p.Bedrooms,
                Price = p.Price,
                Status = p.Status,
                Location = p.Location,
                CreatedAt = p.CreatedAt,
                ViewsCount = p.ViewsCount,
                PrimaryImageUrl = p.Images?.FirstOrDefault(i => i.IsPrimary)?.Url,
                LandlordName = p.Landlord?.Name,
                LandlordPhoneNumber = p.Landlord?.PhoneNumber,
                LandlordProfilePictureUrl = p.Landlord?.ProfilePictureUrl,
                Amenities = p.PropertyAmenities?.Select(pa => pa.Amenity.Name).ToList() ?? new List<string>(),
                PromotionPackageName = p.PropertyPromotions?
                                            .OrderByDescending(pp => pp.PromotionPackage.Level)
                                            .Select(pp => pp.PromotionPackage.Name)
                                            .FirstOrDefault()
            });
        }
    }
}
