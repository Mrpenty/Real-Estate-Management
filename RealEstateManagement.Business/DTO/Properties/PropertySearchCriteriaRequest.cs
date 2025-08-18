using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Business.DTO.Properties
{
    public class PropertySearchCriteriaRequest
    {
        /// <summary>
        /// Địa chỉ hoặc khu vực tìm kiếm
        /// </summary>
        [Required(ErrorMessage = "Địa chỉ tìm kiếm không được để trống")]
        public string SearchLocation { get; set; }

        /// <summary>
        /// Loại bất động sản (apartment, house, villa, etc.)
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// Giá tối thiểu (triệu đồng/tháng)
        /// </summary>
        [Range(0, 1000, ErrorMessage = "Giá tối thiểu phải từ 0 đến 1000 triệu")]
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// Giá tối đa (triệu đồng/tháng)
        /// </summary>
        [Range(0, 1000, ErrorMessage = "Giá tối đa phải từ 0 đến 1000 triệu")]
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// Số phòng ngủ tối thiểu
        /// </summary>
        [Range(0, 10, ErrorMessage = "Số phòng ngủ phải từ 0 đến 10")]
        public int? MinBedrooms { get; set; }

        /// <summary>
        /// Diện tích tối thiểu (m²)
        /// </summary>
        [Range(0, 1000, ErrorMessage = "Diện tích phải từ 0 đến 1000 m²")]
        public double? MinArea { get; set; }

        /// <summary>
        /// Diện tích tối đa (m²)
        /// </summary>
        [Range(0, 1000, ErrorMessage = "Diện tích phải từ 0 đến 1000 m²")]
        public double? MaxArea { get; set; }

        /// <summary>
        /// Số kết quả tối đa
        /// </summary>
        [Range(1, 100, ErrorMessage = "Số kết quả phải từ 1 đến 100")]
        public int MaxResults { get; set; } = 20;

        /// <summary>
        /// Tiện ích cần thiết (gần trường học, bệnh viện, siêu thị, etc.)
        /// </summary>
        public List<string> RequiredAmenities { get; set; } = new List<string>();
    }
} 