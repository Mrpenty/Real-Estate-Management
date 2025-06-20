using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Properties
{
    public class ComparePropertyDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string PrimaryImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ViewsCount { get; set; }

        public decimal Price { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        //public int Bathrooms { get; set; } // nếu có

        public int AddressId { get; set; }
        public string Location { get; set; }

        public List<string> Amenities { get; set; }

        public string LandlordName { get; set; }
        public string LandlordPhoneNumber { get; set; }
        public string LandlordProfilePictureUrl { get; set; }

        public double? AverageRating { get; set; }
        public int TotalReviews { get; set; }

        //Xem bđs có cái nào nhất trong các bđs so sánh
        // Best value indicators for frontend
        public bool IsBestPrice { get; set; }        // Giá rẻ nhất
        public bool IsBestArea { get; set; }         // Diện tích lớn nhất
        public bool IsMostBedrooms { get; set; }     // Nhiều phòng ngủ nhất
        public bool IsBestRating { get; set; }       // Rating cao nhất
        public bool IsMostViewed { get; set; }       // Lượt xem nhiều nhất
        public bool IsMostReviewed { get; set; }     // Nhiều review nhất

    }
}
