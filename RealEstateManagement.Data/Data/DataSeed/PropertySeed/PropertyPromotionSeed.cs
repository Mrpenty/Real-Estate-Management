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
                    PropertyId = 1,
                    PackageId = 1,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30),
                },
                new PropertyPromotion
                {
                    Id = 2,
                    PropertyId = 2,
                    PackageId = 2,
                    StartDate = DateTime.Now.AddDays(-1),
                    EndDate = DateTime.Now.AddDays(29),
                },
                new PropertyPromotion
                {
                    Id = 3,
                    PropertyId = 3,
                    PackageId = 3,
                    StartDate = DateTime.Now.AddDays(-2),
                    EndDate = DateTime.Now.AddDays(88),

                }



            );
        }
    }
}