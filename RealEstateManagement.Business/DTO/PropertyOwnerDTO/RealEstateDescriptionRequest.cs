using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class RealEstateDescriptionRequest
    {
        public string Title { get; set; }
        public string Address { get; set; }             // Địa chỉ chi tiết
        public string Type { get; set; } // Loại phòng cho thuê
        public decimal Area { get; set; }                   // Diện tích, đơn vị: m2
        public int Bedrooms { get; set; }
        public decimal Price { get; set; }              // Triệu VND
        public List<int> AmenityIds { get; set; }
        public int ProvinceId { get; set; }
        public int WardId { get; set; }
        public int StreetId { get; set; }
        public string DetailedAddress { get; set; }
    }

}
