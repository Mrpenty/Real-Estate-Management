using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RealEstateManagement.Data.Data.DataSeed.User
{
    public static class UserRoleSeed
    {
        public static void SeedUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 }, // Admin
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 }, // Landlord
                new IdentityUserRole<int> { UserId = 3, RoleId = 3 }, // Renter
                new IdentityUserRole<int> { UserId = 4, RoleId = 3 }  // Renter 2
            );
        }
    }
}