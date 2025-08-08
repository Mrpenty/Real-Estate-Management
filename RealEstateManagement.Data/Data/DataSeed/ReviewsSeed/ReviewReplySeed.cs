using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Data.DataSeed.ReviewsSeed
{
    public static class ReviewReplySeed
    {
        public static void SeedReviewReplies(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReviewReply>().HasData(
                new ReviewReply
                {
                    Id = 1,
                    ReviewId = 1,
                    LandlordId = 2,
                    ReplyContent = "Cảm ơn bạn đã tin tưởng và sử dụng dịch vụ của chúng tôi!",
                    CreatedAt = new DateTime(2025, 7, 16, 9, 0, 0),
                    IsFlagged = false,
                    IsVisible = true
                },
                new ReviewReply
                {
                    Id = 2,
                    ReviewId = 2,
                    LandlordId = 2,
                    ReplyContent = "Cảm ơn bạn đã phản hồi tích cực. Chúc bạn luôn vui vẻ!",
                    CreatedAt = new DateTime(2025, 8, 6, 8, 30, 0),
                    IsFlagged = false,
                    IsVisible = true
                }
            );
        }
    }
}
