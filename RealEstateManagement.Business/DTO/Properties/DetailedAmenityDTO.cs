namespace RealEstateManagement.Business.DTO.Properties
{
    public class DetailedAmenityDTO
    {
        /// <summary>
        /// Tên tiện ích
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loại tiện ích (restaurant, school, hospital, etc.)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Khoảng cách từ vị trí tìm kiếm (km)
        /// </summary>
        public double DistanceKm { get; set; }

        /// <summary>
        /// Thời gian đi bộ (phút)
        /// </summary>
        public int WalkingTimeMinutes { get; set; }

        /// <summary>
        /// Thời gian lái xe (phút)
        /// </summary>
        public int DrivingTimeMinutes { get; set; }

        /// <summary>
        /// Đánh giá (nếu có)
        /// </summary>
        public double? Rating { get; set; }

        /// <summary>
        /// Số đánh giá (nếu có)
        /// </summary>
        public int? UserRatingsTotal { get; set; }

        /// <summary>
        /// Tọa độ
        /// </summary>
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        /// <summary>
        /// Mô tả thêm
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Giờ mở cửa (nếu có)
        /// </summary>
        public string OpeningHours { get; set; }

        /// <summary>
        /// Số điện thoại (nếu có)
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Website (nếu có)
        /// </summary>
        public string Website { get; set; }
    }

    public class AmenityCategoryDTO
    {
        /// <summary>
        /// Tên danh mục
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Mô tả danh mục
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Danh sách tiện ích trong danh mục
        /// </summary>
        public List<DetailedAmenityDTO> Amenities { get; set; } = new List<DetailedAmenityDTO>();

        /// <summary>
        /// Số lượng tiện ích
        /// </summary>
        public int Count => Amenities.Count;
    }
} 