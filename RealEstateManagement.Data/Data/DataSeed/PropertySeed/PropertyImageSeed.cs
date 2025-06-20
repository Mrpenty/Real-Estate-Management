using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.Data.Data.DataSeed.PropertySeed
{
    public static class PropertyImageSeed
    {
        public static void SeedPropertyImages(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyImage>().HasData(
                new PropertyImage { Id = 1, PropertyId = 1, Url = "https://example.com/apartment1.jpg", IsPrimary = true, Order = 1 },
                new PropertyImage { Id = 2, PropertyId = 1, Url = "https://example.com/apartment2.jpg", IsPrimary = false, Order = 2 },
                new PropertyImage { Id = 3, PropertyId = 2, Url = "https://example.com/room1.jpg", IsPrimary = true, Order = 1 },
                new PropertyImage { Id = 4, PropertyId = 3, Url = "https://example.com/house1.jpg", IsPrimary = true, Order = 1 }
            );
        }
    }
}