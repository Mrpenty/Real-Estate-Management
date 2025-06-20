using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;

namespace RealEstateManagement.Data.Data.DataSeed.PropertySeed
{
    public static class PropertySeed
    {
        public static void SeedProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>().HasData(
                new Property
                {
                    Id = 1,
                    Title = "2BR Apartment in District 1",
                    Description = "Modern apartment with 2 bedrooms in the heart of HCMC.",
                    AddressId = 1,
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
                    AddressId = 2,
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
                    AddressId = 3,
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
    }
}