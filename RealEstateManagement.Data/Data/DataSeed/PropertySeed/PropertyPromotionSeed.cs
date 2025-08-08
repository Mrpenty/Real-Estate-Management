using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;

namespace RealEstateManagement.Data.Data.DataSeed.PropertySeed
{
    public static class PropertyPromotionSeed
    {
        public static void SeedPromotionProperty(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PropertyPromotion>().HasData(
                new PropertyPromotion
                {
                    Id = 1,
                    PropertyId = 3,
                    PackageId = 1,
                    StartDate = DateTime.Now.AddDays(-2),
                    EndDate = DateTime.Now.AddDays(28),
                },
                new PropertyPromotion
                {
                    Id = 2,
                    PropertyId = 4,
                    PackageId = 2,
                    StartDate = DateTime.Now.AddDays(-3),
                    EndDate = DateTime.Now.AddDays(27),
                },
                new PropertyPromotion
                {
                    Id = 3,
                    PropertyId = 5,
                    PackageId = 3,
                    StartDate = DateTime.Now.AddDays(-4),
                    EndDate = DateTime.Now.AddDays(26),
                },
                new PropertyPromotion
                {
                    Id = 4,
                    PropertyId = 6,
                    PackageId = 1,
                    StartDate = DateTime.Now.AddDays(-5),
                    EndDate = DateTime.Now.AddDays(25),
                },
                new PropertyPromotion
                {
                    Id = 5,
                    PropertyId = 7,
                    PackageId = 2,
                    StartDate = DateTime.Now.AddDays(-6),
                    EndDate = DateTime.Now.AddDays(24),
                },
                new PropertyPromotion
                {
                    Id = 6,
                    PropertyId = 8,
                    PackageId = 3,
                    StartDate = DateTime.Now.AddDays(-7),
                    EndDate = DateTime.Now.AddDays(23),
                },
                new PropertyPromotion
                {
                    Id = 7,
                    PropertyId = 9,
                    PackageId = 1,
                    StartDate = DateTime.Now.AddDays(-8),
                    EndDate = DateTime.Now.AddDays(22),
                }
            );
        }
    }
}