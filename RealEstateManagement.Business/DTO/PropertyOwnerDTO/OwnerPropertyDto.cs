using RealEstateManagement.Business.DTO.Properties;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class OwnerPropertyDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsPromoted { get; set; }
        public bool IsVerified { get; set; }
        public string Location { get; set; }
        public int Bedrooms { get; set; }
        public decimal Area { get; set; }

        public List<string> ImageUrls { get; set; } = new List<string>();
        public string? PrimaryImageUrl { get; set; }
        public List<OwnerPropertyImageDto> Images { get; set; } = new List<OwnerPropertyImageDto>();
        public List<OwnerPropertyPostDto> Posts { get; set; } = new List<OwnerPropertyPostDto>();
        public List<string> Amenities { get; set; } = new List<string>();

        public string Type { get; set; }
        public int? ProvinceId { get; set; }
        public string? Province { get; set; }
        public int? WardId { get; set; }
        public string? Ward { get; set; }
        public int? StreetId { get; set; }
        public string? Street { get; set; }
        public string? DetailedAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsExistRenterContract { get; set; }
        public int? RenterContractId { get; set; }
        public int? InterestNo { get; set; }
        public List<InterestedPropertyDTO> InterestedProperties { get; set; }
    }

    public class OwnerPropertyImageDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class OwnerPropertyPostDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public RentalContractViewDto? RentalContract { get; set; }
    }

}
