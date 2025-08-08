using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;

namespace RealEstateManagement.Data.Data.DataSeed.PropertySeed
{
    public static class PropertyPostSeed
    {
        public static void SeedPropertyPosts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyPost>().HasData(
                new PropertyPost
                {
                    Id = 1,
                    PropertyId = 1,
                    LandlordId = 2,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now,
                    VerifiedAt = DateTime.Now,
                    VerifiedBy = 1 
                },
                new PropertyPost
                {
                    Id = 2,
                    PropertyId = 2,
                    LandlordId = 2,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-1),
                    VerifiedAt = DateTime.Now.AddDays(-1),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 3,
                    PropertyId = 3,
                    LandlordId = 2,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-2),
                    VerifiedAt = DateTime.Now.AddDays(-2),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 4,
                    PropertyId = 4,
                    LandlordId = 5,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-3),
                    VerifiedAt = DateTime.Now.AddDays(-3),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 5,
                    PropertyId = 5,
                    LandlordId = 5,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-4),
                    VerifiedAt = DateTime.Now.AddDays(-4),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 6,
                    PropertyId = 6,
                    LandlordId = 6,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-5),
                    VerifiedAt = DateTime.Now.AddDays(-5),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 7,
                    PropertyId = 7,
                    LandlordId = 6,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-6),
                    VerifiedAt = DateTime.Now.AddDays(-6),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 8,
                    PropertyId = 8,
                    LandlordId = 6,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-7),
                    VerifiedAt = DateTime.Now.AddDays(-7),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 9,
                    PropertyId = 9,
                    LandlordId = 2,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-8),
                    VerifiedAt = DateTime.Now.AddDays(-8),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 10,
                    PropertyId = 10,
                    LandlordId = 2,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-9),
                    VerifiedAt = DateTime.Now.AddDays(-9),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 11,
                    PropertyId = 11,
                    LandlordId = 5,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-10),
                    VerifiedAt = DateTime.Now.AddDays(-10),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 12,
                    PropertyId = 12,
                    LandlordId = 5,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-11),
                    VerifiedAt = DateTime.Now.AddDays(-11),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 13,
                    PropertyId = 13,
                    LandlordId = 5,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-12),
                    VerifiedAt = DateTime.Now.AddDays(-12),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 14,
                    PropertyId = 14,
                    LandlordId = 6,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-13),
                    VerifiedAt = DateTime.Now.AddDays(-13),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 15,
                    PropertyId = 15,
                    LandlordId = 6,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-14),
                    VerifiedAt = DateTime.Now.AddDays(-14),
                    VerifiedBy = 1
                },
                new PropertyPost
                {
                    Id = 16,
                    PropertyId = 16,
                    LandlordId = 2,
                    Status = PropertyPost.PropertyPostStatus.Approved,
                    CreatedAt = DateTime.Now.AddDays(-15),
                    VerifiedAt = DateTime.Now.AddDays(-15),
                    VerifiedBy = 1
                }
            );
        }
    }
}