using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.FavoriteProperties
{
    public class UserPreferenceRepository : IUserPreferenceRepository
    {
        private readonly RentalDbContext _context;

        public UserPreferenceRepository(RentalDbContext context)
        {
            _context = context;
        }

        //public async Task<bool> AddFavoritePropertyAsync(int userId, int propertyId)
        //{
        //    // Tìm hoặc tạo mới UserPreference cho user
        //    var userPreference = await _context.UserPreferences
        //        .Include(up => up.FavoriteProperties)
        //        .FirstOrDefaultAsync(up => up.UserId == userId);


        //    // Kiểm tra nếu property đã tồn tại trong danh sách yêu thích
        //    var alreadyExists = _context.UserPreferenceFavoriteProperties.Any(fp => fp.PropertyId == propertyId && fp.UserPreferenceId == userId);

        //    if (alreadyExists)
        //        return false;

        //    // Thêm mới
        //    userPreference.FavoriteProperties.Add(new UserPreferenceFavoriteProperties
        //    {
        //        UserPreferenceId = userId,
        //        PropertyId = propertyId,
        //        CreatedAt = DateTime.UtcNow
        //    });

        //    await _context.SaveChangesAsync();
        //    return true;
        //}
    }
}
