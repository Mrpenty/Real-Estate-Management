using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.User;
using System;

namespace RealEstateManagement.Data.Data.DataSeed.User
{
    public static class UserSeed
    {
        public static void SeedUsers(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = 1,
                    UserName = "admin@example.com",
                    NormalizedUserName = "ADMIN@EXAMPLE.COM",
                    Email = "admin@example.com",
                    PhoneNumber = "+841234567891",
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Admin@123"),
                    Name = "Admin User",
                    Role = "admin",
                    IsVerified = true,
                    CreatedAt = DateTime.Now
                },
                new ApplicationUser
                {
                    Id = 2,
                    UserName = "landlord@example.com",
                    NormalizedUserName = "LANDLORD@EXAMPLE.COM",
                    Email = "landlord@example.com",
                    PhoneNumber = "+842345678910",
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = "LANDLORD@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Landlord@123"),
                    Name = "Landlord User",
                    Role = "landlord",
                    ProfilePictureUrl = "https://th.bing.com/th/id/R.63d31ac6257157ef079f31bb32e342df?rik=63%2bkafQNo5seHg&pid=ImgRaw&r=0",
                    IsVerified = true,
                    CreatedAt = DateTime.Now

                },
                new ApplicationUser
                {
                    Id = 3,
                    UserName = "renter@example.com",
                    NormalizedUserName = "RENTER@EXAMPLE.COM",
                    Email = "renter@example.com",
                    PhoneNumber = "+843345678910",
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = "RENTER@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Renter@123"),
                    Name = "Renter User",
                    Role = "renter",
                    IsVerified = true,
                    CreatedAt = DateTime.Now
                },
                new ApplicationUser
                {
                    Id = 4,
                    UserName = "renter2@example.com",
                    NormalizedUserName = "RENTER2@EXAMPLE.COM",
                    Email = "renter2@example.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = "RENTER2@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Renter2@123"),
                    Name = "Renter User 2",
                    Role = "renter",
                    IsVerified = true,
                    CreatedAt = DateTime.Now.AddDays(-1)
                }
            );
        }
    }
}