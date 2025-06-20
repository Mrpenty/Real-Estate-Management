using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Business.DTO.UserDTO
{
    public class UpdateProfileDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        public string? ProfilePictureUrl { get; set; }

        
    }
}
