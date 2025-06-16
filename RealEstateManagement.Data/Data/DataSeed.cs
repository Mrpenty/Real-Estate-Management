using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Data
{
    public class DataSeed
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            SeedRoles(modelBuilder);
            SeedUsers(modelBuilder);
            SeedUserRoles(modelBuilder);
            //  SeedUserPreferences(modelBuilder);
            SeedAmenities(modelBuilder);
            SeedProperties(modelBuilder);
            SeedPropertyAmenities(modelBuilder);
            SeedPropertyImages(modelBuilder);
            SeedPropertyPosts(modelBuilder);

            SeedPayments(modelBuilder);
            SeedTransactions(modelBuilder);
            SeedReviews(modelBuilder);
            SeedPackage(modelBuilder);
            SeedPromotionProperty(modelBuilder);
        }

        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "Landlord", NormalizedName = "LANDLORD" },
                new IdentityRole<int> { Id = 3, Name = "Renter", NormalizedName = "RENTER" }
            );
        }

        private static void SeedPackage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PromotionPackage>().HasData(
                new PromotionPackage
                {
                    Id = 1,
                    Name = "Basic Promotion",
                    Description = "Basic promotion package for property listings.",
                    Price = 1000000,
                    DurationInDays = 30,
                    Level = 1,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                },
                new PromotionPackage
                {
                    Id = 2,
                    Name = "Premium Promotion",
                    Description = "Premium promotion package for property listings.",
                    Price = 2000000,
                    DurationInDays = 60,
                    Level = 2,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                },
                new PromotionPackage
                {
                    Id = 3,
                    Name = "Ultimate Promotion",
                    Description = "Ultimate promotion package for property listings.",
                    Price = 3000000,
                    DurationInDays = 90,
                    Level = 3,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                }


            );
        }


        private static void SeedPromotionProperty(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyPromotion>().HasData(
                new PropertyPromotion
                {
                    Id = 1,
                    PropertyId = 1, 
                    PackageId = 1, 
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                },
                new PropertyPromotion
                {
                    Id = 2,
                    PropertyId = 2, 
                    PackageId = 2, 
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(29),
                },
                new PropertyPromotion
                {
                    Id = 3,
                    PropertyId = 3, 
                    PackageId = 3,
                    StartDate = DateTime.Now.AddDays(-2),
                    EndDate = DateTime.Now.AddDays(88),
                 
                }



            );
        }



        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = 1,
                    UserName = "admin@example.com",
                    NormalizedUserName = "ADMIN@EXAMPLE.COM",
                    Email = "admin@example.com",
                    PhoneNumber = "12345678910",
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
                    PhoneNumber = "02345678910",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    NormalizedEmail = "LANDLORD@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Landlord@123"),
                    Name = "Landlord User",
                    Role = "landlord",
                    ProfilePictureUrl = "https://th.bing.com/th/id/R.63d31ac6257157ef079f31bb32e342df?rik=63%2bkafQNo5seHg&pid=ImgRaw&r=0",
                    IsVerified = true,
                    CreatedAt = DateTime.Now
                    //03345678910

                },
                new ApplicationUser
                {
                    Id = 3,
                    UserName = "renter@example.com",
                    NormalizedUserName = "RENTER@EXAMPLE.COM",
                    Email = "renter@example.com",
                    PhoneNumber = "03345678910",
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

        private static void SeedUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 }, // Admin
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 }, // Landlord
                new IdentityUserRole<int> { UserId = 3, RoleId = 3 }, // Renter
                new IdentityUserRole<int> { UserId = 4, RoleId = 3 }  // Renter 2
            );
        }

        private static void SeedUserPreferences(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPreference>().HasData(
                new UserPreference
                {
                    Id = 1,
                    UserId = 3, // Renter
                    Location = "District 1",
                    PriceRangeMin = 3000000,
                    PriceRangeMax = 6000000,
                    Amenities = "WiFi,Parking",
                    CreatedAt = DateTime.Now
                },
                new UserPreference
                {
                    Id = 2,
                    UserId = 3, // Same renter, another preference
                    Location = "Go Vap",
                    PriceRangeMin = 1500000,
                    PriceRangeMax = 3000000,
                    Amenities = "WiFi",
                    CreatedAt = DateTime.Now.AddDays(-2)
                },
                new UserPreference
                {
                    Id = 3,
                    UserId = 4, // Renter 2
                    Location = "Tan Binh",
                    PriceRangeMin = 2000000,
                    PriceRangeMax = 4000000,
                    Amenities = "AC",
                    CreatedAt = DateTime.Now.AddDays(-1)
                }
            );

            // Seed UserPreferenceFavoriteProperties
            modelBuilder.Entity<UserPreferenceFavoriteProperties>().HasData(
                new { UserPreferenceId = 1, PropertyId = 1 }, // Renter favors District 1 apartment
                new { UserPreferenceId = 1, PropertyId = 2 }, // Renter favors Go Vap room
                new { UserPreferenceId = 3, PropertyId = 3 }  // Renter 2 favors Tan Binh house
            );
        }

        private static void SeedAmenities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { Id = 1, Name = "AC", Description = "Air Conditioning" },
                new Amenity { Id = 2, Name = "WiFi", Description = "High-speed Internet" },
                new Amenity { Id = 3, Name = "Parking", Description = "Parking Space" },
                new Amenity { Id = 4, Name = "Balcony", Description = "Private Balcony" }
            );
        }

        private static void SeedProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>().HasData(
                new Property
                {
                    Id = 1,
                    Title = "2BR Apartment in District 1",
                    Description = "Modern apartment with 2 bedrooms in the heart of HCMC.",
                    Address = "123 Nguyen Hue, District 1, HCMC",
                    Type = "apartment",
                    Area = 50.5m,
                    Bedrooms = 2,
                    LandlordId = 2,
                    Status = "available",
                    IsPromoted = false,
                    Price = 5000000,

                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "10.7769,106.7009",
                    CreatedAt = DateTime.Now
                },
                new Property
                {
                    Id = 2,
                    Title = "Shared Room in Go Vap",
                    Description = "Cozy shared room for students.",
                    Address = "456 Le Van Tho, Go Vap, HCMC",
                    Type = "room",
                    Area = 20.0m,
                    Bedrooms = 1,
                    LandlordId = 2,
                    Status = "available",
                    IsPromoted = false,
                    Price = 2000000,

                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "10.8505,106.6737",
                    CreatedAt = DateTime.Now.AddDays(-1)
                },
                new Property
                {
                    Id = 3,
                    Title = "3BR House in Tan Binh",
                    Description = "Spacious house with 3 bedrooms.",
                    Address = "789 Ly Thuong Kiet, Tan Binh, HCMC",
                    Type = "house",
                    Area = 80.0m,
                    Bedrooms = 3,
                    LandlordId = 2,
                    Status = "available",
                    IsPromoted = true,
                    Price = 8000000,

                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "10.7982,106.6582",
                    CreatedAt = DateTime.Now.AddDays(-2)
                }
            );
        }

        private static void SeedPropertyAmenities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyAmenity>().HasData(
                new PropertyAmenity { PropertyId = 1, AmenityId = 1 }, // Apartment has AC
                new PropertyAmenity { PropertyId = 1, AmenityId = 2 }, // Apartment has WiFi
                new PropertyAmenity { PropertyId = 1, AmenityId = 3 }, // Apartment has Parking
                new PropertyAmenity { PropertyId = 2, AmenityId = 2 }, // Room has WiFi
                new PropertyAmenity { PropertyId = 3, AmenityId = 1 }, // House has AC
                new PropertyAmenity { PropertyId = 3, AmenityId = 4 }  // House has Balcony
            );
        }

        private static void SeedPropertyImages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyImage>().HasData(
                new PropertyImage { Id = 1, PropertyId = 1, Url = "https://example.com/apartment1.jpg", IsPrimary = true, Order = 1 },
                new PropertyImage { Id = 2, PropertyId = 1, Url = "https://example.com/apartment2.jpg", IsPrimary = false, Order = 2 },
                new PropertyImage { Id = 3, PropertyId = 2, Url = "https://example.com/room1.jpg", IsPrimary = true, Order = 1 },
                new PropertyImage { Id = 4, PropertyId = 3, Url = "https://example.com/house1.jpg", IsPrimary = true, Order = 1 }
            );
        }

        private static void SeedPropertyPosts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyPost>().HasData(
                new PropertyPost
                {
                    Id = 1,
                    PropertyId = 1,
                    LandlordId = 2,
                    Status = "approved",
                    CreatedAt = DateTime.Now,
                    VerifiedAt = DateTime.Now,
                    VerifiedBy = 1 // Admin verified
                },
                new PropertyPost
                {
                    Id = 2,
                    PropertyId = 2,
                    LandlordId = 2,
                    Status = "approved",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    VerifiedAt = DateTime.Now.AddDays(-1),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 3,
                    PropertyId = 3,
                    LandlordId = 2,
                    Status = "approved",
                    CreatedAt = DateTime.Now.AddDays(-2),
                    VerifiedAt = DateTime.Now.AddDays(-2),
                    VerifiedBy = 1
                }
            );
        }




        private static void SeedPayments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    Id = 1,
                    ContractId = 1,
                    Amount = 5000000,
                    PaymentMethod = "Momo",
                    Status = "completed",
                    PaidAt = DateTime.Now,
                    TransactionId = 1
                }
            );
        }

        private static void SeedTransactions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    Id = 1,
                    UserId = 3,
                    Amount = 5000000,
                    TransactionType = "deposit",
                    Description = "Deposit for apartment in District 1",
                    CreatedAt = DateTime.Now
                }
            );
        }

        private static void SeedReviews(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    PropertyId = 1,
                    RenterId = 3,
                    Rating = 4,
                    Comment = "Great location and clean apartment!",
                    IsApproved = true,
                    CreatedAt = DateTime.Now.AddDays(-1)
                }
            );
        }


    }
}
