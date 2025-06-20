using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.AddressEnity;

namespace RealEstateManagement.Data.Data.DataSeed.AddressSeed
{
    public static class AddressSeed
    {
        public static void SeedAddress(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>().HasData(
                new Address
                {
                    Id = 1,
                    PropertyId = 1,
                    ProvinceId = 1,
                    WardId = 2,
                    StreetId = 1,
                    DetailedAddress = "123"
                },
                new Address
                {
                    Id = 2,
                    PropertyId = 2,
                    ProvinceId = 1,
                    WardId = 3,
                    StreetId = 2,
                    DetailedAddress = "456"
                },
                new Address
                {
                    Id = 3,
                    PropertyId = 3,
                    ProvinceId = 1,
                    WardId = 4,
                    StreetId = 3,
                    DetailedAddress = "789"
                }
            );
        }
    }
}