using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Business.DTO.PromotionPackageDTO
{
    public class CreatePromotionPackageDTO
    {
        [Required(ErrorMessage = "Tên gói khuyến mãi là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Thời gian hiệu lực là bắt buộc")]
        [Range(1, 365, ErrorMessage = "Thời gian hiệu lực phải từ 1 đến 365 ngày")]
        public int DurationInDays { get; set; }

        [Required(ErrorMessage = "Cấp độ là bắt buộc")]
        [Range(1, 10, ErrorMessage = "Cấp độ phải từ 1 đến 10")]
        public int Level { get; set; }

        public bool IsActive { get; set; } = true;

    
    }
} 