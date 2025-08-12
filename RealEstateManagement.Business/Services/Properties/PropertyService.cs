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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<PaginatedResponseDTO<HomePropertyDTO>> GetPaginatedPropertiesAsync(int page = 1, int pageSize = 10, 
                int? userId = 0, string type = "room", string provinces = "", string wards = "", string streets = "",
                int minPrice = 0, int maxPrice = 100,
                int minArea = 0, int maxArea = 100, int minRoom = 0, int maxRoom = 15)
        {
            var properties = await _repository.GetAllAsync();
            var favoriteUsers = await _context.UserFavoriteProperties.Where(c => c.UserId == userId).ToListAsync();

            var result = new List<HomePropertyDTO>();
            //properties = properties.ToList();

            properties = properties.Where(c => c.Type == type);

            properties = properties.Where(p => p.Price >= minPrice * 1000000 && p.Price <= maxPrice * 1000000);

            properties = properties.Where(p => p.Area >= minArea && p.Area <= maxArea);

            properties = properties.Where(p => p.Bedrooms >= minRoom && p.Bedrooms <= maxRoom);

            if (!string.IsNullOrEmpty(provinces))
            {
                var Provinces = provinces.Split(',').Select(c => int.Parse(c)).ToList();
                properties = properties.Where(p => Provinces.Contains(p.Address.Province.Id));
            }

            if (!string.IsNullOrEmpty(wards))
            {
                var Wards = wards.Split(',').Select(c => int.Parse(c)).ToList();
                properties = properties.Where(p => Wards.Contains(p.Address.Ward.Id));
            }

            if (!string.IsNullOrEmpty(streets))
            {
                var Streets = streets.Split(',').Select(c => int.Parse(c)).ToList();
                properties = properties.Where(p => Streets.Contains(p.Address.Street.Id));
            }


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
                LandlordId = p.LandlordId,
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
        //Gợi ý bất động sản tương tự
        public async Task<IEnumerable<HomePropertyDTO>> SuggestSimilarPropertiesAsync(int propertyId, int take = 12, int? currentUserId = 0)
        {
            var pivot = await _repository.GetPropertyForSimilarityByIdAsync(propertyId);
            if (pivot == null) return Enumerable.Empty<HomePropertyDTO>();

            // 1) Ứng viên sơ bộ trên SQL (giảm N)
            var price = pivot.Price <= 0 ? (decimal?)null : pivot.Price;
            var area = pivot.Area <= 0 ? (decimal?)null : pivot.Area;

            var q = _repository.QueryApprovedForSimilarity()
                .Where(p => p.Id != pivot.Id && p.Type == pivot.Type);

            if (pivot.Address?.WardId is int wardId && wardId != 0)
                q = q.Where(p => p.Address.WardId == wardId || p.Address.ProvinceId == pivot.Address.ProvinceId);
            else if (pivot.Address?.ProvinceId is int provinceId && provinceId != 0)
                q = q.Where(p => p.Address.ProvinceId == provinceId);

            if (price.HasValue)
            {
                var minP = price.Value * 0.7m;
                var maxP = price.Value * 1.3m;
                q = q.Where(p => p.Price >= minP && p.Price <= maxP);
            }
            if (area.HasValue)
            {
                var minA = area.Value * 0.7m;
                var maxA = area.Value * 1.3m;
                q = q.Where(p => p.Area >= minA && p.Area <= maxA);
            }
            q = q.Where(p => Math.Abs(p.Bedrooms - pivot.Bedrooms) <= 2);

            var candidates = await q.ToListAsync();

            // 2) Chấm điểm tương đồng (0..1)
            var pivotAmenityIds = pivot.PropertyAmenities?.Select(pa => pa.Amenity.Id).ToHashSet() ?? new HashSet<int>();

            double W_TYPE = 0.30, W_LOC = 0.25, W_PRICE = 0.20, W_AREA = 0.10, W_BED = 0.05, W_AMEN = 0.10;

            double Score(Property c)
            {
                // typeScore
                double sType = string.Equals(c.Type, pivot.Type, StringComparison.OrdinalIgnoreCase) ? 1.0 : 0.0;

                // locationScore
                double sLoc = 0.0;
                if (pivot.Address?.WardId != null && c.Address?.WardId == pivot.Address.WardId) sLoc = 1.0;
                else if (pivot.Address?.ProvinceId != null && c.Address?.ProvinceId == pivot.Address.ProvinceId) sLoc = 0.6;

                // priceScore
                double sPrice = 0.5;
                if (pivot.Price > 0 && c.Price > 0)
                {
                    var diff = Math.Abs((double)(c.Price - pivot.Price)) / (double)pivot.Price; // tỉ lệ chênh
                    sPrice = Math.Max(0.0, 1.0 - Math.Min(diff, 1.0));
                }

                // areaScore
                double sArea = 0.5;
                if (pivot.Area > 0 && c.Area > 0)
                {
                    var diff = Math.Abs((double)(c.Area - pivot.Area)) / (double)Math.Max(1, pivot.Area);
                    sArea = Math.Max(0.0, 1.0 - Math.Min(diff, 1.0));
                }

                // bedroomsScore
                double sBed = 1.0 - Math.Min(Math.Abs(c.Bedrooms - pivot.Bedrooms) / 3.0, 1.0);

                // amenitiesScore (Jaccard)
                var cAmenityIds = c.PropertyAmenities?.Select(pa => pa.Amenity.Id).ToHashSet() ?? new HashSet<int>();
                double sAmen = 0.0;
                if (pivotAmenityIds.Count > 0 || cAmenityIds.Count > 0)
                {
                    var inter = pivotAmenityIds.Intersect(cAmenityIds).Count();
                    var union = pivotAmenityIds.Union(cAmenityIds).Count();
                    sAmen = union == 0 ? 0.0 : (double)inter / union;
                }

                return W_TYPE * sType + W_LOC * sLoc + W_PRICE * sPrice + W_AREA * sArea + W_BED * sBed + W_AMEN * sAmen;
            }

            var favorites = currentUserId > 0
                ? (await _context.UserFavoriteProperties
                        .Where(f => f.UserId == currentUserId).Select(f => f.PropertyId).ToListAsync())
                      .ToHashSet()
                : new HashSet<int>();

            var top = candidates
                .Select(c => new { P = c, Score = Score(c) })
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.P.PropertyPromotions.Any()
                    ? x.P.PropertyPromotions.Max(pp => pp.PromotionPackage.Level)
                    : 0)
                .ThenByDescending(x => x.P.CreatedAt)
                .Take(take)
                .Select(x => new HomePropertyDTO
                {
                    Id = x.P.Id,
                    Title = x.P.Title,
                    Description = x.P.Description,
                    Type = x.P.Type,
                    AddressID = x.P.AddressId,
                    StreetId = x.P.Address?.StreetId,
                    Street = x.P.Address?.Street?.Name,
                    ProvinceId = x.P.Address?.ProvinceId,
                    Province = x.P.Address?.Province?.Name,
                    WardId = x.P.Address?.WardId,
                    Ward = x.P.Address?.Ward?.Name,
                    DetailedAddress = x.P.Address?.DetailedAddress,
                    Area = x.P.Area,
                    Bedrooms = x.P.Bedrooms,
                    IsFavorite = favorites.Contains(x.P.Id),
                    Price = x.P.Price,
                    Status = x.P.Status,
                    Location = x.P.Location,
                    CreatedAt = x.P.CreatedAt,
                    ViewsCount = x.P.ViewsCount,
                    PrimaryImageUrl = x.P.Images?.FirstOrDefault(i => i.IsPrimary)?.Url,
                    LandlordId = x.P.Landlord?.Id ?? 0,
                    LandlordName = x.P.Landlord?.Name,
                    LandlordPhoneNumber = x.P.Landlord?.PhoneNumber,
                    LandlordProfilePictureUrl = x.P.Landlord?.ProfilePictureUrl,
                    Amenities = x.P.PropertyAmenities?.Select(pa => pa.Amenity.Name).ToList() ?? new List<string>(),
                    PromotionPackageName = x.P.PropertyPromotions?
                        .OrderByDescending(pp => pp.PromotionPackage.Level)
                        .Select(pp => pp.PromotionPackage.Name)
                        .FirstOrDefault()
                })
                .ToList();

            return top;
        }
        public async Task<List<WeeklyBestRatedPropertyDTO>> GetWeeklyBestRatedPropertiesAsync(
            int topN = 12,
            int minReviewsInWeek = 1,
            DateTime? fromUtc = null,
            DateTime? toUtc = null,
            int? currentUserId = 0)
        {
            if (topN <= 0) topN = 12;
            if (minReviewsInWeek < 0) minReviewsInWeek = 0;

            // Khoảng thời gian mặc định: 7 ngày gần nhất (rolling)
            var to = toUtc ?? DateTime.UtcNow;
            var from = fromUtc ?? to.AddDays(-7);

            // Lấy danh sách đã được tính Avg trong repo
            // Để đảm bảo sau khi lọc minReviews vẫn còn đủ topN, gọi repo với "biên dư"
            // (ví dụ gấp 3 lần). Tuỳ data thực tế bạn có thể chỉnh hệ số này.
            int fetchCount = Math.Max(topN * 3, topN);
            var raw = await _repository.GetWeeklyBestRatedPropertiesAsync(from, to, fetchCount);

            // Lọc theo số review tối thiểu trong tuần
            var filtered = raw
                .Where(x => x.WeeklyReviewCount >= minReviewsInWeek)
                .OrderByDescending(x => x.WeeklyAverageRating)
                .ThenByDescending(x => x.WeeklyReviewCount)
                .ThenByDescending(x => x.PromotionLevel)
                .ThenByDescending(x => x.PropertyCreatedAt)
                .ThenByDescending(x => x.ViewsCount)
                .Take(topN)
                .ToList();

            // Đánh dấu IsFavorite nếu có user
            if (currentUserId.GetValueOrDefault() > 0)
            {
                var uid = currentUserId!.Value;
                var favIds = await _context.UserFavoriteProperties
                    .Where(f => f.UserId == uid)
                    .Select(f => f.PropertyId)
                    .ToListAsync();

                var favSet = favIds.ToHashSet();
                foreach (var item in filtered)
                    item.IsFavorite = favSet.Contains(item.PropertyId);
            }

            return filtered;
        }

    }
}