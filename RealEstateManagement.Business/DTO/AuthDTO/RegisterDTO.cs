using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Business.DTO.AuthDTO
{
    public class RegisterDTO
    {
        [Required]
        public string Name { get; set; }

    
        public string Email { get; set; }

        [Required]
        
        public string? PhoneNumber { get; set; }

      

        [Required]
        public string Password { get; set; }


    }
}
