using RealEstateManagement.Business.DTO.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Interaction
{
    public class ProfileLandlordDTO
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
        public List<HomePropertyDTO> Properties { get; set; }
    }
}
