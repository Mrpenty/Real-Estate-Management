using RealEstateManagement.Business.DTO.Properties;

namespace RealEstateManagement.Business.DTO.Properties
{
    /// <summary>
    /// Thống kê tiện ích xung quanh khu vực
    /// </summary>
    public class AmenityStatistics
    {
        public AmenityStatistics()
        {
            AmenityCounts = new Dictionary<string, int>();
            AverageDistances = new Dictionary<string, double>();
            AverageRatings = new Dictionary<string, double>();
            NearestAmenities = new Dictionary<string, DetailedAmenityDTO>();
        }

        /// <summary>
        /// Tổng số tiện ích
        /// </summary>
        public int TotalAmenities { get; set; }

        /// <summary>
        /// Số loại tiện ích khác nhau
        /// </summary>
        public int TotalTypes { get; set; }

        /// <summary>
        /// Khoảng cách trung bình đến tiện ích (km)
        /// </summary>
        public double AverageDistance { get; set; }

        /// <summary>
        /// Điểm đánh giá trung bình của tiện ích
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// Số lượng tiện ích theo từng loại
        /// </summary>
        public Dictionary<string, int> AmenityCounts { get; set; }

        /// <summary>
        /// Khoảng cách trung bình theo từng loại tiện ích (km)
        /// </summary>
        public Dictionary<string, double> AverageDistances { get; set; }

        /// <summary>
        /// Điểm đánh giá trung bình theo từng loại tiện ích
        /// </summary>
        public Dictionary<string, double> AverageRatings { get; set; }

        /// <summary>
        /// Tiện ích gần nhất theo từng loại
        /// </summary>
        public Dictionary<string, DetailedAmenityDTO> NearestAmenities { get; set; }

        /// <summary>
        /// Lấy thống kê dạng text để hiển thị
        /// </summary>
        public string GetSummaryText()
        {
            var summary = new List<string>();
            summary.Add($"🏪 TỔNG QUAN TIỆN ÍCH XUNG QUANH CÁC BDS");
            summary.Add($"• Tổng số tiện ích: {TotalAmenities}");
            summary.Add($"• Số loại tiện ích: {TotalTypes}");
            summary.Add($"• Khoảng cách trung bình từ BDS: {AverageDistance:F1}km");
            summary.Add($"• Điểm đánh giá trung bình: {AverageRating:F1}/5.0");
            summary.Add("");

            if (AmenityCounts.Any())
            {
                summary.Add("📍 CHI TIẾT THEO LOẠI:");
                foreach (var kvp in AmenityCounts.OrderByDescending(x => x.Value))
                {
                    var type = kvp.Key;
                    var count = kvp.Value;
                    var avgDistance = AverageDistances.GetValueOrDefault(type, 0);
                    var avgRating = AverageRatings.GetValueOrDefault(type, 0);
                    
                    summary.Add($"• {type}: {count} địa điểm");
                    summary.Add($"  - Khoảng cách trung bình từ BDS: {avgDistance:F1}km");
                    summary.Add($"  - Điểm đánh giá: {avgRating:F1}/5.0");
                    
                    if (NearestAmenities.TryGetValue(type, out var nearest))
                    {
                        summary.Add($"  - Gần nhất: {nearest.Name} (cách BDS {nearest.DistanceKm:F1}km)");
                    }
                    summary.Add("");
                }
            }

            return string.Join("\n", summary);
        }

        /// <summary>
        /// Lấy thống kê ngắn gọn cho hiển thị
        /// </summary>
        public string GetShortSummary()
        {
            var topAmenities = AmenityCounts
                .OrderByDescending(x => x.Value)
                .Take(5)
                .Select(x => $"{x.Key}: {x.Value} địa điểm")
                .ToList();

            return $"🏪 {TotalAmenities} tiện ích xung quanh các BDS ({TotalTypes} loại) - Trung bình: {AverageDistance:F1}km từ BDS";
        }
    }
} 