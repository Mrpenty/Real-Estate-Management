using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class OwnerUpdatePropertyDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
    }
}
