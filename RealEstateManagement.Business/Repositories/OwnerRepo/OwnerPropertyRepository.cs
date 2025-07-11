using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Business.Repositories.impl;
using RealEstateManagement.Business.Repositories.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.OwnerRepo
{
    public class OwnerPropertyRepository : RepositoryAsync<Property>, IOwnerPropertyRepository
    {
        private readonly RentalDbContext _context;

        public OwnerPropertyRepository(RentalDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Property> GetByLandlordQueryable(int landlordId)
        {
            return _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Address)
                .ThenInclude(p => p.Province)
                .Include(p => p.Address)
                .ThenInclude(p => p.Street)
                .Include(p => p.Address)
                .ThenInclude(p => p.Ward)
                .Where(p => p.LandlordId == landlordId);
        }

        public async Task<Property> GetByIdAsync(int id, int landlordId)
        {
            return await _context.Properties
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id && p.LandlordId == landlordId);
        }
        public async Task DeleteAsync(Property property)
        {
            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
        }


    }

}
