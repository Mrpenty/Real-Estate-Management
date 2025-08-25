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
                    Status = "active",
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
                    Status = "active",
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
                    Status = "active",
                    IsPromoted = true,
                    Price = 8000000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "10.7982,106.6582",
                    CreatedAt = DateTime.Now.AddDays(-2)
                },
                new Property
                {
                    Id = 4,
                    Title = "Luxury Studio in District 1",
                    Description = "High-end studio apartment with modern amenities and city view.",
                    AddressId = 4,
                    Type = "apartment",
                    Area = 35.0m,
                    Bedrooms = 1,
                    LandlordId = 5,
                    Status = "active",
                    IsPromoted = true,
                    Price = 6500000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "10.7769,106.7009",
                    CreatedAt = DateTime.Now.AddDays(-3)
                },
                new Property
                {
                    Id = 5,
                    Title = "Family Villa in District 7",
                    Description = "Beautiful villa with garden, perfect for families.",
                    AddressId = 5,
                    Type = "house",
                    Area = 120.0m,
                    Bedrooms = 4,
                    LandlordId = 5,
                    Status = "active",
                    IsPromoted = true,
                    Price = 15000000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "10.7308,106.7267",
                    CreatedAt = DateTime.Now.AddDays(-4)
                },
                new Property
                {
                    Id = 6,
                    Title = "Student Dormitory in District 3",
                    Description = "Affordable dormitory for students near universities.",
                    AddressId = 6,
                    Type = "apartment",
                    Area = 15.0m,
                    Bedrooms = 1,
                    LandlordId = 6,
                    Status = "active",
                    IsPromoted = true,
                    Price = 1500000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "10.7829,106.6889",
                    CreatedAt = DateTime.Now.AddDays(-5)
                },
                new Property
                {
                    Id = 7,
                    Title = "Penthouse in Binh Thanh",
                    Description = "Luxury penthouse with panoramic city views.",
                    AddressId = 7,
                    Type = "apartment",
                    Area = 150.0m,
                    Bedrooms = 3,
                    LandlordId = 6,
                    Status = "active",
                    IsPromoted = true,
                    Price = 25000000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "10.8105,106.7091",
                    CreatedAt = DateTime.Now.AddDays(-6)
                },
                new Property
                {
                    Id = 8,
                    Title = "Cozy Apartment in Phu Nhuan",
                    Description = "Well-maintained apartment in quiet neighborhood.",
                    AddressId = 8,
                    Type = "apartment",
                    Area = 45.0m,
                    Bedrooms = 2,
                    LandlordId = 6,
                    Status = "active",
                    IsPromoted = true,
                    Price = 5500000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "10.7947,106.6789",
                    CreatedAt = DateTime.Now.AddDays(-7)
                },
                new Property
                {
                    Id = 9,
                    Title = "Modern Loft in District 2",
                    Description = "Industrial-style loft with high ceilings and open space.",
                    AddressId = 9,
                    Type = "room",
                    Area = 60.0m,
                    Bedrooms = 2,
                    LandlordId = 2,
                    Status = "active",
                    IsPromoted = true,
                    Price = 7500000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "10.7871,106.7492",
                    CreatedAt = DateTime.Now.AddDays(-8)
                },
                new Property
                {
                    Id = 10,
                    Title = "Traditional House in Hanoi Old Quarter",
                    Description = "Charming traditional Vietnamese house in historic area.",
                    AddressId = 10,
                    Type = "house",
                    Area = 70.0m,
                    Bedrooms = 3,
                    LandlordId = 2,
                    Status = "active",
                    IsPromoted = false,
                    Price = 6000000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "21.0285,105.8542",
                    CreatedAt = DateTime.Now.AddDays(-9)
                },
                new Property
                {
                    Id = 11,
                    Title = "Studio in Ba Dinh District",
                    Description = "Compact studio perfect for young professionals.",
                    AddressId = 11,
                    Type = "room",
                    Area = 25.0m,
                    Bedrooms = 1,
                    LandlordId = 5,
                    Status = "active",
                    IsPromoted = false,
                    Price = 3500000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "21.0352,105.8342",
                    CreatedAt = DateTime.Now.AddDays(-10)
                },
                new Property
                {
                    Id = 12,
                    Title = "Shared Apartment in Hai Ba Trung",
                    Description = "Furnished shared apartment with utilities included.",
                    AddressId = 12,
                    Type = "apartment",
                    Area = 40.0m,
                    Bedrooms = 2,
                    LandlordId = 5,
                    Status = "active",
                    IsPromoted = false,
                    Price = 4000000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "21.0122,105.8441",
                    CreatedAt = DateTime.Now.AddDays(-11)
                },
                new Property
                {
                    Id = 13,
                    Title = "Student Room in Dong Da",
                    Description = "Budget-friendly room for students near universities.",
                    AddressId = 13,
                    Type = "room",
                    Area = 18.0m,
                    Bedrooms = 1,
                    LandlordId = 5,
                    Status = "active",
                    IsPromoted = false,
                    Price = 1800000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "21.0188,105.8292",
                    CreatedAt = DateTime.Now.AddDays(-12)
                },
                new Property
                {
                    Id = 14,
                    Title = "Family Apartment in Cau Giay",
                    Description = "Spacious apartment suitable for families with children.",
                    AddressId = 14,
                    Type = "apartment",
                    Area = 85.0m,
                    Bedrooms = 3,
                    LandlordId = 6,
                    Status = "active",
                    IsPromoted = false,
                    Price = 7000000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "21.0367,105.7826",
                    CreatedAt = DateTime.Now.AddDays(-13)
                },
                new Property
                {
                    Id = 15,
                    Title = "Lakeside Villa in Tay Ho",
                    Description = "Beautiful villa with lake view and private garden.",
                    AddressId = 15,
                    Type = "house",
                    Area = 200.0m,
                    Bedrooms = 4,
                    LandlordId = 6,
                    Status = "active",
                    IsPromoted = false,
                    Price = 20000000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "21.0811,105.8144",
                    CreatedAt = DateTime.Now.AddDays(-14)
                },
                new Property
                {
                    Id = 16,
                    Title = "Modern Condo in Long Bien",
                    Description = "Newly built condo with modern amenities and security.",
                    AddressId = 16,
                    Type = "apartment",
                    Area = 55.0m,
                    Bedrooms = 2,
                    LandlordId = 2,
                    Status = "active",
                    IsPromoted = false,
                    Price = 6500000,
                    IsVerified = true,
                    ViewsCount = 0,
                    Location = "21.0455,105.8952",
                    CreatedAt = DateTime.Now.AddDays(-15)
                }
            );
        }
    }
}