using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RealEstateManagement.Data.Data.DataSeed.User
{
    public static class RoleSeed
    {
        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "Landlord", NormalizedName = "LANDLORD" },
                new IdentityRole<int> { Id = 3, Name = "Renter", NormalizedName = "RENTER" }
            );
        }
    }
}