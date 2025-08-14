using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RealEstateManagement.Business.DTO.Location;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services
{
    public interface IAIRecommendationService
    {
        Task<LocationRecommendationResponse> GetLocationBasedRecommendationsAsync(LocationRecommendationRequest request);
        Task<List<PropertyRecommendationDTO>> GetNearbyPropertiesAsync(double latitude, double longitude, double radiusKm, int maxResults = 20);
        Task<double> CalculateDistanceAsync(double lat1, double lon1, double lat2, double lon2);
        Task<string> GetAIRecommendationReasonAsync(LocationRecommendationRequest request, List<PropertyRecommendationDTO> properties);
        Task<List<string>> GetNearbyAmenitiesAsync(double latitude, double longitude, double radiusKm);
        Task<List<string>> GetTransportationInfoAsync(double latitude, double longitude);
    }

    public class AIRecommendationService : IAIRecommendationService
    {
        private readonly RentalDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _openAiApiKey;
        private readonly IGoogleMapsService _googleMapsService;

        public AIRecommendationService(RentalDbContext context, IConfiguration configuration, IGoogleMapsService googleMapsService)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = new HttpClient();
            _openAiApiKey = configuration["OpenAI:ApiKey"];
            _googleMapsService = googleMapsService;
        }

        public async Task<LocationRecommendationResponse> GetLocationBasedRecommendationsAsync(LocationRecommendationRequest request)
        {
            try
            {
                // Lấy danh sách bất động sản gần vị trí
                var nearbyProperties = await GetNearbyPropertiesAsync(
                    request.Latitude, 
                    request.Longitude, 
                    request.RadiusKm ?? 10, 
                    request.MaxResults ?? 20
                );

                // Lọc theo tiêu chí người dùng
                var filteredProperties = await FilterPropertiesByUserCriteriaAsync(nearbyProperties, request);

                // Tính điểm phù hợp cho từng bất động sản
                var scoredProperties = await CalculateMatchScoresAsync(filteredProperties, request);

                // Sắp xếp theo điểm phù hợp
                var sortedProperties = scoredProperties.OrderByDescending(p => p.MatchScore).ToList();

                // Lấy thông tin tiện ích gần đó
                var nearbyAmenities = await GetNearbyAmenitiesAsync(request.Latitude, request.Longitude, request.RadiusKm ?? 10);

                // Lấy thông tin giao thông
                var transportationInfo = await GetTransportationInfoAsync(request.Latitude, request.Longitude);

                // Tạo lý do recommendation bằng AI
                var recommendationReason = await GetAIRecommendationReasonAsync(request, sortedProperties);

                return new LocationRecommendationResponse
                {
                    Properties = sortedProperties,
                    UserLocation = new LocationDTO
                    {
                        Latitude = request.Latitude,
                        Longitude = request.Longitude,
                        RadiusKm = request.RadiusKm
                    },
                    SearchRadiusKm = request.RadiusKm ?? 10,
                    TotalResults = sortedProperties.Count,
                    RecommendationReason = recommendationReason,
                    NearbyAmenities = nearbyAmenities,
                    TransportationInfo = transportationInfo
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting location-based recommendations: {ex.Message}");
            }
        }

        public async Task<List<PropertyRecommendationDTO>> GetNearbyPropertiesAsync(double latitude, double longitude, double radiusKm, int maxResults = 20)
        {
            var properties = await _context.Properties
                .Include(p => p.Address)
                .ThenInclude(a => a.Province)
                .Include(p => p.Address)
                .ThenInclude(a => a.Ward)
                .Include(p => p.Address)
                .ThenInclude(a => a.Street)
                .Include(p => p.Images)
                .Include(p => p.PropertyAmenities)
                .ThenInclude(pa => pa.Amenity)
                .Include(p => p.Landlord)
                .Where(p => p.Status == "Available")
                .ToListAsync();

            var nearbyProperties = new List<PropertyRecommendationDTO>();

            foreach (var property in properties)
            {
                // Tính khoảng cách (sử dụng tọa độ mẫu - trong thực tế cần có tọa độ thực)
                var distance = await CalculateDistanceAsync(latitude, longitude, 
                    GetPropertyLatitude(property), GetPropertyLongitude(property));

                if (distance <= radiusKm)
                {
                    var propertyDto = new PropertyRecommendationDTO
                    {
                        Id = property.Id,
                        Title = property.Title,
                        Description = property.Description,
                        Type = property.Type,
                        Area = property.Area,
                        Bedrooms = property.Bedrooms,
                        Price = property.Price,
                        Status = property.Status,
                        PrimaryImageUrl = property.Images.FirstOrDefault()?.Url,
                        Province = property.Address?.Province?.Name,
                        Ward = property.Address?.Ward?.Name,
                        Street = property.Address?.Street?.Name,
                        DetailedAddress = property.Address?.DetailedAddress,
                        DistanceKm = distance,
                        WalkingTimeMinutes = (int)(distance * 15), // Ước tính thời gian đi bộ
                        DrivingTimeMinutes = (int)(distance * 3), // Ước tính thời gian lái xe
                        Amenities = property.PropertyAmenities.Select(pa => pa.Amenity.Description).ToList(),
                        NearbyPoints = await GetNearbyPointsAsync(GetPropertyLatitude(property), GetPropertyLongitude(property))
                    };

                    nearbyProperties.Add(propertyDto);
                }
            }

            return nearbyProperties.OrderBy(p => p.DistanceKm).Take(maxResults).ToList();
        }

        public async Task<double> CalculateDistanceAsync(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Bán kính Trái Đất (km)
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public async Task<string> GetAIRecommendationReasonAsync(LocationRecommendationRequest request, List<PropertyRecommendationDTO> properties)
        {
            if (string.IsNullOrEmpty(_openAiApiKey))
            {
                return "Dựa trên vị trí và tiêu chí tìm kiếm của bạn, chúng tôi đã tìm thấy những bất động sản phù hợp nhất.";
            }

            try
            {
                var prompt = BuildRecommendationPrompt(request, properties);
                
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new object[]
                    {
                        new { role = "system", content = "Bạn là chuyên gia tư vấn bất động sản. Hãy phân tích và đưa ra lý do tại sao những bất động sản này phù hợp với người dùng dựa trên vị trí và tiêu chí của họ." },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.7,
                    max_tokens = 300
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAiApiKey}");

                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic result = JsonConvert.DeserializeObject(responseString);
                    return result.choices[0].message.content.ToString();
                }
            }
            catch (Exception ex)
            {
                // Log error but don't fail the request
                Console.WriteLine($"OpenAI API error: {ex.Message}");
            }

            return "Dựa trên vị trí và tiêu chí tìm kiếm của bạn, chúng tôi đã tìm thấy những bất động sản phù hợp nhất.";
        }

        private string BuildRecommendationPrompt(LocationRecommendationRequest request, List<PropertyRecommendationDTO> properties)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Người dùng đang tìm kiếm bất động sản tại vị trí: {request.Latitude}, {request.Longitude}");
            builder.AppendLine($"Bán kính tìm kiếm: {request.RadiusKm}km");
            
            if (!string.IsNullOrEmpty(request.PropertyType))
                builder.AppendLine($"Loại bất động sản: {request.PropertyType}");
            
            if (request.MaxPrice.HasValue)
                builder.AppendLine($"Giá tối đa: {request.MaxPrice} triệu đồng/tháng");
            
            if (request.MinBedrooms.HasValue)
                builder.AppendLine($"Số phòng ngủ tối thiểu: {request.MinBedrooms}");
            
            if (!string.IsNullOrEmpty(request.UserPreference))
                builder.AppendLine($"Sở thích: {request.UserPreference}");
            
            builder.AppendLine($"\nĐã tìm thấy {properties.Count} bất động sản phù hợp:");
            
            foreach (var prop in properties.Take(5)) // Chỉ lấy 5 bất động sản đầu tiên để phân tích
            {
                builder.AppendLine($"- {prop.Title}: {prop.Type}, {prop.Bedrooms} phòng ngủ, {prop.Area}m², {prop.Price} triệu/tháng, cách {prop.DistanceKm:F1}km");
            }
            
            builder.AppendLine("\nHãy đưa ra lý do tại sao những bất động sản này phù hợp với người dùng, dựa trên vị trí, tiêu chí và sở thích của họ.");
            
            return builder.ToString();
        }

        public async Task<List<string>> GetNearbyAmenitiesAsync(double latitude, double longitude, double radiusKm)
        {
            try
            {
                var places = await _googleMapsService.GetNearbyPlacesAsync(latitude, longitude, radiusKm * 1000); // Convert to meters
                var amenities = new List<string>();
                
                if (places?.Results != null)
                {
                    foreach (var place in places.Results.Take(10)) // Limit to 10 results
                    {
                        amenities.Add($"{place.Name} - {place.Vicinity}");
                    }
                }
                
                // Fallback to sample data if API fails
                if (!amenities.Any())
                {
                    amenities = new List<string>
                    {
                        "Trường học",
                        "Bệnh viện",
                        "Siêu thị",
                        "Nhà hàng",
                        "Công viên",
                        "Trạm xe buýt",
                        "Ngân hàng"
                    };
                }
                
                return amenities;
            }
            catch (Exception ex)
            {
                // Log error but return sample data
                Console.WriteLine($"Error getting nearby amenities: {ex.Message}");
                return new List<string>
                {
                    "Trường học",
                    "Bệnh viện",
                    "Siêu thị",
                    "Nhà hàng",
                    "Công viên",
                    "Trạm xe buýt",
                    "Ngân hàng"
                };
            }
        }

        public async Task<List<string>> GetTransportationInfoAsync(double latitude, double longitude)
        {
            try
            {
                var transportationInfo = new List<string>();
                
                // Get nearby transit stations
                var transitPlaces = await _googleMapsService.GetNearbyPlacesAsync(latitude, longitude, 2000, "transit_station");
                if (transitPlaces?.Results != null)
                {
                    foreach (var place in transitPlaces.Results.Take(5))
                    {
                        var distance = await CalculateDistanceAsync(latitude, longitude, place.Geometry.Location.Lat, place.Geometry.Location.Lng);
                        transportationInfo.Add($"{place.Name} - cách {distance:F1}km");
                    }
                }
                
                // Get nearby bus stations
                var busStops = await _googleMapsService.GetNearbyPlacesAsync(latitude, longitude, 1000, "bus_station");
                if (busStops?.Results != null)
                {
                    foreach (var place in busStops.Results.Take(3))
                    {
                        var distance = await CalculateDistanceAsync(latitude, longitude, place.Geometry.Location.Lat, place.Geometry.Location.Lng);
                        transportationInfo.Add($"Trạm xe buýt {place.Name} - cách {distance:F1}km");
                    }
                }
                
                // Fallback to sample data if API fails
                if (!transportationInfo.Any())
                {
                    transportationInfo = new List<string>
                    {
                        "Trạm xe buýt cách 200m",
                        "Tàu điện ngầm cách 500m",
                        "Bãi đỗ xe công cộng cách 300m"
                    };
                }
                
                return transportationInfo;
            }
            catch (Exception ex)
            {
                // Log error but return sample data
                Console.WriteLine($"Error getting transportation info: {ex.Message}");
                return new List<string>
                {
                    "Trạm xe buýt cách 200m",
                    "Tàu điện ngầm cách 500m",
                    "Bãi đỗ xe công cộng cách 300m"
                };
            }
        }

        private async Task<List<PropertyRecommendationDTO>> FilterPropertiesByUserCriteriaAsync(List<PropertyRecommendationDTO> properties, LocationRecommendationRequest request)
        {
            var filtered = properties.AsQueryable();

            if (!string.IsNullOrEmpty(request.PropertyType))
                filtered = filtered.Where(p => p.Type.Contains(request.PropertyType));

            if (request.MaxPrice.HasValue)
                filtered = filtered.Where(p => p.Price <= request.MaxPrice.Value);

            if (request.MinPrice.HasValue)
                filtered = filtered.Where(p => p.Price >= request.MinPrice.Value);

            if (request.MinBedrooms.HasValue)
                filtered = filtered.Where(p => p.Bedrooms >= request.MinBedrooms.Value);

            if (request.MinArea.HasValue)
                filtered = filtered.Where(p => p.Area >= request.MinArea.Value);

            if (request.MaxArea.HasValue)
                filtered = filtered.Where(p => p.Area <= request.MaxArea.Value);

            return await Task.FromResult(filtered.ToList());
        }

        private async Task<List<PropertyRecommendationDTO>> CalculateMatchScoresAsync(List<PropertyRecommendationDTO> properties, LocationRecommendationRequest request)
        {
            foreach (var property in properties)
            {
                double score = 100;

                // Giảm điểm dựa trên khoảng cách
                if (property.DistanceKm > 5)
                    score -= (property.DistanceKm - 5) * 2;

                // Tăng điểm nếu phù hợp với sở thích
                if (!string.IsNullOrEmpty(request.UserPreference))
                {
                    switch (request.UserPreference.ToLower())
                    {
                        case "budget_friendly":
                            if (property.Price <= 15) score += 20;
                            break;
                        case "luxury":
                            if (property.Price >= 30) score += 20;
                            break;
                        case "family_friendly":
                            if (property.Bedrooms >= 3) score += 15;
                            break;
                    }
                }

                // Tăng điểm nếu có nhiều tiện ích
                score += property.Amenities.Count * 2;

                // Đảm bảo điểm không âm
                property.MatchScore = Math.Max(0, Math.Min(100, score));
                property.MatchReason = GetMatchReason(property.MatchScore, property.DistanceKm, property.Price);
            }

            return properties;
        }

        private string GetMatchReason(double score, double distance, decimal price)
        {
            if (score >= 90) return "Phù hợp hoàn hảo với tiêu chí của bạn";
            if (score >= 80) return "Rất phù hợp với nhu cầu";
            if (score >= 70) return "Phù hợp với yêu cầu cơ bản";
            if (score >= 60) return "Đáp ứng phần lớn tiêu chí";
            return "Có thể phù hợp với một số tiêu chí";
        }

        private async Task<List<string>> GetNearbyPointsAsync(double latitude, double longitude)
        {
            // Trong thực tế, bạn sẽ gọi API để lấy thông tin điểm gần đó
            return new List<string>
            {
                "Trường tiểu học ABC",
                "Bệnh viện XYZ",
                "Siêu thị Metro"
            };
        }

        // Hàm tạm thời để lấy tọa độ - trong thực tế cần có tọa độ thực trong database
        private double GetPropertyLatitude(Property property)
        {
            // Sử dụng tọa độ mẫu cho Hà Nội
            return 21.0285 + (property.Id % 100) * 0.001;
        }

        private double GetPropertyLongitude(Property property)
        {
            // Sử dụng tọa độ mẫu cho Hà Nội
            return 105.8542 + (property.Id % 100) * 0.001;
        }
    }
} 