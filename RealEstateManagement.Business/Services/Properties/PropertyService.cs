using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using RealEstateManagement.Business.DTO.Location;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.AddressEnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Properties
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _repository;
        private readonly RentalDbContext _context;

        public PropertyService(IPropertyRepository repository, RentalDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<IEnumerable<HomePropertyDTO>> GetAllPropertiesAsync(int? userId = 0)
        {
            var properties = await _repository.GetAllAsync();
            var favoriteUsers = await _context.UserFavoriteProperties.Where(c => c.UserId == userId).ToListAsync();
            return properties.Select(p => new HomePropertyDTO
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Type = p.Type,
                AddressID = p.AddressId,
                StreetId = p.Address.StreetId,
                Street = p.Address.Street.Name,
                ProvinceId = p.Address.ProvinceId,
                Province = p.Address.Province.Name,
                WardId = p.Address.WardId,
                Ward = p.Address.Ward.Name,
                DetailedAddress = p.Address.DetailedAddress,
                Area = p.Area,
                Bedrooms = p.Bedrooms,
                IsFavorite = favoriteUsers.FirstOrDefault(c => c.UserId == userId && c.PropertyId == p.Id) != null ? true : false,
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
        public async Task<PropertyDetailDTO> GetPropertyByIdAsync(int id)
        {
            var p = await _repository.GetPropertyByIdAsync(id);
            var rentalContract = p.Posts
                .FirstOrDefault(pp => pp.RentalContract != null)?.RentalContract;
            if (p == null)
            {
                throw new KeyNotFoundException($"Property with ID = {id} not found.");
            }

            return new PropertyDetailDTO
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
                                        .FirstOrDefault(),

                // Mapping thêm thông tin hợp đồng
                ContractDeposit = rentalContract?.DepositAmount,
                ContractMonthlyRent = rentalContract?.MonthlyRent,
                ContractDurationMonths = rentalContract?.ContractDurationMonths,
                ContractStartDate = rentalContract?.StartDate,
                ContractEndDate = rentalContract?.EndDate,
                ContractStatus = rentalContract?.Status.ToString(),
                ContractPaymentMethod = rentalContract?.PaymentMethod,
                ContractContactInfo = rentalContract?.ContactInfo,
                Street = p.Address.Street.Name,
                Province = p.Address.Province.Name,
                Ward = p.Address.Ward.Name,
                DetailedAddress = p.Address.DetailedAddress,

            };
        }
        public async Task<IEnumerable<HomePropertyDTO>> FilterByPriceAsync([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var p = await _repository.FilterByPriceAsync(minPrice, maxPrice);
            if (p == null) return null;
            return p.Select(p => new HomePropertyDTO
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
        public async Task<IEnumerable<HomePropertyDTO>> FilterByAreaAsync([FromQuery] decimal? minArea, [FromQuery] decimal? maxArea)
        {
            var p = await _repository.FilterByAreaAsync(minArea, maxArea);
            if (p == null) return null;
            return p.Select(p => new HomePropertyDTO
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
        public async Task<List<PropertyDetailDTO>> GetPropertiesByIdsAsync(List<int> ids)
        {
            var properties = await _repository.GetPropertiesByIdsAsync(ids);

            return properties.Select(p => new PropertyDetailDTO
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
                LandlordName = p.Landlord?.Name,
                LandlordPhoneNumber = p.Landlord?.PhoneNumber,
                LandlordProfilePictureUrl = p.Landlord?.ProfilePictureUrl,
                LandlordCreatedAt = p.Landlord?.CreatedAt ?? DateTime.MinValue,
                Amenities = p.PropertyAmenities.Select(pa => pa.Amenity.Name).ToList(),
                ImageUrls = p.Images.Select(i => i.Url).ToList()
            }).ToList();
        }

        public async Task<IEnumerable<HomePropertyDTO>> FilterAdvancedAsync(PropertyFilterDTO filter)
        {
            var properties = await _repository.FilterAdvancedAsync(filter);
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
                ProvinceId = p.Address.ProvinceId,
                Province = p.Address.Province.Name,
                WardId = p.Address.WardId,
                Ward = p.Address.Ward.Name,
                StreetId = p.Address.StreetId,
                Street = p.Address.Street.Name,
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


        public async Task<IEnumerable<ComparePropertyDTO>> ComparePropertiesAsync(List<int> ids)
        {

            var props = await _repository.GetPropertiesByIdsAsync(ids);
            // Kiểm tra nếu thiếu ID
            if (props.Count != ids.Count)
            {
                var foundIds = props.Select(p => p.Id);
                var notFoundIds = ids.Except(foundIds).ToList();
                throw new KeyNotFoundException($"Không tìm thấy bất động sản với ID: {string.Join(", ", notFoundIds)}");
            }

            if (props == null || !props.Any())
                return new List<ComparePropertyDTO>();

            // Tính các giá trị "best" trước khi map
            var validPriceProps = props.Where(p => p.Price > 0).ToList();
            var bestPrice = validPriceProps.Any() ? validPriceProps.Min(p => p.Price) : 0;
            var bestArea = props.Max(p => p.Area);
            var mostBedrooms = props.Max(p => p.Bedrooms);

            var propsWithReviews = props.Where(p => p.Reviews.Any()).ToList();
            var bestRating = propsWithReviews.Any()
                ? propsWithReviews.Max(p => p.Reviews.Average(r => r.Rating))
                : (double?)null;

            var mostViewed = props.Max(p => p.ViewsCount);
            var mostReviewed = props.Max(p => p.Reviews.Count);
            return props.Select(p => new ComparePropertyDTO
            {
                Id = p.Id,
                Title = p.Title,
                Type = p.Type,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                ViewsCount = p.ViewsCount,
                Price = p.Price,
                Area = p.Area,
                Bedrooms = p.Bedrooms,
                AddressId = p.AddressId,
                Location = p.Location,
                PrimaryImageUrl = p.Images.FirstOrDefault(i => i.IsPrimary)?.Url,
                Amenities = p.PropertyAmenities.Select(pa => pa.Amenity.Name).ToList(),
                LandlordName = p.Landlord.Name,
                LandlordPhoneNumber = p.Landlord.PhoneNumber,
                LandlordProfilePictureUrl = p.Landlord.ProfilePictureUrl,
                AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : null,
                TotalReviews = p.Reviews.Count,
                // Cái nào tốt nhất thì hiện là TRUE
                IsBestPrice = p.Price == bestPrice,
                IsBestArea = p.Area == bestArea,
                IsMostBedrooms = p.Bedrooms == mostBedrooms,
                IsBestRating = p.Reviews.Any() && Math.Abs(p.Reviews.Average(r => r.Rating) - bestRating.GetValueOrDefault()) < 0.0001,
                IsMostViewed = p.ViewsCount == mostViewed,
                IsMostReviewed = p.Reviews.Count == mostReviewed

            }).ToList();
        }




        public async Task<List<ProvinceDTO>> GetListLocationAsync()
        {
            var result = await _repository.GetListLocationAsync();

            return result;
        }

        public async Task<IEnumerable<HomePropertyDTO>> SearchAdvanceAsync(int? provinceId = 0, int? wardId = 0, int? streetId = 0, int? userId = 0)
        {
            var properties = await GetAllPropertiesAsync(userId);
            if (provinceId != 0) properties = properties.Where(c => c.ProvinceId == provinceId);
            if (wardId != 0) properties = properties.Where(c => c.WardId == wardId);
            if (streetId != 0) properties = properties.Where(c => c.StreetId == streetId);
            return properties.ToList();
        }

        public async Task<List<AmenityDTO>> GetListAmenityAsync()
        {
            var amenity = await _context.Amenities.ToListAsync();
            return amenity.Select(c => new AmenityDTO
            {
                Id = c.Id,
                Description = c.Description,
                Name = c.Name
            }).ToList();
        }

        public async Task<IEnumerable<Province>> GetProvincesAsync()
        {
            return await _context.Provinces.ToListAsync();
        }

        public async Task<IEnumerable<Street>> GetStreetAsync(int wardId)
        {
            return await _context.Streets.Where(w => w.WardId == wardId).ToListAsync();
        }

        public async Task<IEnumerable<Ward>> GetWardsAsync(int provinceid)
        {
            return await _context.Wards.Where(w => w.ProvinceId == provinceid).ToListAsync();
        }
        public async Task<IEnumerable<Amenity>> GetAmenitiesAsync()
        {
            return await _context.Amenities.ToListAsync();

        }


        public Task<bool> IndexPropertyAsync(PropertySearchDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task BulkIndexPropertiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HomePropertyDTO>> SearchAsync(string keyword)
        {
            throw new NotImplementedException();
        }
    }  }