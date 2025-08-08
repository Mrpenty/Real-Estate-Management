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
                    EmailConfirmed = true,
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
                    UserName = "MinhTri",
                    NormalizedUserName = "MINHTRI",
                    Email = "MinhTri@example.com",
                    PhoneNumber = "+842345678910",
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = "TRI@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "Landlord@123"),
                    Name = "Minh Trisgei",
                    Role = "landlord",
                    ProfilePictureUrl = "https://th.bing.com/th/id/R.63d31ac6257157ef079f31bb32e342df?rik=63%2bkafQNo5seHg&pid=ImgRaw&r=0",
                    IsVerified = true,
                    CreatedAt = DateTime.Now,
                    CitizenIdNumber = "002345678901",
                    CitizenIdIssuedDate = new DateTime(2021, 2, 2),
                    CitizenIdExpiryDate = new DateTime(2031, 2, 2),
                    CitizenIdFrontImageUrl = "https://example.com/cccd/landlord_front.jpg",
                    CitizenIdBackImageUrl = "https://example.com/cccd/landlord_back.jpg",
                    IsActive = true
                },
                new ApplicationUser
                {
                    Id = 3,
                    UserName = "Khanh",
                    NormalizedUserName = "KHANH",
                    Email = "Khanh@example.com",
                    PhoneNumber = "+843345678910",
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = "KHANH@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Renter@123"),
                    Name = "Khanh",
                    Role = "renter",
                    IsVerified = true,
                    CreatedAt = DateTime.Now,
                    CitizenIdNumber = "003456789012",
                    CitizenIdIssuedDate = new DateTime(2022, 3, 3),
                    CitizenIdExpiryDate = new DateTime(2032, 3, 3),
                    CitizenIdFrontImageUrl = "https://example.com/cccd/renter_front.jpg",
                    CitizenIdBackImageUrl = "https://example.com/cccd/renter_back.jpg",
                    IsActive = true
                },
                new ApplicationUser
                {
                    Id = 4,
                    UserName = "Duongkhmt",
                    NormalizedUserName = "DUONGKHMT",
                    Email = "renter2@example.com",
                    PhoneNumber = "+846574837475",
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = "RENTER2@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Renter2@123"),
                    Name = "Duongkhmt",
                    Role = "renter",
                    IsVerified = true,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    CitizenIdNumber = "004567890123",
                    CitizenIdIssuedDate = new DateTime(2023, 4, 4),
                    CitizenIdExpiryDate = new DateTime(2033, 4, 4),
                    CitizenIdFrontImageUrl = "https://example.com/cccd/renter2_front.jpg",
                    CitizenIdBackImageUrl = "https://example.com/cccd/renter2_back.jpg",
                    VerificationRejectReason = "Ảnh CCCD mặt sau bị mờ, thiếu ngày cấp. Vui lòng bổ sung lại!",
                    IsActive = false
                }, new ApplicationUser
                {
                    Id = 5,
                    UserName = "Manh",
                    NormalizedUserName = "MANH",
                    Email = "Manh@example.com",
                    PhoneNumber = "+840987654567",
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = "MANH@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Landlord1@123"),
                    Name = "Manh home",
                    Role = "landlord",
                    ProfilePictureUrl = "https://th.bing.com/th/id/R.63d31ac6257157ef079f31bb32e342df?rik=63%2bkafQNo5seHg&pid=ImgRaw&r=0",
                    IsVerified = true,
                    CreatedAt = DateTime.Now,
                    CitizenIdNumber = "002345678901",
                    CitizenIdIssuedDate = new DateTime(2021, 2, 2),
                    CitizenIdExpiryDate = new DateTime(2031, 2, 2),
                    CitizenIdFrontImageUrl = "https://example.com/cccd/landlord_front.jpg",
                    CitizenIdBackImageUrl = "https://example.com/cccd/landlord_back.jpg",
                    IsActive = true
                },
                new ApplicationUser
                {
                    Id = 6,
                    UserName = "DongVT",
                    NormalizedUserName = "DONGVT",
                    Email = "Tadong@example.com",
                    PhoneNumber = "+843541234567",
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = "TADONG@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Landlord2@123"),
                    Name = "DongAUTO",
                    Role = "landlord",
                    ProfilePictureUrl = "https://th.bing.com/th/id/R.63d31ac6257157ef079f31bb32e342df?rik=63%2bkafQNo5seHg&pid=ImgRaw&r=0",
                    IsVerified = true,
                    CreatedAt = DateTime.Now,
                    CitizenIdNumber = "002345678901",
                    CitizenIdIssuedDate = new DateTime(2021, 2, 2),
                    CitizenIdExpiryDate = new DateTime(2031, 2, 2),
                    CitizenIdFrontImageUrl = "https://example.com/cccd/landlord_front.jpg",
                    CitizenIdBackImageUrl = "https://example.com/cccd/landlord_back.jpg",
                    IsActive = true
                }

            );
        }
    }
}