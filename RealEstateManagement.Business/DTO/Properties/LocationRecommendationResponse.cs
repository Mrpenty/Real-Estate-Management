using RealEstateManagement.Business.DTO.Location;

namespace RealEstateManagement.Business.DTO.Properties
{
    public class LocationRecommendationResponse
    {
        /// <summary>
        /// Danh sách bất động sản được recommend
        /// </summary>
        public List<PropertyRecommendationDTO> Properties { get; set; } = new List<PropertyRecommendationDTO>();

        /// <summary>
        /// Địa chỉ tìm kiếm
        /// </summary>
        public string SearchLocation { get; set; }

        /// <summary>
        /// Tổng số kết quả
        /// </summary>
        public int TotalResults { get; set; }

        /// <summary>
        /// Lý do recommendation từ AI
        /// </summary>
        public string RecommendationReason { get; set; }

        /// <summary>
        /// Danh sách tiện ích gần đó
        /// </summary>
        public List<string> NearbyAmenities { get; set; } = new List<string>();

        /// <summary>
        /// Thông tin giao thông gần đó
        /// </summary>
        public List<string> TransportationInfo { get; set; } = new List<string>();

        /// <summary>
        /// Thông tin chi tiết về tiện ích xung quanh
        /// </summary>
        public List<DetailedAmenityDTO> DetailedAmenities { get; set; } = new List<DetailedAmenityDTO>();
        
        /// <summary>
        /// Thống kê tiện ích xung quanh (ngắn gọn)
        /// </summary>
        public string AmenityStatistics { get; set; }
        
        /// <summary>
        /// Thống kê tiện ích xung quanh (chi tiết)
        /// </summary>
        public string DetailedAmenityStatistics { get; set; }
    }
} 