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

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 