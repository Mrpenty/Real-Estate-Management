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
                    VerifiedBy = 1 // Admin verified
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
                }
            );
        }
    }
}