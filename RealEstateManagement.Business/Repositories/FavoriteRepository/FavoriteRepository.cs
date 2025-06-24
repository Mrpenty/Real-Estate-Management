using Microsoft.EntityFrameworkCore;
using Nest;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.FavoriteRepository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly RentalDbContext _context;
        public FavoriteRepository(RentalDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddFavoritePropertyAsync(int userId, int propertyId)
        {

            // Kiểm tra User có tồn tại không
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Kiểm tra Property có tồn tại không
            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null) return false;

            // Kiểm tra đã tồn tại chưa
            var exists = await _context.UserFavoriteProperties
                .AnyAsync(fp => fp.UserId == userId && fp.PropertyId == propertyId);
            if (exists) return false;

            // Thêm mới
            var favorite = new UserFavoriteProperty
            {
                UserId = userId,
                PropertyId = propertyId,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserFavoriteProperties.Add(favorite);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveFavoritePropertyAsync(int userId, int propertyId)
        {
            var user = await (_context.Users.FindAsync(userId));
            if (user == null) return false;

            var property = await _context.Properties.FindAsync(propertyId);
            if (property == null) return false;

            var exists = await _context.UserFavoriteProperties
                .FirstOrDefaultAsync(ex => ex.UserId == userId && ex.PropertyId == propertyId);

            if (exists == null) return false;

            _context.UserFavoriteProperties.Remove(exists);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Property>> AllFavoritePropertyAsync(int userId)
        {
            var properties = await _context.UserFavoriteProperties
                .Where(pro => pro.UserId == userId)
                .Include(f => f.Property)
                    .ThenInclude(p => p.Images)
                .Include(f => f.Property)
                    .ThenInclude(p => p.Address)
                        .ThenInclude(c => c.Province)
                .Include(f => f.Property)
                    .ThenInclude(p => p.Address)
                        .ThenInclude(c => c.Ward)
                .Include(f => f.Property)
                    .ThenInclude(p => p.Address)
                        .ThenInclude(c => c.Street)
                .Include(f => f.Property)
                    .ThenInclude(p => p.Landlord)
                .Include(f => f.Property)
                    .ThenInclude(p => p.PropertyAmenities)
                        .ThenInclude(pa => pa.Amenity)
                .Include(f => f.Property)
                    .ThenInclude(p => p.PropertyPromotions)
                        .ThenInclude(pp => pp.PromotionPackage)
                .Select(f => f.Property)
                .ToListAsync();
            return properties;
        }
    }
}
