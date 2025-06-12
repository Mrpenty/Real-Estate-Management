using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.AuthDTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email or PhoneNumber Is Required")]
        public string LoginIdentifier { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }
    }
}
