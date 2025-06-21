using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Location
{
    public class ProvinceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<WardDTO> Wards { get; set; }
    }
}
