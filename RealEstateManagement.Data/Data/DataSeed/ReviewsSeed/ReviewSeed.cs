using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Reviews;
using System;

namespace RealEstateManagement.Data.Data.DataSeed.ReviewsSeed
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
                    ContractId = 1,
                    Rating = 5,
                    ReviewText = "Căn hộ rất sạch sẽ, chủ nhà thân thiện. Sẽ giới thiệu bạn bè!",
                    CreatedAt = new DateTime(2025, 7, 15, 10, 0, 0),
                    IsFlagged = false,
                    IsVisible = true
                },
                new Review
                {
                    Id = 2,
                    PropertyId = 2,
                    RenterId = 4,
                    ContractId = 2,
                    Rating = 4,
                    ReviewText = "Giá hợp lý, vị trí thuận tiện. Chủ nhà hỗ trợ tốt.",
                    CreatedAt = new DateTime(2025, 8, 5, 15, 30, 0),
                    IsFlagged = false,
                    IsVisible = true
                }
            );
        }
    }
}