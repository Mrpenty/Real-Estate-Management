using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Payment;
using System;

namespace RealEstateManagement.Data.Data.DataSeed.PaymentSeed
{
    public static class PackageSeed
    {
        public static void SeedPackage(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PromotionPackage>().HasData(
                new PromotionPackage
                {
                    Id = 1,
                    Name = "Basic Promotion",
                    Description = "Basic promotion package for property listings.",
                    Price = 10000,
                    DurationInDays = 30,
                    Level = 1,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                },
                new PromotionPackage
                {
                    Id = 2,
                    Name = "Premium Promotion",
                    Description = "Premium promotion package for property listings.",
                    Price = 400000,
                    DurationInDays = 60,
                    Level = 2,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                },
                new PromotionPackage
                {
                    Id = 3,
                    Name = "Ultimate Promotion",
                    Description = "Ultimate promotion package for property listings.",
                    Price = 50000,
                    DurationInDays = 90,
                    Level = 3,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                }


            );
        }
    }
}