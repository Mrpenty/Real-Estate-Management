using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Properties
{
    public class HomepagePropertyResponseDTO
    {
        public List<HomePropertyDTO> AllProperties { get; set; }
        public List<HomePropertyDTO> PromotedProperties { get; set; }
    }
}
