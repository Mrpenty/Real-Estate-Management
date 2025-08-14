using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Properties
{
    public class WeeklyBestRatedPropertyDTO
    {
        // Thông tin hiển thị
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public string PrimaryImageUrl { get; set; }
        public string Province { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; }
        public bool IsFavorite { get; set; }

        // Điểm trong tuần
        public double WeeklyAverageRating { get; set; } // 0..5
        public int WeeklyReviewCount { get; set; }

        // Tùy chọn: ưu tiên hiển thị
        public int PromotionLevel { get; set; } // tối đa level gói đang chạy
        public DateTime PropertyCreatedAt { get; set; }
        public int ViewsCount { get; set; }
    }
}
