namespace RealEstateManagement.Business.DTO.Properties
{
    public class PropertySearchDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Area { get; set; }
        public int AddressId { get; set; } // ✅ Thêm dòng này
        public List<string> Amenities { get; set; }
        public int PackageLevel { get; set; }
        public int PromotionPackageLevel { get; set; }
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public string Location { get; set; }
    }

}
