using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Properties
{
    public class InterestedPropertyRepository : IInterestedPropertyRepository
    {
        private readonly RentalDbContext _context;
        public InterestedPropertyRepository(RentalDbContext context) => _context = context;

        public async Task<InterestedProperty> AddAsync(InterestedProperty entity)
        {
            _context.InterestedProperties.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<InterestedProperty> GetByIdAsync(int id)
            => await _context.InterestedProperties
                .Include(x => x.Property)
                .Include(x => x.Renter)
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<InterestedProperty> GetByRenterAndPropertyAsync(int renterId, int propertyId)
            => await _context.InterestedProperties
                .FirstOrDefaultAsync(x => x.RenterId == renterId && x.PropertyId == propertyId);

        public async Task<IEnumerable<InterestedProperty>> GetByRenterAsync(int renterId)
            => await _context.InterestedProperties
                .Include(x => x.Property)
                .Where(x => x.RenterId == renterId)
                .ToListAsync();

        public async Task<IEnumerable<InterestedProperty>> GetByPropertyAsync(int propertyId)
            => await _context.InterestedProperties
                .Include(x => x.Renter)
                .Where(x => x.PropertyId == propertyId)
                .ToListAsync();

        public async Task UpdateAsync(InterestedProperty entity)
        {
            _context.InterestedProperties.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(InterestedProperty entity)
        {
            _context.InterestedProperties.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // Chuẩn OData + phân trang LINQ
        public IQueryable<InterestedProperty> Query()
            => _context.InterestedProperties.AsNoTracking();
    }
}
