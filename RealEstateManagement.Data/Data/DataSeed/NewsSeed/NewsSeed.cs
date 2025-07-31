using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Data.DataSeed.NewsSeed
{
    public static class NewsSeed
    {
        public static void SeedNews(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<News>().HasData(
                new News
                {
                    Id = 1,
                    Title = "5 lưu ý khi thuê nhà trọ tại TP.HCM",
                    Slug = "5-luu-y-khi-thue-nha-tro-tai-tphcm",
                    Content = "Dưới đây là 5 điều bạn nên cân nhắc khi thuê nhà trọ...",
                    Summary = "Các lưu ý quan trọng khi thuê trọ tại TP.HCM",
                    PublishedAt = new DateTime(2025, 7, 1),
                    AuthorName = "Admin",
                    Source = null,
                    IsPublished = true,
                    CreatedAt = new DateTime(2025, 7, 1)
                },
                new News
                {
                    Id = 2,
                    Title = "So sánh giá thuê nhà giữa TP.HCM và Hà Nội",
                    Slug = "so-sanh-gia-thue-nha-giua-tphcm-va-ha-noi",
                    Content = "Giá thuê nhà ở hai thành phố lớn có sự chênh lệch như thế nào...",
                    Summary = "Giá thuê nhà giữa TP.HCM và Hà Nội có gì khác biệt?",
                    PublishedAt = new DateTime(2025, 7, 5),
                    AuthorName = "AI Bot",
                    Source = "Vietnamnet.vn",
                    IsPublished = true,
                    CreatedAt = new DateTime(2025, 7, 5)
                }
            );
        }
    }
}
