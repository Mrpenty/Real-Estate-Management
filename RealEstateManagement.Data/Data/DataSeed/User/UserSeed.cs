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
                    CreatedAt = DateTime.Now,
                    CitizenIdNumber = "01234567890",
                    CitizenIdIssuedDate = new DateTime(2020, 1, 1),
                    CitizenIdExpiryDate = new DateTime(2030, 1, 1),
                    CitizenIdFrontImageUrl = "https://example.com/cccd/admin_front.jpg",
                    CitizenIdBackImageUrl = "https://example.com/cccd/admin_back.jpg",
                    IsActive = true
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
                    CreatedAt = DateTime.Now,
                    CitizenIdNumber = "002345678901",
                    CitizenIdImageUrl = "https://example.com/cccd/landlord.jpg",
                    CitizenIdIssuedDate = new DateTime(2021, 2, 2),
                    CitizenIdExpiryDate = new DateTime(2031, 2, 2),
                    CitizenIdFrontImageUrl = "https://example.com/cccd/landlord_front.jpg",
                    CitizenIdBackImageUrl = "https://example.com/cccd/landlord_back.jpg",
                    IsActive = true
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
                    CreatedAt = DateTime.Now,
                    CitizenIdNumber = "003456789012",
                    CitizenIdImageUrl = "https://example.com/cccd/renter.jpg",
                    CitizenIdIssuedDate = new DateTime(2022, 3, 3),
                    CitizenIdExpiryDate = new DateTime(2032, 3, 3),
                    CitizenIdFrontImageUrl = "https://example.com/cccd/renter_front.jpg",
                    CitizenIdBackImageUrl = "https://example.com/cccd/renter_back.jpg",
                    IsActive = true
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
                    CreatedAt = DateTime.Now.AddDays(-1),
                    CitizenIdNumber = "004567890123",
                    CitizenIdImageUrl = "https://example.com/cccd/renter2.jpg",
                    CitizenIdIssuedDate = new DateTime(2023, 4, 4),
                    CitizenIdExpiryDate = new DateTime(2033, 4, 4),
                    CitizenIdFrontImageUrl = "https://example.com/cccd/renter2_front.jpg",
                    CitizenIdBackImageUrl = "https://example.com/cccd/renter2_back.jpg",
                    VerificationRejectReason = "Ảnh CCCD mặt sau bị mờ, thiếu ngày cấp. Vui lòng bổ sung lại!",
                    IsActive = false
                }
            );
        }
    }
}