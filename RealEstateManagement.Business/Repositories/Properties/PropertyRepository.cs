using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using RealEstateManagement.Business.DTO.Location;
using RealEstateManagement.Data.Entity.AddressEnity;

namespace RealEstateManagement.Business.Repositories.Properties
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly RentalDbContext _context;

        public PropertyRepository(RentalDbContext context)
        {
            _context = context;
        }
        //Lấy tất cả property
        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            var properties = await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.Address).ThenInclude(a => a.Province)
                .Include(p => p.Address).ThenInclude(a => a.Ward)
                .Include(p => p.Address).ThenInclude(a => a.Street)
                .Include(p => p.Posts)
                .Include(p => p.Reviews)
                .Where(p => p.Posts.Any(post => post.Status == PropertyPost.PropertyPostStatus.Approved))
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.PropertyPromotions).ThenInclude(pp => pp.PromotionPackage)
                .AsNoTracking()
                .ToListAsync();

            return properties
                .OrderByDescending(p => p.PropertyPromotions.Any()
                    ? p.PropertyPromotions.Max(pp => pp.PromotionPackage.Level)
                    : 0)
                .ThenByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.ViewsCount);

        }
        //Lấy property theo Id
        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.PropertyPromotions)
                    .ThenInclude(pp => pp.PromotionPackage)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Province)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Ward)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Street)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p=>p.Posts)
                    .ThenInclude(Pa=>Pa.RentalContract)
                 .Include(p=>p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        //Sắp xếp theo giá
        public async Task<IEnumerable<Property>> FilterByPriceAsync(decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.Posts)
                .Where(p => p.Posts.Any(post => post.Status == PropertyPost.PropertyPostStatus.Approved))
                .Include(p => p.PropertyPromotions).ThenInclude(pp => pp.PromotionPackage)
                .AsQueryable();

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            return await query
                .Select(p => new
                {
                    Property = p,
                    PromotionLevel = p.PropertyPromotions.Any()
                        ? p.PropertyPromotions.Max(pp => pp.PromotionPackage.Level)
                        : 0
                })
                .OrderByDescending(x => x.PromotionLevel)
                .ThenByDescending(x => x.Property.CreatedAt)
                .ThenByDescending(x => x.Property.ViewsCount)
                .Select(x => x.Property)
                .ToListAsync();
        }
        //Sắp xếp theo diện tích
        public async Task<IEnumerable<Property>> FilterByAreaAsync(decimal? minArea, decimal? maxArea)
        {
            var query = _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.Posts)
                .Where(p => p.Posts.Any(post => post.Status == PropertyPost.PropertyPostStatus.Approved))
                .Include(p => p.PropertyPromotions).ThenInclude(pp => pp.PromotionPackage)
                .AsQueryable();

            if (minArea.HasValue)
                query = query.Where(p => p.Area >= minArea.Value);
            if (maxArea.HasValue)
                query = query.Where(p => p.Area <= maxArea.Value);

            return await query
                .Select(p => new
                {
                    Property = p,
                    PromotionLevel = p.PropertyPromotions.Any()
                        ? p.PropertyPromotions.Max(pp => pp.PromotionPackage.Level)
                        : 0
                })
                .OrderByDescending(x => x.PromotionLevel)
                .ThenByDescending(x => x.Property.CreatedAt)
                .ThenByDescending(x => x.Property.ViewsCount)
                .Select(x => x.Property)
                .ToListAsync();
        }

        //Sắp xếp nâng cao
        public async Task<IEnumerable<Property>> FilterAdvancedAsync(PropertyFilterDTO filter)
        {
            var query = _context.Properties
                .Include(p=>p.Landlord)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Province)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Ward)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Street)
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.Posts)
                .Where(p => p.Posts.Any(post => post.Status == PropertyPost.PropertyPostStatus.Approved))
                .Include(p => p.PropertyPromotions).ThenInclude(pp => pp.PromotionPackage)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Type))
            {
                query = query.Where(p => p.PropertyType.Name == filter.Type);
            }

            if (filter.MinPrice.HasValue)
            {
                filter.MinPrice = filter.MinPrice * 1000000;
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }
                
            if (filter.MaxPrice.HasValue)
            {
                filter.MaxPrice = filter.MaxPrice * 1000000;
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }
                
            if (filter.MinBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms >= filter.MinBedrooms.Value);
            if (filter.MaxBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms <= filter.MaxBedrooms.Value);

            if (filter.MinArea.HasValue)
            {
                query = query.Where(p => p.Area >= filter.MinArea.Value);
            }
                
            if (filter.MaxArea.HasValue)
            {
                query = query.Where(p => p.Area <= filter.MaxArea.Value);
            }
                

            if (filter.AmenityName != null
               && filter.AmenityName.Any(name => !string.IsNullOrWhiteSpace(name) && name.ToLower() != "string"))
            {
                var validNames = filter.AmenityName
                    .Where(name => !string.IsNullOrWhiteSpace(name) && name.ToLower() != "string")
                    .Select(name => name.ToLower())
                    .ToList();

                query = query.Where(p =>
                    p.PropertyAmenities.Any(pa => validNames.Contains(pa.Amenity.Name.ToLower())));
            }


            if (filter.Provinces.Count != 0)
            {
                query = query.Where(p => filter.Provinces.Contains(p.Address.Province.Id));
            }

            if (filter.Wards.Count != 0)
            {
                query = query.Where(p => filter.Wards.Contains(p.Address.Ward.Id));
            }

            if (filter.Streets.Count != 0)
            {
                query = query.Where(p => filter.Streets.Contains(p.Address.Street.Id));
            }

            return await query
                .Select(p => new
                {
                    Property = p,
                    PromotionLevel = p.PropertyPromotions.Any()
                        ? p.PropertyPromotions.Max(pp => pp.PromotionPackage.Level)
                        : 0
                })
                .OrderByDescending(x => x.PromotionLevel)
                .ThenByDescending(x => x.Property.CreatedAt)
                .ThenByDescending(x => x.Property.ViewsCount)
                .Select(x => x.Property)
                .ToListAsync();
        }


        //So sánh property
        public async Task<IEnumerable<Property>> ComparePropertiesAsync(List<int> ids)
        {
            return await _context.Properties
                .Where(p => ids.Contains(p.Id))
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.Posts)
                .Where(p => p.Posts.Any(post => post.Status == PropertyPost.PropertyPostStatus.Approved))
                .ToListAsync();
        }

        //Lấy nhiều property để so sánh
        public async Task<List<Property>> GetPropertiesByIdsAsync(List<int> ids)
        {
            return await _context.Properties
                .Where(p => ids.Contains(p.Id))
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.Reviews)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.Posts)
                .Where(p => p.Posts.Any(post => post.Status == PropertyPost.PropertyPostStatus.Approved))
                .ToListAsync();
        }


        public async Task<List<ProvinceDTO>> GetListLocationAsync()
        {
            var streets = await _context.Streets.ToListAsync();
            var wards = await _context.Wards.ToListAsync();
            var provinces = await _context.Provinces.ToListAsync();

            provinces = provinces.OrderBy(p => p.Id).ToList();
            wards = wards.OrderBy(p => p.Id).ToList();
            streets = streets.OrderBy(p => p.Id).ToList();

            var result = provinces.Select(c => new ProvinceDTO
            {
                Id = c.Id,
                Name = c.Name,
                Wards = wards.Where(w => w.ProvinceId == c.Id || w.Id == 0).Select(w => new WardDTO
                {
                    Id = w.Id,
                    Name = w.Name,
                    Streets = streets.Where(s => s.WardId == w.Id || s.Id == 0).Select(s => new StreetDTO
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList()
                }).ToList(),
            }).ToList();
            return result;
        }

        public async Task<IEnumerable<Property>> FilterByTypeAsync(string type)
        {
            var query = _context.Properties
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Province)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Ward)
                .Include(p => p.Address)
                    .ThenInclude(pa => pa.Street)
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.Posts)
                .Where(p => p.Posts.Any(post => post.Status == PropertyPost.PropertyPostStatus.Approved))
                .Include(p => p.PropertyPromotions).ThenInclude(pp => pp.PromotionPackage)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(p => p.PropertyType.Name == type);
            }
            return await query
                .Select(p => new
                {
                    Property = p,
                    PromotionLevel = p.PropertyPromotions.Any()
                        ? p.PropertyPromotions.Max(pp => pp.PromotionPackage.Level)
                        : 0
                })
                .OrderByDescending(x => x.PromotionLevel)
                .ThenByDescending(x => x.Property.CreatedAt)
                .ThenByDescending(x => x.Property.ViewsCount)
                .Select(x => x.Property)
                .ToListAsync();
        }
        // Lấy thông tin người dùng
        public async Task<ApplicationUser?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
        // Lấy property theo landlordId
        public async Task<List<Property>> GetPropertiesByLandlordIdAsync(int landlordId)
        {
            return await _context.Properties
                .Where(p => p.LandlordId == landlordId)
                .Include(p => p.Images)
                .Include(p => p.Address).ThenInclude(a => a.Province)
                .Include(p => p.Address).ThenInclude(a => a.Ward)
                .Include(p => p.Address).ThenInclude(a => a.Street)
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.Posts)
                .Where(p => p.Posts.Any(post => post.Status == PropertyPost.PropertyPostStatus.Approved))
                .Include(p => p.PropertyPromotions).ThenInclude(pp => pp.PromotionPackage)
                .OrderByDescending(p => p.PropertyPromotions.Any()
                    ? p.PropertyPromotions.Max(pp => pp.PromotionPackage.Level)
                    : 0)
                .ThenByDescending(p => p.CreatedAt)
                .ThenByDescending(p => p.ViewsCount)
                .AsNoTracking()
                .ToListAsync();
        }
        //Gợi ý bđs tương tự
        public IQueryable<Property> QueryApprovedForSimilarity()
        {
            return _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.Address).ThenInclude(a => a.Province)
                .Include(p => p.Address).ThenInclude(a => a.Ward)
                .Include(p => p.Address).ThenInclude(a => a.Street)
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.Posts)
                .Where(p => p.Posts.Any(post => post.Status == PropertyPost.PropertyPostStatus.Approved))
                .AsNoTracking();
        }

        public Task<Property?> GetPropertyForSimilarityByIdAsync(int id)
        {
            return QueryApprovedForSimilarity()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<WeeklyBestRatedPropertyDTO>> GetWeeklyBestRatedPropertiesAsync(
            DateTime fromUtc, DateTime toUtc, int topN)
        {
            return await _context.Reviews
                .Where(r => r.CreatedAt >= fromUtc && r.CreatedAt <= toUtc && r.IsVisible && !r.IsFlagged)
                .GroupBy(r => r.PropertyId)
                .Select(g => new
                {
                    PropertyId = g.Key,
                    WeeklyAverageRating = g.Average(r => r.Rating),
                    WeeklyReviewCount = g.Count()
                })
                .Join(
                    _context.Properties
                        .Include(p => p.Images)
                        .Include(p => p.Address).ThenInclude(a => a.Province)
                        .Include(p => p.Address).ThenInclude(a => a.Ward)
                        .Include(p => p.Address).ThenInclude(a => a.Street)
                        .Include(p => p.PropertyPromotions).ThenInclude(pp => pp.PromotionPackage),
                    g => g.PropertyId,
                    p => p.Id,
                    (g, p) => new WeeklyBestRatedPropertyDTO
                    {
                        PropertyId = p.Id,
                        Title = p.Title,
                        Type = p.PropertyType.Name,
                        Price = p.Price,
                        Area = p.Area,
                        Bedrooms = p.Bedrooms,

                        PrimaryImageUrl =
                            p.Images.Where(img => img.IsPrimary)
                                    .OrderBy(img => img.Order)
                                    .Select(img => img.Url)
                                    .FirstOrDefault()
                            ?? p.Images.OrderBy(img => img.Order)
                                       .Select(img => img.Url)
                                       .FirstOrDefault(),

                        // Tránh ?. bằng kiểm tra null
                        Province = (p.Address != null && p.Address.Province != null)
                                    ? p.Address.Province.Name
                                    : null,
                        Ward = (p.Address != null && p.Address.Ward != null)
                                    ? p.Address.Ward.Name
                                    : null,
                        Street = (p.Address != null && p.Address.Street != null)
                                    ? p.Address.Street.Name
                                    : null,

                        IsFavorite = false, // set ở Service nếu cần

                        WeeklyAverageRating = g.WeeklyAverageRating,
                        WeeklyReviewCount = g.WeeklyReviewCount,

                        PromotionLevel = p.PropertyPromotions
                            .Where(pp => pp.EndDate >= DateTime.UtcNow && pp.PromotionPackage != null)
                            .OrderByDescending(pp => pp.PromotionPackage.Level)
                            .Select(pp => pp.PromotionPackage.Level)
                            .FirstOrDefault(),

                        PropertyCreatedAt = p.CreatedAt,
                        ViewsCount = p.ViewsCount
                    }
                )
                .OrderByDescending(x => x.WeeklyAverageRating)
                .ThenByDescending(x => x.WeeklyReviewCount)
                .ThenByDescending(x => x.PromotionLevel)
                .Take(topN)
                .ToListAsync();
        }

        // Thêm phương thức lấy danh sách bất động sản mà người dùng đang thuê
        public async Task<List<Property>> GetPropertiesRentedByUserAsync(int userId)
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.Address).ThenInclude(a => a.Province)
                .Include(p => p.Address).ThenInclude(a => a.Ward)
                .Include(p => p.Address).ThenInclude(a => a.Street)
                .Include(p => p.PropertyAmenities).ThenInclude(pa => pa.Amenity)
                .Include(p => p.Posts).ThenInclude(post => post.RentalContract)
                .Where(p => p.Posts.Any(post =>
                    post.Status == PropertyPost.PropertyPostStatus.Rented &&
                    post.RentalContract != null &&
                    post.RentalContract.RenterId == userId))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
