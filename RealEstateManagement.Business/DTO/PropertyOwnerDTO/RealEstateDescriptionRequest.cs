public class RealEstateDescriptionRequest
{
    public string Title { get; set; }
    public string Address { get; set; }
    public string Type { get; set; }
    public decimal? Area { get; set; }
    public int? Bedrooms { get; set; }
    public decimal Price { get; set; }
    public List<int> AmenityIds { get; set; } = new();
    public int? ProvinceId { get; set; }
    public int? WardId { get; set; }
    public int? StreetId { get; set; }
    public string? DetailedAddress { get; set; }
}
