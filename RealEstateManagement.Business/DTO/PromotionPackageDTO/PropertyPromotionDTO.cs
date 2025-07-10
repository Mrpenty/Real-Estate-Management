using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Business.DTO.PromotionPackageDTO
{
    public class CreatePropertyPromotionDTO
    {
        [Required]
        public int PropertyId { get; set; }
        [Required]
        public int PackageId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }

    public class UpdatePropertyPromotionDTO
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }

    public class ViewPropertyPromotionDTO
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int PackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}