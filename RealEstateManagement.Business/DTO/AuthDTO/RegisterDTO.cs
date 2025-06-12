using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Business.DTO.AuthDTO
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

       

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        
        public string? PhoneNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
