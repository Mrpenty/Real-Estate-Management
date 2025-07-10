using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class ContinuePropertyPostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }
        public int ProvinceId { get; set; }
        public int WardId { get; set; }
        public int StreetId { get; set; }
        public string DetailedAddress { get; set; }
        public List<int> AmenityIds { get; set; }

        public List<PropertyImageCreateDto> Images { get; set; }
    }

}
