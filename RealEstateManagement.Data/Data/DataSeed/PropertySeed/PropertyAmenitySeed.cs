using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.Data.Data.DataSeed.PropertySeed
{
    public static class PropertyAmenitySeed
    {
        public static void SeedPropertyAmenities(ModelBuilder modelBuilder)
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
    }
}