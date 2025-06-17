using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.OwnerRepo
{
    public class PropertyImageRepository : IPropertyImageRepository
    {
        private readonly RentalDbContext _context;

        public PropertyImageRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<PropertyImage> AddImageAsync(PropertyImage image)
        {
            _context.PropertyImages.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<bool> PropertyExistsAsync(int propertyId)
        {
            return await _context.Properties.AnyAsync(p => p.Id == propertyId);
        }
    }

}
