using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.Repositories.Repository;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.AmenityRepo
{
    public class AmenityRepository : RepositoryAsync<Amenity>, IAmenityRepository
    {
        private readonly RentalDbContext _context;

        public AmenityRepository(RentalDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Amenity>> GetPagedAsync(int skip, int take)
        {
            return await _context.Set<Amenity>()
                .OrderBy(a => a.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Set<Amenity>().CountAsync();
        }
    }
}
