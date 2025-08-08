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
},
new News
{
   Id = 3,
   Title = "Mẹo chọn khu trọ an toàn tại Hà Nội",
   Slug = "meo-chon-khu-tro-an-toan-tai-ha-noi",
   Content = "Một số mẹo giúp bạn chọn khu trọ an toàn và tiện nghi tại Hà Nội...",
   Summary = "Hướng dẫn chọn khu trọ an toàn tại Hà Nội",
   PublishedAt = new DateTime(2025, 7, 10),
   AuthorName = "Nguyen Van A",
   Source = "TuoiTre.vn",
   IsPublished = true,
   CreatedAt = new DateTime(2025, 7, 10)
},
new News
{
   Id = 4,
   Title = "Xu hướng thuê nhà năm 2025",
   Slug = "xu-huong-thue-nha-nam-2025",
   Content = "Các xu hướng mới trong thị trường thuê nhà tại Việt Nam năm 2025...",
   Summary = "Tìm hiểu xu hướng thuê nhà trong năm nay",
   PublishedAt = new DateTime(2025, 7, 15),
   AuthorName = "Tran Thi B",
   Source = "VTV.vn",
   IsPublished = true,
   CreatedAt = new DateTime(2025, 7, 15)
},
new News
{
   Id = 5,
   Title = "Hướng dẫn ký hợp đồng thuê nhà",
   Slug = "huong-dan-ky-hop-dong-thue-nha",
   Content = "Những điều cần biết khi ký hợp đồng thuê nhà để tránh rủi ro...",
   Summary = "Hướng dẫn chi tiết về ký hợp đồng thuê nhà",
   PublishedAt = new DateTime(2025, 7, 20),
   AuthorName = "Le Van C",
   Source = null,
   IsPublished = true,
   CreatedAt = new DateTime(2025, 7, 20)
},
new News
{
   Id = 6,
   Title = "Lợi ích của việc thuê nhà dài hạn",
   Slug = "loi-ich-cua-viec-thue-nha-dai-han",
   Content = "Thuê nhà dài hạn mang lại nhiều lợi ích cho người thuê và chủ nhà...",
   Summary = "Tìm hiểu lợi ích khi thuê nhà dài hạn",
   PublishedAt = new DateTime(2025, 7, 25),
   AuthorName = "Pham Thi D",
   Source = "ThanhNien.vn",
   IsPublished = true,
   CreatedAt = new DateTime(2025, 7, 25)
},
new News
{
   Id = 7,
   Title = "Top 5 khu trọ giá rẻ tại Đà Nẵng",
   Slug = "top-5-khu-tro-gia-re-tai-da-nang",
   Content = "Danh sách các khu trọ giá rẻ và chất lượng tại Đà Nẵng năm 2025...",
   Summary = "Khám phá 5 khu trọ giá rẻ tại Đà Nẵng",
   PublishedAt = new DateTime(2025, 8, 1),
   AuthorName = "Hoang Van E",
   Source = "NguoiLaoDong.vn",
   IsPublished = true,
   CreatedAt = new DateTime(2025, 8, 1)
}
);
        }
    }
}
