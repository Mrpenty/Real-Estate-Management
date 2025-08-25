using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.User;
using System;

namespace RealEstateManagement.Data.Data.DataSeed.User
{
    public static class UserPreferenceSeed
    {
        public static void SeedUserPreferences(ModelBuilder modelBuilder)
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
            modelBuilder.Entity<UserFavoriteProperty>().HasData(
                new { UserId = 1, PropertyId = 1, CreatedAt = DateTime.UtcNow }, // Renter favors District 1 apartment
                new { UserId = 1, PropertyId = 2, CreatedAt = DateTime.UtcNow }, // Renter favors Go Vap room
                new { UserId = 3, PropertyId = 3, CreatedAt = DateTime.UtcNow }  // Renter 2 favors Tan Binh house
            );
        }
    }
}
