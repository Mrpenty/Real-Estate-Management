using static RealEstateManagement.Data.Entity.PropertyEntity.PropertyPost;

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

        public List<OwnerPropertyImageDto> Images { get; set; }
        public List<OwnerPropertyPostDto> Posts { get; set; }

        //them
        public string Type { get; set; }
        public int? ProvinceId { get; set; }
        public string Province { get; set; }
        public int? WardId { get; set; }
        public string Ward { get; set; }
        public int? StreetId { get; set; }
        public string Street { get; set; }
        public string DetailedAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public PropertyPostStatus Status { get; set; }
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
    }
}
