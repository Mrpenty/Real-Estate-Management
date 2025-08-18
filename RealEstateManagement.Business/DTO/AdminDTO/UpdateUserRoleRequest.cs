using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Business.DTO.AdminDTO
{
    public class UpdateUserRoleRequest
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [RegularExpression("^(Admin|Landlord|Renter)$", ErrorMessage = "Role must be Admin, Landlord, or Renter")]
        public string NewRole { get; set; }
    }
} 