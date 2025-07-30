using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Business.DTO.NotificationDTO
{
    public class CreateNotificationDTO
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = "info"; // info, warning, alert

        [Required]
        public string Audience { get; set; } = "all"; // all, renters, landlords, specific

        // Only used when Audience is "specific"
        public List<int>? SpecificUserIds { get; set; }
    }
} 