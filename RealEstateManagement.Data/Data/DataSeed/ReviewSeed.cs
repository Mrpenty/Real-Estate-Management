using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
using System;

namespace RealEstateManagement.Data.Data.DataSeed
{
    public static class ReviewSeed
    {
        public static void SeedReviews(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    PropertyId = 1,
                    RenterId = 3,
                    Rating = 4,
                    Comment = "Great location and clean apartment!",
                    IsApproved = true,
                    CreatedAt = DateTime.Now.AddDays(-1)
                }
            );
        }
    }
} 