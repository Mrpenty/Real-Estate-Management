using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class    PropertyCreateRequestDto
    {
        public string? Title { get; set; }
        public string Description { get; set; }
        public int PropertyTypeId { get; set; } 
        public decimal Area { get; set; } 
        public int Bedrooms { get; set; } 
        public int Bathrooms { get; set; }

        public int Floors { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; } 
        public List<int> AmenityIds { get; set; }

        // Address fields
        public int? ProvinceId { get; set; }
        public int? WardId { get; set; }
        public int?  StreetId { get; set; }
        public string DetailedAddress { get; set; }

    }
}
