using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Properties
{
    public class PropertyDetailDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ViewsCount { get; set; }
        public string? PrimaryImageUrl { get; set; }
        public string LandlordName { get; set; }
        public string LandlordPhoneNumber { get; set; }
        public string LandlordProfilePictureUrl { get; set; }
        public DateTime LandlordCreatedAt { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}
