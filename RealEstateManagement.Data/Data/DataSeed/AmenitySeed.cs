using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.Data.Data.DataSeed
{
    public static class AmenitySeed
    {
        public static void SeedAmenities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity { Id = 1, Name = "AC", Description = "Air Conditioning" },
                new Amenity { Id = 2, Name = "WiFi", Description = "High-speed Internet" },
                new Amenity { Id = 3, Name = "Parking", Description = "Parking Space" },
                new Amenity { Id = 4, Name = "Balcony", Description = "Private Balcony" }
            );
        }
    }
} 