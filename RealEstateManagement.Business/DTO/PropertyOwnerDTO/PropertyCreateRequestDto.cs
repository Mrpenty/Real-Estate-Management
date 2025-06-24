using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class PropertyCreateRequestDto
    {
        public string? Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // Loại phòng cho thuê
        public decimal Area { get; set; } // Bao nhiêu m2
        public int Bedrooms { get; set; } //Số phòng ngủ
        public decimal Price { get; set; }
        public string Location { get; set; } // Tọa độ
        public List<int> AmenityIds { get; set; }

        // Address fields
        public int ProvinceId { get; set; }
        public int WardId { get; set; }
        public string Street { get; set; }
    }
}
