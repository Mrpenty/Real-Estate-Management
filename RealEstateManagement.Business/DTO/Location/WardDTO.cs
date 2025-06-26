using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Location
{
    public class WardDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<StreetDTO> Streets { get; set; }
    }
}
