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
                },
                new Address
                {
                    Id = 4,
                    PropertyId = 4,
                    ProvinceId = 1,
                    WardId = 5,
                    StreetId = 4,
                    DetailedAddress = "101"
                },
                new Address
                {
                    Id = 5,
                    PropertyId = 5,
                    ProvinceId = 1,
                    WardId = 6,
                    StreetId = 5,
                    DetailedAddress = "202"
                },
                new Address
                {
                    Id = 6,
                    PropertyId = 6,
                    ProvinceId = 1,
                    WardId = 7,
                    StreetId = 6,
                    DetailedAddress = "303"
                },
                new Address
                {
                    Id = 7,
                    PropertyId = 7,
                    ProvinceId = 1,
                    WardId = 8,
                    StreetId = 7,
                    DetailedAddress = "404"
                },
                new Address
                {
                    Id = 8,
                    PropertyId = 8,
                    ProvinceId = 1,
                    WardId = 9,
                    StreetId = 8,
                    DetailedAddress = "505"
                },
                new Address
                {
                    Id = 9,
                    PropertyId = 9,
                    ProvinceId = 1,
                    WardId = 10,
                    StreetId = 9,
                    DetailedAddress = "606"
                },
                new Address
                {
                    Id = 10,
                    PropertyId = 10,
                    ProvinceId = 2,
                    WardId = 11,
                    StreetId = 10,
                    DetailedAddress = "707"
                },
                new Address
                {
                    Id = 11,
                    PropertyId = 11,
                    ProvinceId = 2,
                    WardId = 12,
                    StreetId = 11,
                    DetailedAddress = "808"
                },
                new Address
                {
                    Id = 12,
                    PropertyId = 12,
                    ProvinceId = 2,
                    WardId = 13,
                    StreetId = 12,
                    DetailedAddress = "909"
                },
                new Address
                {
                    Id = 13,
                    PropertyId = 13,
                    ProvinceId = 2,
                    WardId = 14,
                    StreetId = 13,
                    DetailedAddress = "111"
                },
                new Address
                {
                    Id = 14,
                    PropertyId = 14,
                    ProvinceId = 2,
                    WardId = 15,
                    StreetId = 14,
                    DetailedAddress = "222"
                },
                new Address
                {
                    Id = 15,
                    PropertyId = 15,
                    ProvinceId = 2,
                    WardId = 16,
                    StreetId = 15,
                    DetailedAddress = "333"
                },
                new Address
                {
                    Id = 16,
                    PropertyId = 16,
                    ProvinceId = 2,
                    WardId = 17,
                    StreetId = 16,
                    DetailedAddress = "444"
                }
            );
        }
    }
}