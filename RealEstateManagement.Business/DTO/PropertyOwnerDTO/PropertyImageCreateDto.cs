using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class PropertyImageCreateDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string Url { get; set; }
        public bool IsPrimary { get; set; }
        public int Order { get; set; }
    }

}
