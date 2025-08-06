using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nest;
using RealEstateManagement.Business.DTO.Location;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Repositories.FavoriteRepository;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace RealEstateManagement.Business.Services.Properties
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _repository;
        private readonly IFavoriteRepository _repositoryFavorite;
        private readonly RentalDbContext _context;

        public PropertyService(IPropertyRepository repository, IFavoriteRepository repositoryFavorite, RentalDbContext context)
        {
            _repository = repository;
            _context = context;
            _repositoryFavorite = repositoryFavorite;
        }
        public async Task<IEnumerable<HomePropertyDTO>> GetAllPropertiesAsync(int? userId = 0)
        {
            var properties = await _repository.GetAllAsync();
            var favoriteUsers = await _context.UserFavoriteProperties.Where(c => c.UserId == userId).ToListAsync();

            var result = new List<HomePropertyDTO>();

            foreach (var p in properties)
            {
                try
                {
                    result.Add(new HomePropertyDTO
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        Type = p.Type,
                        AddressID = p.AddressId,
                        StreetId = p.Address?.StreetId,
                        Street = p.Address?.Street?.Name,
                        ProvinceId = p.Address?.ProvinceId,
                        Province = p.Address?.Province?.Name,
                        WardId = p.Address?.WardId,
                        Ward = p.Address?.Ward?.Name,
                        DetailedAddress = p.Address?.DetailedAddress,
                        Area = p.Area,
                        Bedrooms = p.Bedrooms,
                        IsFavorite = favoriteUsers.Any(c => c.PropertyId == p.Id),
                        Price = p.Price,
                        Status = p.Status,
                        Location = p.Location,
                        CreatedAt = p.CreatedAt,
                        ViewsCount = p.ViewsCount,
                        PrimaryImageUrl = p.Images?.FirstOrDefault(i => i.IsPrimary)?.Url,
                        LandlordId = p.Landlord?.Id ?? 0,
                        LandlordName = p.Landlord?.Name,
                        LandlordPhoneNumber = p.Landlord?.PhoneNumber,
                        LandlordProfilePictureUrl = p.Landlord?.ProfilePictureUrl,
                        PropertyPostId = p.Posts.FirstOrDefault(post => post.Status == PropertyPost.PropertyPostStatus.Approved)?.Id ?? 0,
                        Amenities = p.PropertyAmenities?.Select(pa => pa.Amenity.Name).ToList() ?? new List<string>(),
                        PromotionPackageName = p.PropertyPromotions?
                                                .OrderByDescending(pp => pp.PromotionPackage.Level)
                                                .Select(pp => pp.PromotionPackage.Name)
                                                .FirstOrDefault()
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi xử lý Property Id = {p.Id}: {ex.Message}");
                }
            }
            return result;

        }

        public async Task<PaginatedResponseDTO<HomePropertyDTO>> GetPaginatedPropertiesAsync(int page = 1, int pageSize = 10, int? userId = 0)
        {
            var properties = await _repository.GetAllAsync();
            var favoriteUsers = await _context.UserFavoriteProperties.Where(c => c.UserId == userId).ToListAsync();

            var result = new List<HomePropertyDTO>();

            foreach (var p in properties)
            {
                try
                {
                    result.Add(new HomePropertyDTO
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Description = p.Description,
                        Type = p.Type,
                        AddressID = p.AddressId,
                        StreetId = p.Address?.StreetId,
                        Street = p.Address?.Street?.Name,
                        ProvinceId = p.Address?.ProvinceId,
                        Province = p.Address?.Province?.Name,
                        WardId = p.Address?.WardId,
                        Ward = p.Address?.Ward?.Name,
                        DetailedAddress = p.Address?.DetailedAddress,
                        Area = p.Area,
                        Bedrooms = p.Bedrooms,
                        IsFavorite = favoriteUsers.Any(c => c.PropertyId == p.Id),
                        Price = p.Price,
                        Status = p.Status,
                        Location = p.Location,
                        CreatedAt = p.CreatedAt,
                        ViewsCount = p.ViewsCount,
                        PrimaryImageUrl = p.Images?.FirstOrDefault(i => i.IsPrimary)?.Url,
                        LandlordId = p.Landlord?.Id ?? 0,
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
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi xử lý Property Id = {p.Id}: {ex.Message}");
                }
            }

            var totalCount = result.Count;
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            
            // Đảm bảo page không vượt quá totalPages
            page = Math.Max(1, Math.Min(page, totalPages));
            
            var pagedData = result
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResponseDTO<HomePropertyDTO>
            {
                Data = pagedData,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasNextPage = page < totalPages,
                HasPreviousPage = page > 1
            };
        }
        public async Task<PropertyDetailDTO> GetPropertyByIdAsync(int id,int userId = 0)
        {
            var p = await _repository.GetPropertyByIdAsync(id);
            var rentalContract = p.Posts
                .FirstOrDefault(pp => pp.RentalContract != null)?.RentalContract;
            if (p == null)
            {
                throw new KeyNotFoundException($"Property with ID = {id} not found.");
            }

            var isFavorite = await _repositoryFavorite.GetFavoritePropertyByIdAsync(userId, id);
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
                IsFavorite = isFavorite == null ? false : true,
                PrimaryImageUrl = p.Images?.FirstOrDefault(i => i.IsPrimary)?.Url,
                ImageUrls = p.Images?.Select(c => c.Url).ToList(),
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
                StreetId = p.Address.StreetId,
                ProvinceId = p.Address.ProvinceId,
                WardId = p.Address.WardId

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
            var favoriteUsers = await _repositoryFavorite.AllFavoritePropertyAsync(filter.UserId);
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
                DetailedAddress = p.Address.DetailedAddress,
                IsFavorite = favoriteUsers.FirstOrDefault(c => c.Id == p.Id) != null ? true : false,
                Street = p.Address.Street.Name,
                ViewsCount = p.ViewsCount,
                PrimaryImageUrl = p.Images?.FirstOrDefault(i => i.IsPrimary)?.Url,
                LandlordId = p.Landlord?.Id ?? 0,
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
        public async Task<IEnumerable<HomePropertyDTO>> FilterByTypeAsync(string type)
        {
            var properties = await _repository.FilterByTypeAsync(type);
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
        public async Task<IEnumerable<HomePropertyDTO>> GetPropertiesByUserAsync(int? userId)
        {
            var properties = await _repository.GetAllAsync();
            properties = properties.Where(c => c.LandlordId == userId);
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
        public async Task<UserProfileWithPropertiesDTO?> GetUserProfileWithPropertiesAsync(int userId, int? currentId = null)
        {
            var user = await _repository.GetUserByIdAsync(userId);
            if (user == null) return null;
            var properties = await _repository.GetPropertiesByLandlordIdAsync(userId);
            var favoriteProperties = new List<Property>();
            if (currentId.HasValue && currentId.Value > 0)
            {
                favoriteProperties = (await _repositoryFavorite.AllFavoritePropertyAsync(currentId.Value)).ToList();
            }
            var propertyDtos = properties.Select(p => new HomePropertyDTO
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
                Location = p.Address?.Province?.Name + " - " + p.Address?.Ward?.Name + " - " + p.Address?.Street?.Name,
                DetailedAddress = p.Address?.DetailedAddress ?? "",
                PrimaryImageUrl = p.Images?.FirstOrDefault(i => i.IsPrimary)?.Url,
                CreatedAt = p.CreatedAt,
                ViewsCount = p.ViewsCount,
                LandlordId = p.LandlordId,
                LandlordName = user.Name,
                LandlordPhoneNumber = user.PhoneNumber,
                Amenities = p.PropertyAmenities?.Select(pa => pa.Amenity.Name).ToList() ?? new List<string>(),
                PromotionPackageName = p.PropertyPromotions?
                    .OrderByDescending(pp => pp.PromotionPackage.Level)
                    .Select(pp => pp.PromotionPackage.Name)
                    .FirstOrDefault(),
                ProvinceId = p.Address?.ProvinceId,
                Province = p.Address?.Province?.Name,
                WardId = p.Address?.WardId,
                Ward = p.Address?.Ward?.Name,
                StreetId = p.Address?.StreetId,
                Street = p.Address?.Street?.Name,
                IsFavorite = favoriteProperties.Any(fav => fav.Id == p.Id)
            }).ToList();

            return new UserProfileWithPropertiesDTO
            {
                UserId = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                AvatarUrl = user.ProfilePictureUrl,
                Properties = propertyDtos
            };
        }
    }
}