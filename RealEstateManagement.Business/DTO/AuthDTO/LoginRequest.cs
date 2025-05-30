using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.AuthDTO
{
    public class LoginRequest
    {
        public string LoginIdentifier { get; set; } 
        public string Password { get; set; }
    }
}
