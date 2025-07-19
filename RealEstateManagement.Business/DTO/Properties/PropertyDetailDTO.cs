using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity.PropertyEntity;
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
        public string? Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int AddressID { get; set; }
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
        public string? PromotionPackageName { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> ImageUrls { get; set; }
        public List<PropertyImageCreateDto> Images { get; set; }
        // Contract Details
        public decimal? ContractDeposit { get; set; }
        public decimal? ContractMonthlyRent { get; set; }
        public int? ContractDurationMonths { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public string ContractStatus { get; set; }
        public string ContractPaymentMethod { get; set; }
        public string ContractContactInfo { get; set; }
        // thêm
        public string Province { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; }
        public string DetailedAddress { get; set; }
        public int? ProvinceId { get; set; }
        public int? WardId { get; set; }
        public int? StreetId { get; set; }
        public bool IsFavorite { get; set; }
    }
}
