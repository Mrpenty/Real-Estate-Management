using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace RealEstateManagement.Business.Repositories.TenantInteraction
{
    public class InteractionRepository: IInteractionRepository
    {
        private readonly RentalDbContext _context;
        public InteractionRepository(RentalDbContext context)
        {
            _context = context;
        }
        public async Task<ApplicationUser> GetProfileLandlordByIdAsync(int landlordId)
        {
            var landlord =await _context.Users
                .Where(u => u.Id == landlordId)
                .Include(u=>u.Properties).ThenInclude(p => p.Address).ThenInclude(a => a.Street)
                .Include(u => u.Properties).ThenInclude(p => p.Address).ThenInclude(a => a.Province)
                .Include(u => u.Properties).ThenInclude(p => p.Address).ThenInclude(a => a.Ward)
                .FirstOrDefaultAsync();
            return landlord;    
        }
    }
}
