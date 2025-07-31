using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Properties
{
    public class UserProfileWithPropertiesDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public List<HomePropertyDTO> Properties { get; set; }
    }
}
