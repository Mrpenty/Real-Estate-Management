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
                .OrderByDescending(p => p.CreatedAt)
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
                .Where(p=>p.Price >= minPrice && p.Price <= maxPrice) // Dòng này sắp xếp theo giá
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
        //Sắp xếp theo diện tích
        public async Task<IEnumerable<Property>> FilterByAreaAsync(decimal minArea, decimal maxArea)
        {
            return await _context.Properties
                .Include(p => p.Images)
                .Include (p => p.Landlord)
                .Include(p=>p.PropertyAmenities)
                    .ThenInclude(pa=>pa.Amenity)
                .Where(p=>p.Area >= minArea && p.Area <=maxArea)
                .OrderByDescending(p => p.CreatedAt)
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
                //.OrderByDescending(p => p.CreatedAt)
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
