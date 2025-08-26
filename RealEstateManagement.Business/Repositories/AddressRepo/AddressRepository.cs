using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.Repositories.Repository;
using RealEstateManagement.Data.Data;
using RealEstateManagement.Data.Entity.AddressEnity;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.AddressRepo
{
    public class AddressRepository : RepositoryAsync<Address>, IAddressRepository
    {
        private readonly RentalDbContext _context;

        public AddressRepository(RentalDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Address> GetByDetailsAsync(int provinceId, int wardId, string street)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.ProvinceId == provinceId &&
                                          a.WardId == wardId &&
                                          a.DetailedAddress == street);
        }

        public async Task<Address> GetAddressAsync(int provinceId, int wardId, int streetId, string detailedAddress)
        {
            return await _context.Addresses
                .Include(a => a.Province)
                .Include(a => a.Ward)
                .Include(a => a.Street)
                .FirstOrDefaultAsync(a => a.ProvinceId == provinceId &&
                                          a.WardId == wardId &&
                                          a.StreetId == streetId &&
                                          a.DetailedAddress == detailedAddress);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 