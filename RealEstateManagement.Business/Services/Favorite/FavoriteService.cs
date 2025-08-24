using Nest;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Repositories.FavoriteRepository;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Favorite
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _repository;
        private readonly RentalDbContext _context;

        public FavoriteService(IFavoriteRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> AddToFavoriteAsync(int userId, int propertyId)
        {
            return await _repository.AddFavoritePropertyAsync(userId, propertyId);
        }
        public async Task<bool> RemoveFavoritePropertyAsync(int userId, int propertyId)
        {
            return await _repository.RemoveFavoritePropertyAsync(userId, propertyId);
        }
        public async Task<IEnumerable<HomePropertyDTO>> AllFavoritePropertyAsync(int userId)
        {
            var properties = await _repository.AllFavoritePropertyAsync(userId);
            return properties.Select(p => new HomePropertyDTO
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Type = p.PropertyType.Name,
                AddressID = p.AddressId,
                Area = p.Area,
                Bedrooms = p.Bedrooms,
                Price = p.Price,
                Status = p.Status,
                Location = p.Location,
                DetailedAddress = p.Address.DetailedAddress,
                Province = p.Address.Province.Name,
                Ward = p.Address.Ward.Name,
                Street = p.Address.Street.Name,
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
