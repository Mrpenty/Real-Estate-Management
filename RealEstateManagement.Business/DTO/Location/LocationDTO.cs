using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Business.DTO.Location
{
    public class LocationDTO
    {
        [Required]
        public double Latitude { get; set; }
        
        [Required]
        public double Longitude { get; set; }
        
        public string? Address { get; set; }
        
        public string? City { get; set; }
        
        public string? District { get; set; }
        
        public string? Ward { get; set; }
        
        public double? RadiusKm { get; set; } = 10; // Bán kính tìm kiếm mặc định 10km
    }

    public class LocationRecommendationRequest
    {
        [Required]
        public double Latitude { get; set; }
        
        [Required]
        public double Longitude { get; set; }
        
        public double? RadiusKm { get; set; } = 10;
        
        public int? MaxResults { get; set; } = 20;
        
        public string? PropertyType { get; set; }
        
        public decimal? MaxPrice { get; set; }
        
        public decimal? MinPrice { get; set; }
        
        public int? MinBedrooms { get; set; }
        
        public decimal? MinArea { get; set; }
        
        public decimal? MaxArea { get; set; }
        
        public List<int>? AmenityIds { get; set; }
        
        public string? UserPreference { get; set; } // "budget_friendly", "luxury", "family_friendly", etc.
    }

    public class LocationRecommendationResponse
    {
        public List<PropertyRecommendationDTO> Properties { get; set; } = new();
        
        public LocationDTO UserLocation { get; set; }
        
        public double SearchRadiusKm { get; set; }
        
        public int TotalResults { get; set; }
        
        public string RecommendationReason { get; set; }
        
        public List<string> NearbyAmenities { get; set; } = new();
        
        public List<string> TransportationInfo { get; set; } = new();
    }

    public class PropertyRecommendationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string PrimaryImageUrl { get; set; }
        public string Province { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; }
        public string DetailedAddress { get; set; }
        
        // Location specific fields
        public double DistanceKm { get; set; }
        public int WalkingTimeMinutes { get; set; }
        public int DrivingTimeMinutes { get; set; }
        public double MatchScore { get; set; } // 0-100 score based on user preferences
        public string MatchReason { get; set; }
        public List<string> Amenities { get; set; } = new();
        public List<string> NearbyPoints { get; set; } = new(); // Schools, hospitals, malls, etc.
    }
} 