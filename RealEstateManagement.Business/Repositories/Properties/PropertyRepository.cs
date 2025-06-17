using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Properties
{
    public class PropertyRepository: IPropertyRepository
    {
        private readonly RentalDbContext _context;

        public PropertyRepository(RentalDbContext context)
        {
            _context = context;
        }
        //Lấy tất cả property
        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.PropertyPromotions)
                    .ThenInclude(pp => pp.PromotionPackage)
                .Select(p => new
                {
                    Property = p,
                    // Nếu có bất kỳ gói khuyến mãi nào, lấy mức cao nhất; nếu không thì bằng 0.
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
        //Lấy property theo Id
        public async Task<Property> GetPropertyByIdAsync(int id)
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        //Sắp xếp theo giá
        public async Task<IEnumerable<Property>> FilterByPriceAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.PropertyPromotions)
                    .ThenInclude(pp => pp.PromotionPackage)
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
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
                .Select(x => x.Property) // Trả lại Property sau khi sort
                .ToListAsync();
        }
        //Sắp xếp theo diện tích
        public async Task<IEnumerable<Property>> FilterByAreaAsync(decimal minArea, decimal maxArea)
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.PropertyPromotions)
                    .ThenInclude(pp => pp.PromotionPackage)
                .Where(p => p.Area >= minArea && p.Price <= maxArea)
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
                .Select(x => x.Property) // Trả lại Property sau khi sort
                .ToListAsync();
        }
        //Sắp xếp nâng cao
        public async Task<IEnumerable<Property>> FilterAdvancedAsync(PropertyFilterDTO filter)
        {
            var query = _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Landlord)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.PropertyPromotions)
                    .ThenInclude(pp => pp.PromotionPackage)
                .AsQueryable();

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            if (filter.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            if (filter.MinBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms >= filter.MinBedrooms.Value);
            if (filter.MaxBedrooms.HasValue)
                query = query.Where(p => p.Bedrooms <= filter.MaxBedrooms.Value);
            if (filter.MinArea.HasValue)
                query = query.Where(p => p.Area >= filter.MinArea.Value);
            if (filter.MaxArea.HasValue)
                query = query.Where(p => p.Area <= filter.MaxArea.Value);
            if (!string.IsNullOrEmpty(filter.Location))
                query = query.Where(p => p.Location.Contains(filter.Location));
            if (!string.IsNullOrEmpty(filter.PropertyType))
                query = query.Where(p => p.Type == filter.PropertyType);
            if (filter.IsVerified.HasValue)
                query = query.Where(p => p.IsVerified == filter.IsVerified.Value);
            if (!string.IsNullOrEmpty(filter.SearchKeyword))
                query = query.Where(p => p.Title.Contains(filter.SearchKeyword));

            if (filter.AmenityIds != null && filter.AmenityIds.Count > 0)
            {
                query = query.Where(p =>
                    p.PropertyAmenities.Any(pa => filter.AmenityIds.Contains(pa.AmenityId)));
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
                .ToListAsync();
        }
    }
}
