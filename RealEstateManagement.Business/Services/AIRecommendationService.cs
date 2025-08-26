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
        Task<LocationRecommendationResponse> GetRecommendationsByCriteriaAsync(PropertySearchCriteriaRequest request);
        Task<List<PropertyRecommendationDTO>> GetNearbyPropertiesAsync(double latitude, double longitude, double radiusKm, int maxResults = 20, string searchLocation = null);
        Task<double> CalculateDistanceAsync(double lat1, double lon1, double lat2, double lon2);
        Task<string> GetAIRecommendationReasonAsync(LocationRecommendationRequest request, List<PropertyRecommendationDTO> properties);
        Task<string> GetAIRecommendationReasonAsync(PropertySearchCriteriaRequest request, List<PropertyRecommendationDTO> properties);
        Task<List<string>> GetNearbyAmenitiesAsync(double latitude, double longitude, double radiusKm);
        Task<List<DetailedAmenityDTO>> GetDetailedAmenitiesAsync(double latitude, double longitude, double radiusKm);
        Task<List<string>> GetTransportationInfoAsync(double latitude, double longitude);
    }

    public class AIRecommendationService : IAIRecommendationService
    {
        private readonly RentalDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _openAiApiKey;
        private readonly IOpenStreetMapService _openStreetMapService;

        public AIRecommendationService(RentalDbContext context, IConfiguration configuration, IOpenStreetMapService openStreetMapService)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = new HttpClient();
            _openAiApiKey = configuration["OpenAI:ApiKey"];
            _openStreetMapService = openStreetMapService;
            
            Console.WriteLine($"AIRecommendationService initialized. OpenStreetMap service: {_openStreetMapService != null}");
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
                    request.MaxResults ?? 20,
                    null // Không có searchLocation cho location-based search
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
                    SearchLocation = "Vị trí hiện tại",
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

        public async Task<LocationRecommendationResponse> GetRecommendationsByCriteriaAsync(PropertySearchCriteriaRequest request)
        {
            try
            {
                Console.WriteLine("=== AI Recommendation Request ===");
                Console.WriteLine($"Search Location: {request.SearchLocation}");
                Console.WriteLine($"Property Type: {request.PropertyType}");
                Console.WriteLine($"Price Range: {request.MinPrice} - {request.MaxPrice} trieu");
                Console.WriteLine($"Bedrooms: {request.MinBedrooms}");
                Console.WriteLine($"Area: {request.MinArea} - {request.MaxArea} m2");
                
                // Lấy tọa độ từ địa chỉ tìm kiếm
                var searchLocation = await GetCoordinatesFromAddressAsync(request.SearchLocation);
                if (searchLocation == null)
                {
                    throw new Exception("Khong the xac dinh toa do tu dia chi tim kiem");
                }
                
                Console.WriteLine($"Resolved coordinates: ({searchLocation.Latitude}, {searchLocation.Longitude}) - {searchLocation.Address}");

                // Lấy danh sách bất động sản gần vị trí tìm kiếm (sử dụng bán kính cố định 10km)
                var nearbyProperties = await GetNearbyPropertiesAsync(
                    searchLocation.Latitude, 
                    searchLocation.Longitude, 
                    10.0, // Bán kính cố định 10km
                    request.MaxResults,
                    request.SearchLocation
                );

                // Lọc theo tiêu chí người dùng
                var filteredProperties = await FilterPropertiesByCriteriaAsync(nearbyProperties, request);

                // Tính điểm phù hợp cho từng bất động sản
                var scoredProperties = await CalculateMatchScoresByCriteriaAsync(filteredProperties, request);

                // Sắp xếp theo điểm phù hợp
                var sortedProperties = scoredProperties.OrderByDescending(p => p.MatchScore).ToList();

                // Lấy thông tin tiện ích và giao thông riêng cho từng BDS
                var propertiesWithAmenities = await GetPropertiesWithIndividualAmenitiesAsync(sortedProperties);

                // Thống kê tiện ích xung quanh các BDS
                var allAmenities = propertiesWithAmenities.SelectMany(p => p.NearbyAmenities).ToList();
                var amenityStats = AnalyzeAmenityStatistics(allAmenities);

                // Tạo lý do recommendation bằng AI
                var recommendationReason = await GetAIRecommendationReasonAsync(request, sortedProperties);

                return new LocationRecommendationResponse
                {
                    Properties = propertiesWithAmenities, // Sử dụng BDS đã có tiện ích riêng
                    SearchLocation = request.SearchLocation,
                    TotalResults = propertiesWithAmenities.Count,
                    RecommendationReason = recommendationReason,
                    // Bỏ tiện ích và giao thông tổng quan
                    NearbyAmenities = new List<string>(),
                    TransportationInfo = new List<string>(),
                    DetailedAmenities = new List<DetailedAmenityDTO>(),
                    AmenityStatistics = "",
                    DetailedAmenityStatistics = ""
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetRecommendationsByCriteriaAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw new Exception($"Error getting recommendations by criteria: {ex.Message}");
            }
        }

        public async Task<List<PropertyRecommendationDTO>> GetNearbyPropertiesAsync(double latitude, double longitude, double radiusKm, int maxResults = 20, string searchLocation = null)
        {
            var properties = await _context.Properties
                .Include(p => p.Address)
                .ThenInclude(a => a.Province)
                .Include(p => p.Address)
                .ThenInclude(a => a.Ward)
                .Include(p => p.Address)
                .ThenInclude(a => a.Street)
                .Include(p => p.Images)
                .Include(p => p.PropertyType) // Thêm PropertyType Include
                .Include(p => p.PropertyAmenities)
                .ThenInclude(pa => pa.Amenity)
                .Include(p => p.Landlord)
                .ToListAsync();

            Console.WriteLine($"Found {properties.Count} properties in database");

            // Lọc theo province nếu có searchLocation
            if (!string.IsNullOrEmpty(searchLocation))
            {
                var targetProvince = GetTargetProvince(searchLocation);
                Console.WriteLine($"Search location: '{searchLocation}' -> Target province: '{targetProvince}'");
                
                if (!string.IsNullOrEmpty(targetProvince))
                {
                    var originalCount = properties.Count;
                    
                    // Debug: Log tất cả provinces trong database
                    var allProvinces = properties.Select(p => p.Address?.Province?.Name).Where(p => !string.IsNullOrEmpty(p)).Distinct().ToList();
                    Console.WriteLine($"All provinces in database: {string.Join(", ", allProvinces)}");
                    
                    properties = properties.Where(p => 
                        p.Address?.Province?.Name != null && 
                        p.Address.Province.Name.ToLower().Contains(targetProvince.ToLower())
                    ).ToList();
                    Console.WriteLine($"Filtered by province '{targetProvince}': {properties.Count} properties (removed {originalCount - properties.Count})");
                    
                    // Debug: Log properties sau khi filter
                    foreach (var prop in properties.Take(5))
                    {
                        Console.WriteLine($"Property {prop.Id}: {prop.Title} - Province: {prop.Address?.Province?.Name}");
                    }
                }
            }

            var nearbyProperties = new List<PropertyRecommendationDTO>();

            Console.WriteLine($"Checking {properties.Count} properties within {radiusKm}km radius from ({latitude}, {longitude})");
            
            foreach (var property in properties)
            {
                // Tính khoảng cách (sử dụng tọa độ mẫu - trong thực tế cần có tọa độ thực)
                var propertyLat = GetPropertyLatitude(property);
                var propertyLon = GetPropertyLongitude(property);
                var distance = await CalculateDistanceAsync(latitude, longitude, propertyLat, propertyLon);

                Console.WriteLine($"Property {property.Id}: {property.Title} - Coordinates: ({propertyLat}, {propertyLon}) - Distance: {distance:F2}km, Radius: {radiusKm}km");

                if (distance <= radiusKm)
                {
                    var propertyDto = new PropertyRecommendationDTO
                    {
                        Id = property.Id,
                        Title = property.Title,
                        Description = property.Description,
                        Type = property.PropertyType?.Name ?? "Không xác định",
                        Area = property.Area,
                        Bedrooms = property.Bedrooms,
                        Price = property.Price,
                        Status = property.Status,
                        PrimaryImageUrl = property.Images?.FirstOrDefault()?.Url,
                        Province = property.Address?.Province?.Name,
                        Ward = property.Address?.Ward?.Name,
                        Street = property.Address?.Street?.Name,
                        DetailedAddress = property.Address?.DetailedAddress,
                        Latitude = GetPropertyLatitude(property),
                        Longitude = GetPropertyLongitude(property),
                        DistanceKm = distance,
                        WalkingTimeMinutes = (int)(distance * 15), // Ước tính thời gian đi bộ
                        DrivingTimeMinutes = (int)(distance * 3), // Ước tính thời gian lái xe
                        Amenities = property.PropertyAmenities?.Select(pa => pa.Amenity?.Description).Where(d => !string.IsNullOrEmpty(d)).ToList() ?? new List<string>(),
                        NearbyPoints = new List<string>() // Bỏ sample data
                    };
                    
                    // Debug: Log property type information
                    Console.WriteLine($"  Created DTO for Property {property.Id}: Type='{propertyDto.Type}' from PropertyType.Name='{property.PropertyType?.Name}'");

                    nearbyProperties.Add(propertyDto);
                    Console.WriteLine($"Added property {property.Id} to nearby list");
                }
            }

            Console.WriteLine($"Total nearby properties found: {nearbyProperties.Count}");
            
            // Nếu không có properties nào, trả về danh sách rỗng
            if (!nearbyProperties.Any())
            {
                Console.WriteLine("No properties found in the specified area");
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

        public async Task<string> GetAIRecommendationReasonAsync(PropertySearchCriteriaRequest request, List<PropertyRecommendationDTO> properties)
        {
            if (string.IsNullOrEmpty(_openAiApiKey))
            {
                return "Dựa trên tiêu chí tìm kiếm của bạn, chúng tôi đã tìm thấy những bất động sản phù hợp nhất.";
            }

            try
            {
                var prompt = BuildRecommendationPromptByCriteria(request, properties);
                
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new object[]
                    {
                        new { role = "system", content = "Bạn là chuyên gia tư vấn bất động sản. Hãy phân tích và đưa ra lý do tại sao những bất động sản này phù hợp với người dùng dựa trên tiêu chí tìm kiếm của họ." },
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

            return "Dựa trên tiêu chí tìm kiếm của bạn, chúng tôi đã tìm thấy những bất động sản phù hợp nhất.";
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
            
            // Sở thích đã được bỏ
            
            builder.AppendLine($"\nĐã tìm thấy {properties.Count} bất động sản phù hợp:");
            
            foreach (var prop in properties.Take(5)) // Chỉ lấy 5 bất động sản đầu tiên để phân tích
            {
                builder.AppendLine($"- {prop.Title}: {prop.Type}, {prop.Bedrooms} phòng ngủ, {prop.Area}m², {prop.Price} triệu/tháng, cách {prop.DistanceKm:F1}km");
            }
            
            builder.AppendLine("\nHãy đưa ra lý do tại sao những bất động sản này phù hợp với người dùng, dựa trên vị trí, tiêu chí và sở thích của họ.");
            
            return builder.ToString();
        }

        private string BuildRecommendationPromptByCriteria(PropertySearchCriteriaRequest request, List<PropertyRecommendationDTO> properties)
        {
            var builder = new StringBuilder();
            builder.AppendLine($"=== PHÂN TÍCH BẤT ĐỘNG SẢN PHÙ HỢP ===");
            builder.AppendLine($"Khu vực tìm kiếm: {request.SearchLocation}");
            builder.AppendLine($"Bán kính: 10km (cố định)");
            
            if (!string.IsNullOrEmpty(request.PropertyType))
                builder.AppendLine($"Loại BDS: {request.PropertyType}");
            
            if (request.MinPrice.HasValue || request.MaxPrice.HasValue)
                builder.AppendLine($"Khoảng giá: {request.MinPrice ?? 0} - {request.MaxPrice ?? 1000} triệu/tháng");
            
            if (request.MinBedrooms.HasValue)
                builder.AppendLine($"Số phòng ngủ: Tối thiểu {request.MinBedrooms} phòng");
            
            if (request.MinArea.HasValue || request.MaxArea.HasValue)
                builder.AppendLine($"Diện tích: {request.MinArea ?? 0} - {request.MaxArea ?? 1000}m²");
            
            if (request.RequiredAmenities.Any())
                builder.AppendLine($"Tiện ích yêu cầu: {string.Join(", ", request.RequiredAmenities)}");
            
            builder.AppendLine($"\n=== TOP {Math.Min(5, properties.Count)} BẤT ĐỘNG SẢN PHÙ HỢP NHẤT ===");
            
            foreach (var prop in properties.Take(5).OrderByDescending(p => p.MatchScore))
            {
                builder.AppendLine($"🏠 {prop.Title}");
                builder.AppendLine($"   • Loại: {prop.Type}");
                builder.AppendLine($"   • Phòng ngủ: {prop.Bedrooms} phòng");
                builder.AppendLine($"   • Diện tích: {prop.Area}m²");
                builder.AppendLine($"   • Giá: {prop.Price/1000000:F1} triệu/tháng");
                builder.AppendLine($"   • Khoảng cách: {prop.DistanceKm:F1}km");
                builder.AppendLine($"   • Điểm phù hợp: {prop.MatchScore:F1}/100");
                builder.AppendLine($"   • Lý do: {prop.MatchReason}");
                builder.AppendLine();
            }
            
            builder.AppendLine("=== YÊU CẦU PHÂN TÍCH ===");
            builder.AppendLine("Hãy phân tích và giải thích:");
            builder.AppendLine("1. Tại sao những BDS này được xếp hạng cao nhất?");
            builder.AppendLine("2. So sánh ưu điểm của từng BDS với các BDS khác");
            builder.AppendLine("3. Đưa ra lời khuyên cụ thể cho người dùng");
            builder.AppendLine("4. Giải thích tại sao BDS này phù hợp với tiêu chí tìm kiếm");
            
            return builder.ToString();
        }

        public async Task<List<string>> GetNearbyAmenitiesAsync(double latitude, double longitude, double radiusKm)
        {
            try
            {
                // Sử dụng OpenStreetMap service để lấy tiện ích gần đó
                var detailedAmenities = await _openStreetMapService.GetNearbyAmenitiesAsync(latitude, longitude, radiusKm);
                var amenities = new List<string>();
                
                if (detailedAmenities.Any())
                {
                    foreach (var amenity in detailedAmenities.Take(10)) // Limit to 10 results
                    {
                        amenities.Add($"{amenity.Name} - cách {amenity.DistanceKm:F1}km");
                    }
                }
                
                // Fallback to sample data if no results
                if (!amenities.Any())
                {
                    amenities = new List<string>
                    {
                        "Trường học - cách 0.5km",
                        "Bệnh viện - cách 1.2km",
                        "Siêu thị - cách 0.8km",
                        "Nhà hàng - cách 0.3km",
                        "Công viên - cách 0.6km",
                        "Trạm xe buýt - cách 0.2km",
                        "Ngân hàng - cách 1.0km"
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
                    "Trường học - cách 0.5km",
                    "Bệnh viện - cách 1.2km",
                    "Siêu thị - cách 0.8km",
                    "Nhà hàng - cách 0.3km",
                    "Công viên - cách 0.6km",
                    "Trạm xe buýt - cách 0.2km",
                    "Ngân hàng - cách 1.0km"
                };
            }
        }

        public async Task<List<DetailedAmenityDTO>> GetDetailedAmenitiesAsync(double latitude, double longitude, double radiusKm)
        {
            try
            {
                // Sử dụng OpenStreetMap service để lấy tiện ích gần đó
                var detailedAmenities = await _openStreetMapService.GetNearbyAmenitiesAsync(latitude, longitude, radiusKm);
                
                if (detailedAmenities.Any())
                {
                    return detailedAmenities;
                }

                // Fallback to sample data if no results
                return GetSampleDetailedAmenities(latitude, longitude);
            }
            catch (Exception ex)
            {
                // Log error but return sample data
                Console.WriteLine($"Error getting detailed amenities: {ex.Message}");
                return GetSampleDetailedAmenities(latitude, longitude);
            }
        }

        private string GetAmenityTypeName(string type)
        {
            return type switch
            {
                "restaurant" => "Nhà hàng",
                "school" => "Trường học",
                "hospital" => "Bệnh viện",
                "shopping_mall" => "Trung tâm mua sắm",
                "supermarket" => "Siêu thị",
                "bank" => "Ngân hàng",
                "bus_station" => "Trạm xe buýt",
                "subway_station" => "Trạm tàu điện",
                "park" => "Công viên",
                "gym" => "Phòng tập gym",
                "pharmacy" => "Nhà thuốc",
                "convenience_store" => "Cửa hàng tiện lợi",
                "atm" => "Máy ATM",
                "post_office" => "Bưu điện",
                "police" => "Đồn cảnh sát",
                _ => "Tiện ích khác"
            };
        }

        private string GetAmenityDescription(string type, string name)
        {
            return type switch
            {
                "restaurant" => $"Nhà hàng {name} - nơi thưởng thức ẩm thực ngon",
                "school" => $"Trường học {name} - môi trường giáo dục chất lượng",
                "hospital" => $"Bệnh viện {name} - chăm sóc sức khỏe",
                "shopping_mall" => $"Trung tâm mua sắm {name} - mua sắm tiện lợi",
                "supermarket" => $"Siêu thị {name} - mua sắm hàng ngày",
                "bank" => $"Ngân hàng {name} - dịch vụ tài chính",
                "bus_station" => $"Trạm xe buýt {name} - giao thông công cộng",
                "subway_station" => $"Trạm tàu điện {name} - giao thông nhanh",
                "park" => $"Công viên {name} - không gian xanh",
                "gym" => $"Phòng tập {name} - rèn luyện sức khỏe",
                "pharmacy" => $"Nhà thuốc {name} - chăm sóc sức khỏe",
                "convenience_store" => $"Cửa hàng tiện lợi {name} - mua sắm nhanh",
                "atm" => $"Máy ATM {name} - rút tiền tiện lợi",
                "post_office" => $"Bưu điện {name} - dịch vụ bưu chính",
                "police" => $"Đồn cảnh sát {name} - an ninh trật tự",
                _ => $"Tiện ích {name}"
            };
        }

        private List<DetailedAmenityDTO> GetSampleDetailedAmenities(double latitude, double longitude)
        {
            // Tạo dữ liệu mẫu với tọa độ gần vị trí tìm kiếm
            var sampleAmenities = new List<DetailedAmenityDTO>
            {
                new DetailedAmenityDTO
                {
                    Name = "Trường Tiểu học ABC",
                    Type = "Trường học",
                    Address = "123 Đường ABC, Quận 1",
                    DistanceKm = 0.3,
                    WalkingTimeMinutes = 5,
                    DrivingTimeMinutes = 2,
                    Rating = 4.5,
                    UserRatingsTotal = 120,
                    Latitude = latitude + 0.001,
                    Longitude = longitude + 0.001,
                    Description = "Trường tiểu học chất lượng cao"
                },
                new DetailedAmenityDTO
                {
                    Name = "Siêu thị Metro",
                    Type = "Siêu thị",
                    Address = "456 Đường XYZ, Quận 1",
                    DistanceKm = 0.5,
                    WalkingTimeMinutes = 8,
                    DrivingTimeMinutes = 3,
                    Rating = 4.2,
                    UserRatingsTotal = 89,
                    Latitude = latitude - 0.001,
                    Longitude = longitude + 0.002,
                    Description = "Siêu thị lớn với đầy đủ hàng hóa"
                },
                new DetailedAmenityDTO
                {
                    Name = "Bệnh viện Đa khoa",
                    Type = "Bệnh viện",
                    Address = "789 Đường DEF, Quận 1",
                    DistanceKm = 0.8,
                    WalkingTimeMinutes = 12,
                    DrivingTimeMinutes = 4,
                    Rating = 4.7,
                    UserRatingsTotal = 156,
                    Latitude = latitude + 0.002,
                    Longitude = longitude - 0.001,
                    Description = "Bệnh viện đa khoa uy tín"
                }
            };

            return sampleAmenities;
        }

        public async Task<List<string>> GetTransportationInfoAsync(double latitude, double longitude)
        {
            try
            {
                // Sử dụng OpenStreetMap service để lấy thông tin giao thông
                var transportationInfo = await _openStreetMapService.GetTransportationInfoAsync(latitude, longitude);
                
                if (transportationInfo.Any())
                {
                return transportationInfo;
                }

                // Fallback to sample data if no results
                return new List<string>
                    {
                        "Trạm xe buýt cách 200m",
                        "Tàu điện ngầm cách 500m",
                        "Bãi đỗ xe công cộng cách 300m"
                    };
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

        private async Task<List<PropertyRecommendationDTO>> FilterPropertiesByCriteriaAsync(List<PropertyRecommendationDTO> properties, PropertySearchCriteriaRequest request)
        {
            var filtered = properties.AsQueryable();

                            Console.WriteLine($"Filtering {properties.Count} properties with criteria:");
                Console.WriteLine($"PropertyType: '{request.PropertyType}' (length: {request.PropertyType?.Length ?? 0})");
                Console.WriteLine($"Price: {request.MinPrice} - {request.MaxPrice}");
                Console.WriteLine($"Bedrooms: {request.MinBedrooms}");
                Console.WriteLine($"Area: {request.MinArea} - {request.MaxArea}");
                
                // Debug: Log first few properties to see their types
                Console.WriteLine("Sample properties and their types:");
                foreach (var prop in properties.Take(3))
                {
                    Console.WriteLine($"  Property {prop.Id}: Type='{prop.Type}' (length: {prop.Type?.Length ?? 0})");
                }

            // Lọc theo loại bất động sản (nếu có)
            if (!string.IsNullOrEmpty(request.PropertyType))
            {
                var originalCount = filtered.Count();
                var searchType = request.PropertyType.ToLower().Trim();
                
                // Debug: Log the search type and available types
                var availableTypes = filtered.Select(p => p.Type).Where(t => !string.IsNullOrEmpty(t)).Distinct().ToList();
                Console.WriteLine($"Searching for PropertyType: '{request.PropertyType}' (normalized: '{searchType}')");
                Console.WriteLine($"Available types in database: {string.Join(", ", availableTypes)}");
                
                // More flexible matching logic
                filtered = filtered.Where(p => !string.IsNullOrEmpty(p.Type) && 
                    (p.Type.ToLower().Contains(searchType) || 
                     searchType.Contains(p.Type.ToLower()) ||
                     p.Type.ToLower().Replace(" ", "").Contains(searchType.Replace(" ", "")) ||
                     searchType.Replace(" ", "").Contains(p.Type.ToLower().Replace(" ", ""))));
                
                Console.WriteLine($"After PropertyType filter: {filtered.Count()} (removed {originalCount - filtered.Count()})");
            }

            // Lọc theo giá (nếu có)
            if (request.MaxPrice.HasValue && request.MaxPrice.Value > 0)
            {
                var originalCount = filtered.Count();
                filtered = filtered.Where(p => p.Price <= request.MaxPrice.Value * 1000000); // Convert to VND
                Console.WriteLine($"After MaxPrice filter: {filtered.Count()} (removed {originalCount - filtered.Count()})");
            }

            if (request.MinPrice.HasValue && request.MinPrice.Value > 0)
            {
                var originalCount = filtered.Count();
                filtered = filtered.Where(p => p.Price >= request.MinPrice.Value * 1000000); // Convert to VND
                Console.WriteLine($"After MinPrice filter: {filtered.Count()} (removed {originalCount - filtered.Count()})");
            }

            // Lọc theo số phòng ngủ (nếu có)
            if (request.MinBedrooms.HasValue && request.MinBedrooms.Value > 0)
            {
                var originalCount = filtered.Count();
                filtered = filtered.Where(p => p.Bedrooms >= request.MinBedrooms.Value);
                Console.WriteLine($"After MinBedrooms filter: {filtered.Count()} (removed {originalCount - filtered.Count()})");
            }

            // Lọc theo diện tích (nếu có)
            if (request.MinArea.HasValue && request.MinArea.Value > 0)
            {
                var originalCount = filtered.Count();
                filtered = filtered.Where(p => p.Area >= (decimal)request.MinArea.Value);
                Console.WriteLine($"After MinArea filter: {filtered.Count()} (removed {originalCount - filtered.Count()})");
            }

            if (request.MaxArea.HasValue && request.MaxArea.Value > 0)
            {
                var originalCount = filtered.Count();
                filtered = filtered.Where(p => p.Area <= (decimal)request.MaxArea.Value);
                Console.WriteLine($"After MaxArea filter: {filtered.Count()} (removed {originalCount - filtered.Count()})");
            }

            var result = await Task.FromResult(filtered.ToList());
            Console.WriteLine($"Final filtered result: {result.Count} properties");
            
            return result;
        }

        private async Task<List<PropertyRecommendationDTO>> CalculateMatchScoresAsync(List<PropertyRecommendationDTO> properties, LocationRecommendationRequest request)
        {
            foreach (var property in properties)
            {
                double score = 100;

                // Giảm điểm dựa trên khoảng cách
                if (property.DistanceKm > 5)
                    score -= (property.DistanceKm - 5) * 2;

                // Tăng điểm nếu phù hợp với sở thích - ĐÃ BỎ
                // Không còn sử dụng UserPreference

                // Tăng điểm nếu có nhiều tiện ích
                score += (property.Amenities?.Count ?? 0) * 2;

                // Đảm bảo điểm không âm
                property.MatchScore = Math.Max(0, Math.Min(100, score));
                property.MatchReason = GetMatchReason(property.MatchScore, property.DistanceKm, property.Price);
            }

            return properties;
        }

        private async Task<List<PropertyRecommendationDTO>> CalculateMatchScoresByCriteriaAsync(List<PropertyRecommendationDTO> properties, PropertySearchCriteriaRequest request)
        {
            Console.WriteLine($"=== Calculating Match Scores for {properties.Count} properties ===");
            
            foreach (var property in properties)
            {
                double score = 100;
                var reasons = new List<string>();

                        // 1. Điểm cơ bản theo khoảng cách (30% trọng số)
        var distanceScore = CalculateDistanceScore(property.DistanceKm);
        score = score * 0.7 + distanceScore * 0.3;
        reasons.Add($"Khoảng cách: {property.DistanceKm:F1}km (điểm: {distanceScore:F1})");

                // 2. Điểm theo giá cả (25% trọng số)
                if (request.MinPrice.HasValue || request.MaxPrice.HasValue)
                {
                    var priceScore = CalculatePriceScore(
                        property.Price,
                        request.MinPrice.HasValue ? (double?)((double)request.MinPrice.Value) : null,
                        request.MaxPrice.HasValue ? (double?)((double)request.MaxPrice.Value) : null
                    );
                    score = score * 0.75 + priceScore * 0.25;
                    reasons.Add($"Giá cả: {property.Price/1000000:F1} triệu (điểm: {priceScore:F1})");
                }

                // 3. Điểm theo số phòng ngủ (20% trọng số)
                if (request.MinBedrooms.HasValue)
                {
                    var bedroomScore = CalculateBedroomScore(property.Bedrooms, request.MinBedrooms.Value);
                    score = score * 0.8 + bedroomScore * 0.2;
                    reasons.Add($"Phòng ngủ: {property.Bedrooms} phòng (điểm: {bedroomScore:F1})");
                }

                // 4. Điểm theo diện tích (15% trọng số)
                if (request.MinArea.HasValue || request.MaxArea.HasValue)
                {
                    var areaScore = CalculateAreaScore(property.Area, request.MinArea, request.MaxArea);
                    score = score * 0.85 + areaScore * 0.15;
                    reasons.Add($"Diện tích: {property.Area}m² (điểm: {areaScore:F1})");
                }

                // 5. Điểm theo loại bất động sản (10% trọng số)
                if (!string.IsNullOrEmpty(request.PropertyType))
                {
                    var typeScore = CalculateTypeScore(property.Type, request.PropertyType);
                    score = score * 0.9 + typeScore * 0.1;
                    reasons.Add($"Loại BDS: {property.Type ?? "N/A"} (điểm: {typeScore:F1})");
                }

                // 6. Điểm bonus theo sở thích người dùng - ĐÃ BỎ
                // Không còn sử dụng UserPreference

                // 7. Điểm bonus theo tiện ích yêu cầu
                if (request.RequiredAmenities.Any())
                {
                    var amenityBonus = CalculateAmenityBonus(property, request.RequiredAmenities);
                    score += amenityBonus;
                    if (amenityBonus > 0)
                        reasons.Add($"Tiện ích yêu cầu: +{amenityBonus:F1} điểm");
                }

                // Đảm bảo điểm không âm và không vượt quá 100
                property.MatchScore = Math.Max(0, Math.Min(100, score));
                property.MatchReason = string.Join(" | ", reasons);
                
                Console.WriteLine($"Property {property.Id}: {property.Title} - Final Score: {property.MatchScore:F1}");
                Console.WriteLine($"  Reasons: {property.MatchReason}");
            }

            return properties;
        }

        // Helper methods để tính điểm chi tiết
        private double CalculateDistanceScore(double distance)
        {
            if (distance <= 3.0) return 100; // Rất gần (≤3km)
            if (distance <= 6.0) return 85;  // Gần (≤6km)
            if (distance <= 8.0) return 70;  // Trung bình (≤8km)
            if (distance <= 10.0) return 50; // Xa (≤10km)
            return 30; // Rất xa (>10km)
        }

        private double CalculatePriceScore(decimal propertyPrice, double? minPrice, double? maxPrice)
        {
            var priceInMillion = (double)(propertyPrice / 1000000);
            
            if (minPrice.HasValue && maxPrice.HasValue)
            {
                if (priceInMillion >= minPrice.Value && priceInMillion <= maxPrice.Value)
                    return 100; // Trong khoảng giá
                if (priceInMillion < minPrice.Value)
                    return 70;  // Dưới khoảng giá
                return 50;      // Trên khoảng giá
            }
            
            if (maxPrice.HasValue && priceInMillion <= maxPrice.Value)
                return 100; // Dưới giá tối đa
            return 60;      // Trên giá tối đa
        }

        private double CalculateBedroomScore(int propertyBedrooms, int minBedrooms)
        {
            if (propertyBedrooms >= minBedrooms + 2) return 100;      // Vượt yêu cầu
            if (propertyBedrooms >= minBedrooms + 1) return 90;       // Vượt yêu cầu ít
            if (propertyBedrooms >= minBedrooms) return 100;           // Đúng yêu cầu
            if (propertyBedrooms >= minBedrooms - 1) return 70;       // Thiếu 1 phòng
            return 40;                                                // Thiếu nhiều
        }

        private double CalculateAreaScore(decimal propertyArea, double? minArea, double? maxArea)
        {
            var area = (double)propertyArea;
            
            if (minArea.HasValue && maxArea.HasValue)
            {
                if (area >= minArea.Value && area <= maxArea.Value)
                    return 100; // Trong khoảng diện tích
                if (area < minArea.Value)
                    return 60;  // Dưới diện tích tối thiểu
                return 80;      // Trên diện tích tối đa
            }
            
            if (minArea.HasValue && area >= minArea.Value)
                return 100; // Đạt diện tích tối thiểu
            return 60;      // Không đạt diện tích tối thiểu
        }

        private double CalculateTypeScore(string propertyType, string searchType)
        {
            if (string.IsNullOrEmpty(propertyType) || string.IsNullOrEmpty(searchType))
                return 60; // Không match nếu thiếu thông tin
            
            var propType = propertyType.ToLower();
            var search = searchType.ToLower();
            
            if (propType.Contains(search) || search.Contains(propType)) return 100;
            
            // Logic matching thông minh
            if (search.Contains("phòng") && (propType.Contains("phòng") || propType.Contains("room"))) return 90;
            if (search.Contains("trọ") && (propType.Contains("trọ") || propType.Contains("room"))) return 90;
            if (search.Contains("chung cư") && (propType.Contains("chung cư") || propType.Contains("apartment"))) return 90;
            if (search.Contains("nhà") && (propType.Contains("nhà") || propType.Contains("house"))) return 90;
            if (search.Contains("căn hộ") && (propType.Contains("căn hộ") || propType.Contains("apartment"))) return 90;
            
            return 60; // Không match
        }

        private double CalculatePreferenceBonus(PropertyRecommendationDTO property, string userPreference)
        {
            var preference = userPreference.ToLower();
            var priceInMillion = (double)(property.Price / 1000000);
            
            switch (preference)
            {
                case "budget_friendly":
                    if (priceInMillion <= 15) return 15;
                    if (priceInMillion <= 25) return 10;
                    return 0;
                    
                case "luxury":
                    if (priceInMillion >= 50) return 20;
                    if (priceInMillion >= 30) return 15;
                    return 0;
                    
                case "family_friendly":
                    if (property.Bedrooms >= 3 && property.Area >= 80) return 20;
                    if (property.Bedrooms >= 2) return 15;
                    return 0;
                    
                case "student":
                    if (priceInMillion <= 10 && property.Area <= 50) return 25;
                    if (priceInMillion <= 15) return 15;
                    return 0;
                    
                case "professional":
                    if (priceInMillion >= 20 && property.Area >= 80) return 20;
                    if (priceInMillion >= 15) return 10;
                    return 0;
                    
                default:
                    return 0;
            }
        }

        private double CalculateAmenityBonus(PropertyRecommendationDTO property, List<string> requiredAmenities)
        {
            if (!requiredAmenities.Any()) return 0;
            
            var matchedCount = requiredAmenities.Count(required => 
                (property.Amenities?.Any(amenity => 
                    amenity.ToLower().Contains(required.ToLower())) ?? false));
            
            return matchedCount * 8; // 8 điểm cho mỗi tiện ích match
        }

        private string GetMatchReason(double score, double distance, decimal price)
        {
            if (score >= 90) return "Phù hợp hoàn hảo với tiêu chí của bạn";
            if (score >= 80) return "Rất phù hợp với nhu cầu";
            if (score >= 70) return "Phù hợp với yêu cầu cơ bản";
            if (score >= 60) return "Đáp ứng phần lớn tiêu chí";
            return "Có thể phù hợp với một số tiêu chí";
        }

        // Method này đã được bỏ vì không còn dùng sample data

        // Hàm tạm thời để lấy tọa độ - trong thực tế cần có tọa độ thực trong database
        private double GetPropertyLatitude(Property property)
        {
            // Fallback: Sử dụng tọa độ dựa trên province
            var provinceName = property.Address?.Province?.Name?.ToLower();
            if (!string.IsNullOrEmpty(provinceName))
            {
                if (provinceName.Contains("ho chi minh") || provinceName.Contains("tp hcm"))
                {
                    // Tọa độ TP.HCM + offset nhỏ để tránh trùng lặp
                    return 10.8231 + (property.Id % 100) * 0.001;
                }
                else if (provinceName.Contains("ha noi") || provinceName.Contains("hanoi"))
                {
                    // Tọa độ Hà Nội + offset nhỏ để tránh trùng lặp
                    return 21.0285 + (property.Id % 100) * 0.001;
                }
            }
            
            // Mặc định: Hà Nội
            return 21.0285 + (property.Id % 100) * 0.001;
        }

        private double GetPropertyLongitude(Property property)
        {
            // Fallback: Sử dụng tọa độ dựa trên province
            var provinceName = property.Address?.Province?.Name?.ToLower();
            if (!string.IsNullOrEmpty(provinceName))
            {
                if (provinceName.Contains("ho chi minh") || provinceName.Contains("tp hcm"))
                {
                    // Tọa độ TP.HCM + offset nhỏ để tránh trùng lặp
                    return 106.6297 + (property.Id % 100) * 0.001;
                }
                else if (provinceName.Contains("ha noi") || provinceName.Contains("hanoi"))
                {
                    // Tọa độ Hà Nội + offset nhỏ để tránh trùng lặp
                    return 105.8542 + (property.Id % 100) * 0.001;
                }
            }
            
            // Mặc định: Hà Nội
            return 105.8542 + (property.Id % 100) * 0.001;
        }

        // Method này đã được bỏ vì không còn dùng sample data

        private AmenityStatistics AnalyzeAmenityStatistics(List<DetailedAmenityDTO> amenities)
        {
            var stats = new AmenityStatistics();
            
            if (!amenities.Any()) return stats;
            
            // Nhóm theo loại tiện ích
            var groupedAmenities = amenities.GroupBy(a => a.Type).ToList();
            
            foreach (var group in groupedAmenities)
            {
                var type = group.Key;
                var count = group.Count();
                var avgDistance = group.Average(a => a.DistanceKm);
                var avgRating = group.Average(a => a.Rating ?? 0);
                
                stats.AmenityCounts[type] = count;
                stats.AverageDistances[type] = avgDistance;
                stats.AverageRatings[type] = avgRating;
                
                // Tìm tiện ích gần nhất cho mỗi loại
                var nearest = group.OrderBy(a => a.DistanceKm).First();
                stats.NearestAmenities[type] = nearest;
            }
            
            // Tính tổng quan
            stats.TotalAmenities = amenities.Count;
            stats.TotalTypes = groupedAmenities.Count;
            stats.AverageDistance = amenities.Average(a => a.DistanceKm);
            stats.AverageRating = amenities.Average(a => a.Rating ?? 0);
            
            return stats;
        }

        /// <summary>
        /// Lấy tiện ích xung quanh các BDS phù hợp
        /// </summary>
        private async Task<List<DetailedAmenityDTO>> GetAmenitiesAroundPropertiesAsync(List<PropertyRecommendationDTO> properties)
        {
            var allAmenities = new List<DetailedAmenityDTO>();
            var processedLocations = new HashSet<string>(); // Tránh duplicate

            foreach (var property in properties.Take(5)) // Chỉ lấy top 5 BDS để tìm tiện ích
            {
                var locationKey = $"{property.Latitude:F4}_{property.Longitude:F4}";
                if (processedLocations.Contains(locationKey)) continue;

                try
                {
                    Console.WriteLine($"Tìm tiện ích xung quanh BDS {property.Id} tại ({property.Latitude:F4}, {property.Longitude:F4})");
                    
                    // Tìm tiện ích trong bán kính 2km xung quanh mỗi BDS
                    var propertyAmenities = await _openStreetMapService.GetNearbyAmenitiesAsync(
                        property.Latitude, 
                        property.Longitude, 
                        2.0 // Bán kính cố định 2km
                    );

                    // Chuyển đổi sang DetailedAmenityDTO
                    var detailedAmenities = propertyAmenities.Select(a => new DetailedAmenityDTO
                    {
                        Name = a.Name,
                        Type = a.Type,
                        Address = $"{a.Latitude:F6}, {a.Longitude:F6}",
                        DistanceKm = a.DistanceKm,
                        WalkingTimeMinutes = a.WalkingTimeMinutes,
                        DrivingTimeMinutes = a.DrivingTimeMinutes,
                        Rating = a.Rating,
                        UserRatingsTotal = a.UserRatingsTotal,
                        Latitude = a.Latitude,
                        Longitude = a.Longitude,
                        Description = a.Description,
                        OpeningHours = a.OpeningHours,
                        PhoneNumber = a.PhoneNumber,
                        Website = a.Website
                    }).ToList();

                    // Thêm vào danh sách chung
                    allAmenities.AddRange(detailedAmenities);
                    processedLocations.Add(locationKey);
                    
                    Console.WriteLine($"Tìm thấy {detailedAmenities.Count} tiện ích xung quanh BDS {property.Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi tìm tiện ích xung quanh BDS {property.Id}: {ex.Message}");
                }
            }

            // Loại bỏ duplicate và sắp xếp theo khoảng cách
            var uniqueAmenities = allAmenities
                .GroupBy(a => $"{a.Name}_{a.Type}_{a.Latitude:F4}_{a.Longitude:F4}")
                .Select(g => g.First())
                .OrderBy(a => a.DistanceKm)
                .ToList();

            Console.WriteLine($"Tổng cộng tìm thấy {uniqueAmenities.Count} tiện ích xung quanh các BDS");
            return uniqueAmenities;
        }

        /// <summary>
        /// Lấy thông tin giao thông xung quanh các BDS
        /// </summary>
        private async Task<List<string>> GetTransportationAroundPropertiesAsync(List<PropertyRecommendationDTO> properties)
        {
            var allTransportInfo = new List<string>();
            var processedLocations = new HashSet<string>();

            foreach (var property in properties.Take(3)) // Chỉ lấy top 3 BDS
            {
                var locationKey = $"{property.Latitude:F4}_{property.Longitude:F4}";
                if (processedLocations.Contains(locationKey)) continue;

                try
                {
                    var transportInfo = await _openStreetMapService.GetTransportationInfoAsync(
                        property.Latitude, 
                        property.Longitude
                    );

                    // Thêm thông tin giao thông với prefix để biết thuộc BDS nào
                    var propertyTransportInfo = transportInfo.Select(t => $"[BDS {property.Id}] {t}").ToList();
                    allTransportInfo.AddRange(propertyTransportInfo);
                    processedLocations.Add(locationKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi lấy thông tin giao thông cho BDS {property.Id}: {ex.Message}");
                }
            }

            return allTransportInfo.Distinct().ToList();
        }

        /// <summary>
        /// Lấy tiện ích xung quanh các BDS dựa trên địa chỉ
        /// </summary>
        private async Task<List<DetailedAmenityDTO>> GetAmenitiesAroundPropertiesByAddressAsync(List<PropertyRecommendationDTO> properties)
        {
            var allAmenities = new List<DetailedAmenityDTO>();
            var processedAddresses = new HashSet<string>();

            foreach (var property in properties.Take(5)) // Chỉ lấy top 5 BDS
            {
                var addressKey = $"{property.Province}_{property.Ward}_{property.Street}_{property.DetailedAddress}";
                if (processedAddresses.Contains(addressKey)) continue;

                try
                {
                    Console.WriteLine($"Tìm tiện ích xung quanh BDS {property.Id} tại {property.Province}, {property.Ward}, {property.Street}");
                    
                    // Tìm tiện ích dựa trên địa chỉ của BDS
                    var propertyAmenities = await _openStreetMapService.GetAmenitiesByAddressAsync(
                        property.Province ?? "Unknown",
                        property.Ward ?? "Unknown", 
                        property.Street ?? "Unknown",
                        property.DetailedAddress ?? "Unknown"
                    );

                    // Thêm vào danh sách chung
                    allAmenities.AddRange(propertyAmenities);
                    processedAddresses.Add(addressKey);
                    
                    Console.WriteLine($"Tìm thấy {propertyAmenities.Count} tiện ích xung quanh BDS {property.Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi tìm tiện ích xung quanh BDS {property.Id}: {ex.Message}");
                }
            }

            // Loại bỏ duplicate và sắp xếp theo khoảng cách
            var uniqueAmenities = allAmenities
                .GroupBy(a => $"{a.Name}_{a.Type}_{a.Latitude:F4}_{a.Longitude:F4}")
                .Select(g => g.First())
                .OrderBy(a => a.DistanceKm)
                .ToList();

            Console.WriteLine($"Tổng cộng tìm thấy {uniqueAmenities.Count} tiện ích xung quanh các BDS");
            return uniqueAmenities;
        }

        /// <summary>
        /// Lấy thông tin giao thông xung quanh các BDS dựa trên địa chỉ
        /// </summary>
        private async Task<List<string>> GetTransportationAroundPropertiesByAddressAsync(List<PropertyRecommendationDTO> properties)
        {
            var allTransportInfo = new List<string>();
            var processedAddresses = new HashSet<string>();

            foreach (var property in properties.Take(3)) // Chỉ lấy top 3 BDS
            {
                var addressKey = $"{property.Province}_{property.Ward}_{property.Street}_{property.DetailedAddress}";
                if (processedAddresses.Contains(addressKey)) continue;

                try
                {
                    Console.WriteLine($"Tìm giao thông xung quanh BDS {property.Id} tại {property.Province}, {property.Ward}, {property.Street}");
                    
                    var transportInfo = await _openStreetMapService.GetTransportationByAddressAsync(
                        property.Province ?? "Unknown",
                        property.Ward ?? "Unknown",
                        property.Street ?? "Unknown", 
                        property.DetailedAddress ?? "Unknown"
                    );

                    // Thêm thông tin giao thông với prefix để biết thuộc BDS nào
                    var propertyTransportInfo = transportInfo.Select(t => $"[BDS {property.Id}] {t}").ToList();
                    allTransportInfo.AddRange(propertyTransportInfo);
                    processedAddresses.Add(addressKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi lấy thông tin giao thông cho BDS {property.Id}: {ex.Message}");
                }
            }

            return allTransportInfo.Distinct().ToList();
        }

        /// <summary>
        /// Lấy tiện ích và giao thông riêng cho từng BDS
        /// </summary>
        private async Task<List<PropertyRecommendationDTO>> GetPropertiesWithIndividualAmenitiesAsync(List<PropertyRecommendationDTO> properties)
        {
            var propertiesWithAmenities = new List<PropertyRecommendationDTO>();

            foreach (var property in properties)
            {
                try
                {
                    Console.WriteLine($"Lấy tiện ích và giao thông riêng cho BDS {property.Id} tại {property.Province}, {property.Ward}, {property.Street}");
                    
                    List<DetailedAmenityDTO> propertyAmenities = new List<DetailedAmenityDTO>();
                    List<string> propertyTransportation = new List<string>();
                    
                    // Kiểm tra xem OpenStreetMap service có sẵn sàng không
                    if (_openStreetMapService != null)
                    {
                        try
                        {
                            // Lấy tiện ích riêng cho BDS này từ OpenStreetMap API
                            propertyAmenities = await _openStreetMapService.GetAmenitiesByAddressAsync(
                                property.Province ?? "Unknown",
                                property.Ward ?? "Unknown", 
                                property.Street ?? "Unknown",
                                property.DetailedAddress ?? "Unknown"
                            ) ?? new List<DetailedAmenityDTO>();

                            // Lấy giao thông riêng cho BDS này từ OpenStreetMap API
                            propertyTransportation = await _openStreetMapService.GetTransportationByAddressAsync(
                                property.Province ?? "Unknown",
                                property.Ward ?? "Unknown",
                                property.Street ?? "Unknown", 
                                property.DetailedAddress ?? "Unknown"
                            ) ?? new List<string>();
                        }
                        catch (Exception osmEx)
                        {
                            Console.WriteLine($"Lỗi OpenStreetMap API cho BDS {property.Id}: {osmEx.Message}");
                            // Sử dụng danh sách rỗng nếu có lỗi
                            propertyAmenities = new List<DetailedAmenityDTO>();
                            propertyTransportation = new List<string>();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"OpenStreetMap service không khả dụng cho BDS {property.Id}");
                    }

                    // Tạo BDS mới với tiện ích và giao thông thực từ API
                    var propertyWithAmenities = new PropertyRecommendationDTO
                    {
                        Id = property.Id,
                        Title = property.Title,
                        Description = property.Description,
                        Type = property.Type,
                        Area = property.Area,
                        Bedrooms = property.Bedrooms,
                        Price = property.Price,
                        Status = property.Status,
                        PrimaryImageUrl = property.PrimaryImageUrl,
                        Province = property.Province,
                        Ward = property.Ward,
                        Street = property.Street,
                        DetailedAddress = property.DetailedAddress,
                        Latitude = property.Latitude,
                        Longitude = property.Longitude,
                        DistanceKm = property.DistanceKm,
                        WalkingTimeMinutes = property.WalkingTimeMinutes,
                        DrivingTimeMinutes = property.DrivingTimeMinutes,
                        MatchScore = property.MatchScore,
                        MatchReason = property.MatchReason,
                        Amenities = property.Amenities,
                        NearbyPoints = new List<string>(), // Bỏ sample data
                        // Thêm tiện ích và giao thông thực từ OpenStreetMap API
                        NearbyAmenities = propertyAmenities,
                        TransportationInfo = propertyTransportation
                    };

                    propertiesWithAmenities.Add(propertyWithAmenities);
                    Console.WriteLine($"BDS {property.Id}: {propertyAmenities.Count} tiện ích thực, {propertyTransportation.Count} điểm giao thông thực");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi lấy tiện ích cho BDS {property.Id}: {ex.Message}");
                    // Thêm BDS gốc nếu có lỗi, nhưng không có sample data
                    var propertyWithoutSample = new PropertyRecommendationDTO
                    {
                        Id = property.Id,
                        Title = property.Title,
                        Description = property.Description,
                        Type = property.Type,
                        Area = property.Area,
                        Bedrooms = property.Bedrooms,
                        Price = property.Price,
                        Status = property.Status,
                        PrimaryImageUrl = property.PrimaryImageUrl,
                        Province = property.Province,
                        Ward = property.Ward,
                        Street = property.Street,
                        DetailedAddress = property.DetailedAddress,
                        Latitude = property.Latitude,
                        Longitude = property.Longitude,
                        DistanceKm = property.DistanceKm,
                        WalkingTimeMinutes = property.WalkingTimeMinutes,
                        DrivingTimeMinutes = property.DrivingTimeMinutes,
                        MatchScore = property.MatchScore,
                        MatchReason = property.MatchReason,
                        Amenities = property.Amenities,
                        NearbyPoints = new List<string>(), // Bỏ sample data
                        NearbyAmenities = new List<DetailedAmenityDTO>(),
                        TransportationInfo = new List<string>()
                    };
                    propertiesWithAmenities.Add(propertyWithoutSample);
                }
            }

            return propertiesWithAmenities;
        }

        /// <summary>
        /// Lấy tên province từ searchLocation
        /// </summary>
        private string GetTargetProvince(string searchLocation)
        {
            if (string.IsNullOrEmpty(searchLocation)) return null;

            var location = searchLocation.ToLower().Trim();
            
            // Mapping các từ khóa tìm kiếm với tên province
            if (location.Contains("ho chi minh") || location.Contains("tp hcm") || location.Contains("hcm") || location.Contains("saigon"))
                return "Ho Chi Minh City";
            
            if (location.Contains("ha noi") || location.Contains("hanoi") || location.Contains("hn"))
                return "Hanoi";
            
            if (location.Contains("da nang") || location.Contains("danang"))
                return "Da Nang";
            
            if (location.Contains("can tho") || location.Contains("cantho"))
                return "Can Tho";
            
            if (location.Contains("hai phong") || location.Contains("haiphong"))
                return "Hai Phong";
            
            if (location.Contains("nha trang") || location.Contains("nhatrang"))
                return "Khanh Hoa";
            
            if (location.Contains("vung tau") || location.Contains("vungtau"))
                return "Ba Ria - Vung Tau";
            
            if (location.Contains("buon ma thuot") || location.Contains("buonmathuot"))
                return "Dak Lak";
            
            if (location.Contains("dalat") || location.Contains("da lat") || location.Contains("lam dong"))
                return "Lam Dong";
            
            if (location.Contains("phu quoc") || location.Contains("phuquoc"))
                return "Kien Giang";

            // Nếu không match được, trả về null để không lọc
            Console.WriteLine($"Could not determine province for search location: '{searchLocation}'");
            return null;
        }

        private async Task<LocationDTO> GetCoordinatesFromAddressAsync(string address)
        {
            try
            {
                // Sử dụng OpenStreetMap Nominatim API để lấy tọa độ từ địa chỉ (miễn phí)
                var geocodeResponse = await _openStreetMapService.GeocodeAddressAsync(address);
                
                if (geocodeResponse?.Results != null && geocodeResponse.Results.Any())
                {
                    var result = geocodeResponse.Results[0];
                    if (double.TryParse(result.Lat, out double lat) && double.TryParse(result.Lon, out double lon))
                    {
                        return new LocationDTO
                        {
                            Latitude = lat,
                            Longitude = lon,
                            Address = result.DisplayName
                        };
                    }
                }

                // Fallback: Sử dụng tọa độ mẫu cho các địa điểm phổ biến ở Việt Nam
                var sampleLocations = new Dictionary<string, (double lat, double lng)>
                {
                    { "hà nội", (21.0285, 105.8542) },
                    { "tp hcm", (10.8231, 106.6297) },
                    { "ho chi minh", (10.8231, 106.6297) },
                    { "tan dinh", (10.7901, 106.6921) }, // Tân Định, TP.HCM
                    { "quận 1", (10.7769, 106.7009) }, // Quận 1, TP.HCM
                    { "quận 3", (10.7829, 106.6881) }, // Quận 3, TP.HCM
                    { "đà nẵng", (16.0544, 108.2022) },
                    { "huế", (16.4637, 107.5909) },
                    { "nha trang", (12.2388, 109.1967) },
                    { "vũng tàu", (10.3454, 107.0843) },
                    { "cần thơ", (10.0452, 105.7469) },
                    { "hải phòng", (20.8449, 106.6881) }
                };

                var searchAddress = address.ToLower();
                
                // Ưu tiên tìm kiếm chính xác hơn
                foreach (var location in sampleLocations)
                {
                    if (searchAddress.Contains(location.Key))
                    {
                        Console.WriteLine($"Found location match: {location.Key} -> ({location.Value.lat}, {location.Value.lng})");
                        return new LocationDTO
                        {
                            Latitude = location.Value.lat,
                            Longitude = location.Value.lng,
                            Address = address
                        };
                    }
                }
                
                // Nếu không tìm thấy, sử dụng tọa độ mặc định cho TP.HCM
                if (searchAddress.Contains("tp hcm") || searchAddress.Contains("ho chi minh") || searchAddress.Contains("tan dinh"))
                {
                    Console.WriteLine($"Using TP.HCM coordinates for: {address}");
                    return new LocationDTO
                    {
                        Latitude = 10.7901, // Tân Định, TP.HCM
                        Longitude = 106.6921,
                        Address = address
                    };
                }

                // Mặc định trả về Hà Nội nếu không tìm thấy
                return new LocationDTO
                {
                    Latitude = 21.0285,
                    Longitude = 105.8542,
                    Address = address
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting coordinates from address: {ex.Message}");
                
                // Fallback to default location
                return new LocationDTO
                {
                    Latitude = 21.0285,
                    Longitude = 105.8542,
                    Address = address
                };
            }
        }
    }
} 