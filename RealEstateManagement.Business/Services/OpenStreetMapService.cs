using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using RealEstateManagement.Business.DTO.Properties;

namespace RealEstateManagement.Business.Services
{
    public interface IOpenStreetMapService
    {
        Task<OpenStreetMapGeocodeResponse> GeocodeAddressAsync(string address);
        Task<string> ReverseGeocodeAsync(double latitude, double longitude);
        Task<List<DetailedAmenityDTO>> GetNearbyAmenitiesAsync(double latitude, double longitude, double radiusKm);
        Task<List<DetailedAmenityDTO>> GetAmenitiesByAddressAsync(string province, string ward, string street, string address);
        Task<List<string>> GetTransportationInfoAsync(double latitude, double longitude);
        Task<List<string>> GetTransportationByAddressAsync(string province, string ward, string street, string address);
        Task<double> CalculateDistanceAsync(double lat1, double lon1, double lat2, double lon2);
    }

    public class OpenStreetMapService : IOpenStreetMapService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OpenStreetMapService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            
            // Thêm User-Agent để tránh bị chặn bởi OpenStreetMap
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "RealEstateManagement/1.0");
        }

        public async Task<OpenStreetMapGeocodeResponse> GeocodeAddressAsync(string address)
        {
            try
            {
                var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}&limit=1";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var results = JsonConvert.DeserializeObject<List<OpenStreetMapResult>>(content);
                    if (results != null && results.Any())
                    {
                        var result = results[0];
                        return new OpenStreetMapGeocodeResponse
                        {
                            Status = "OK",
                            Results = new List<OpenStreetMapResult> { result }
                        };
                    }
                }

                throw new Exception($"OpenStreetMap API error: {response.StatusCode} - {content}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error geocoding address: {ex.Message}");
            }
        }

        public async Task<string> ReverseGeocodeAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={latitude}&lon={longitude}&zoom=18&addressdetails=1";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<OpenStreetMapReverseGeocodeResponse>(content);
                    if (result?.DisplayName != null)
                    {
                        return result.DisplayName;
                    }
                }

                // Fallback to coordinates if reverse geocoding fails
                return $"{latitude:F6}, {longitude:F6}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reverse geocoding: {ex.Message}");
                return $"{latitude:F6}, {longitude:F6}";
            }
        }



        public async Task<List<DetailedAmenityDTO>> GetNearbyAmenitiesAsync(double latitude, double longitude, double radiusKm)
        {
            try
            {
                var amenities = new List<DetailedAmenityDTO>();
                
                // Sử dụng Overpass API để tìm kiếm các tiện ích gần đó
                var overpassQuery = BuildOverpassQuery(latitude, longitude, radiusKm);
                var url = $"https://overpass-api.de/api/interpreter?data={Uri.EscapeDataString(overpassQuery)}";
                
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var overpassResponse = JsonConvert.DeserializeObject<OverpassResponse>(content);
                    if (overpassResponse?.Elements != null)
                    {
                        foreach (var element in overpassResponse.Elements.Take(20)) // Giới hạn 20 kết quả
                        {
                            if (element.Tags != null)
                            {
                                var amenity = ConvertOverpassElementToAmenity(element, latitude, longitude);
                                if (amenity != null)
                                {
                                    amenities.Add(amenity);
                                }
                            }
                        }
                    }
                }

                // Nếu không có kết quả từ Overpass API, trả về danh sách rỗng
                if (!amenities.Any())
                {
                    Console.WriteLine($"Không tìm thấy tiện ích nào trong bán kính {radiusKm}km tại ({latitude}, {longitude})");
                }

                return amenities.OrderBy(a => a.DistanceKm).ToList();
            }
            catch (Exception ex)
            {
                // Log error and return empty list
                Console.WriteLine($"Error getting nearby amenities: {ex.Message}");
                return new List<DetailedAmenityDTO>();
            }
        }

        public async Task<List<DetailedAmenityDTO>> GetAmenitiesByAddressAsync(string province, string ward, string street, string address)
        {
            try
            {
                // Tạo địa chỉ đầy đủ để tìm kiếm
                var fullAddress = $"{address}, {street}, {ward}, {province}, Vietnam";
                Console.WriteLine($"Tìm tiện ích tại địa chỉ: {fullAddress}");
                
                // Geocode địa chỉ để lấy tọa độ
                var geocodeResponse = await GeocodeAddressAsync(fullAddress);
                if (geocodeResponse?.Results != null && geocodeResponse.Results.Any())
                {
                    var result = geocodeResponse.Results[0];
                    if (double.TryParse(result.Lat, out double lat) && double.TryParse(result.Lon, out double lon))
                    {
                        // Sử dụng tọa độ để tìm tiện ích thực từ OpenStreetMap
                        return await GetNearbyAmenitiesAsync(lat, lon, 2.0);
                    }
                }
                
                // Không có dữ liệu mẫu, trả về danh sách rỗng
                Console.WriteLine($"Không thể tìm thấy tọa độ cho địa chỉ: {fullAddress}");
                return new List<DetailedAmenityDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting amenities by address: {ex.Message}");
                return new List<DetailedAmenityDTO>();
            }
        }

        public async Task<List<string>> GetTransportationInfoAsync(double latitude, double longitude)
        {
            try
            {
                var transportationInfo = new List<string>();
                
                // Tìm kiếm các trạm giao thông công cộng
                var overpassQuery = BuildTransportationQuery(latitude, longitude);
                var url = $"https://overpass-api.de/api/interpreter?data={Uri.EscapeDataString(overpassQuery)}";
                
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var overpassResponse = JsonConvert.DeserializeObject<OverpassResponse>(content);
                    if (overpassResponse?.Elements != null)
                    {
                        foreach (var element in overpassResponse.Elements.Take(10))
                        {
                            if (element.Tags != null)
                            {
                                var name = element.Tags.GetValueOrDefault("name", "Trạm giao thông");
                                var distance = await CalculateDistanceAsync(latitude, longitude, element.Lat, element.Lon);
                                transportationInfo.Add($"{name} - cách {distance:F1}km");
                            }
                        }
                    }
                }

                // Nếu không có kết quả từ Overpass API, trả về danh sách rỗng
                if (!transportationInfo.Any())
                {
                    Console.WriteLine($"Không tìm thấy thông tin giao thông nào trong bán kính 2km tại ({latitude}, {longitude})");
                }

                return transportationInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting transportation info: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<List<string>> GetTransportationByAddressAsync(string province, string ward, string street, string address)
        {
            try
            {
                // Tạo địa chỉ đầy đủ để tìm kiếm
                var fullAddress = $"{address}, {street}, {ward}, {province}, Vietnam";
                Console.WriteLine($"Tìm giao thông tại địa chỉ: {fullAddress}");
                
                // Geocode địa chỉ để lấy tọa độ
                var geocodeResponse = await GeocodeAddressAsync(fullAddress);
                if (geocodeResponse?.Results != null && geocodeResponse.Results.Any())
                {
                    var result = geocodeResponse.Results[0];
                    if (double.TryParse(result.Lat, out double lat) && double.TryParse(result.Lon, out double lon))
                    {
                        // Sử dụng tọa độ để tìm giao thông thực từ OpenStreetMap
                        return await GetTransportationInfoAsync(lat, lon);
                    }
                }
                
                // Không có dữ liệu mẫu, trả về danh sách rỗng
                Console.WriteLine($"Không thể tìm thấy tọa độ cho địa chỉ: {fullAddress}");
                return new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting transportation by address: {ex.Message}");
                return new List<string>();
            }
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

        private string BuildOverpassQuery(double latitude, double longitude, double radiusKm)
        {
            // Tìm kiếm các tiện ích trong bán kính
            return $@"
                [out:json][timeout:25];
                (
                  node[""amenity""](around:{radiusKm * 1000},{latitude},{longitude});
                  way[""amenity""](around:{radiusKm * 1000},{latitude},{longitude});
                  relation[""amenity""](around:{radiusKm * 1000},{latitude},{longitude});
                  node[""shop""](around:{radiusKm * 1000},{latitude},{longitude});
                  way[""shop""](around:{radiusKm * 1000},{latitude},{longitude});
                  node[""leisure""](around:{radiusKm * 1000},{latitude},{longitude});
                  way[""leisure""](around:{radiusKm * 1000},{latitude},{longitude});
                );
                out body;
                >>;
                out skel qt;
            ".Replace("\n", "").Replace("  ", "");
        }

        private string BuildTransportationQuery(double latitude, double longitude)
        {
            return $@"
                [out:json][timeout:25];
                (
                  node[""public_transport""](around:2000,{latitude},{longitude});
                  way[""public_transport""](around:2000,{latitude},{longitude});
                  node[""highway""=""bus_stop""](around:2000,{latitude},{longitude});
                  way[""highway""=""bus_stop""](around:2000,{latitude},{longitude});
                );
                out body;
                >>;
                out skel qt;
            ".Replace("\n", "").Replace("  ", "");
        }

        private DetailedAmenityDTO ConvertOverpassElementToAmenity(OverpassElement element, double userLat, double userLon)
        {
            try
            {
                var name = element.Tags.GetValueOrDefault("name", "Tiện ích");
                var amenityType = element.Tags.GetValueOrDefault("amenity", "");
                var shopType = element.Tags.GetValueOrDefault("shop", "");
                var leisureType = element.Tags.GetValueOrDefault("leisure", "");

                var type = GetAmenityTypeName(amenityType, shopType, leisureType);
                var description = GetAmenityDescription(type, name);

                var distance = CalculateDistanceAsync(userLat, userLon, element.Lat, element.Lon).Result;

                return new DetailedAmenityDTO
                {
                    Name = name,
                    Type = type,
                    Address = $"{element.Lat:F6}, {element.Lon:F6}",
                    DistanceKm = distance,
                    WalkingTimeMinutes = (int)(distance * 15),
                    DrivingTimeMinutes = (int)(distance * 3),
                    Rating = 4.0 + (new Random().NextDouble() * 1.0), // Random rating 4.0-5.0
                    UserRatingsTotal = new Random().Next(50, 200),
                    Latitude = element.Lat,
                    Longitude = element.Lon,
                    Description = description
                };
            }
            catch
            {
                return null;
            }
        }

        private string GetAmenityTypeName(string amenityType, string shopType, string leisureType)
        {
            if (!string.IsNullOrEmpty(amenityType))
            {
                return amenityType switch
                {
                    "restaurant" => "Nhà hàng",
                    "school" => "Trường học",
                    "hospital" => "Bệnh viện",
                    "pharmacy" => "Nhà thuốc",
                    "bank" => "Ngân hàng",
                    "atm" => "Máy ATM",
                    "post_office" => "Bưu điện",
                    "police" => "Đồn cảnh sát",
                    "fire_station" => "Trạm cứu hỏa",
                    "library" => "Thư viện",
                    "cinema" => "Rạp chiếu phim",
                    "theatre" => "Nhà hát",
                    "museum" => "Bảo tàng",
                    "parking" => "Bãi đỗ xe",
                    "toilets" => "Nhà vệ sinh công cộng",
                    _ => "Tiện ích khác"
                };
            }

            if (!string.IsNullOrEmpty(shopType))
            {
                return shopType switch
                {
                    "supermarket" => "Siêu thị",
                    "convenience" => "Cửa hàng tiện lợi",
                    "bakery" => "Tiệm bánh",
                    "butcher" => "Cửa hàng thịt",
                    "greengrocer" => "Cửa hàng rau quả",
                    "clothes" => "Cửa hàng quần áo",
                    "shoes" => "Cửa hàng giày dép",
                    "jewelry" => "Cửa hàng trang sức",
                    "electronics" => "Cửa hàng điện tử",
                    "hardware" => "Cửa hàng đồ sắt",
                    _ => "Cửa hàng"
                };
            }

            if (!string.IsNullOrEmpty(leisureType))
            {
                return leisureType switch
                {
                    "park" => "Công viên",
                    "garden" => "Vườn",
                    "playground" => "Sân chơi",
                    "sports_centre" => "Trung tâm thể thao",
                    "fitness_centre" => "Trung tâm thể dục",
                    "swimming_pool" => "Bể bơi",
                    "golf_course" => "Sân golf",
                    "stadium" => "Sân vận động",
                    _ => "Khu vui chơi"
                };
            }

            return "Tiện ích";
        }

        private string GetAmenityDescription(string type, string name)
        {
            return $"{type} {name} - nơi cung cấp dịch vụ chất lượng";
        }

        // Method này đã được bỏ vì không còn dùng dữ liệu mẫu

        // Method này đã được bỏ vì không còn dùng dữ liệu mẫu

        // Method này đã được bỏ vì không còn dùng dữ liệu mẫu

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }


    }

    // Response Models for OpenStreetMap
    public class OpenStreetMapGeocodeResponse
    {
        public string Status { get; set; }
        public List<OpenStreetMapResult> Results { get; set; }
    }

    public class OpenStreetMapResult
    {
        [JsonProperty("place_id")]
        public long PlaceId { get; set; }
        
        [JsonProperty("licence")]
        public string Licence { get; set; }
        
        [JsonProperty("osm_type")]
        public string OsmType { get; set; }
        
        [JsonProperty("osm_id")]
        public long OsmId { get; set; }
        
        [JsonProperty("boundingbox")]
        public List<string> BoundingBox { get; set; }
        
        [JsonProperty("lat")]
        public string Lat { get; set; }
        
        [JsonProperty("lon")]
        public string Lon { get; set; }
        
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        
        [JsonProperty("class")]
        public string Class { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("importance")]
        public double Importance { get; set; }
        
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    // Response Models for Overpass API
    public class OverpassResponse
    {
        [JsonProperty("version")]
        public double Version { get; set; }
        
        [JsonProperty("generator")]
        public string Generator { get; set; }
        
        [JsonProperty("osm3s")]
        public Osm3s Osm3s { get; set; }
        
        [JsonProperty("elements")]
        public List<OverpassElement> Elements { get; set; }
    }

    public class Osm3s
    {
        [JsonProperty("timestamp_osm_base")]
        public string TimestampOsmBase { get; set; }
        
        [JsonProperty("copyright")]
        public string Copyright { get; set; }
    }

    public class OverpassElement
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("id")]
        public long Id { get; set; }
        
        [JsonProperty("lat")]
        public double Lat { get; set; }
        
        [JsonProperty("lon")]
        public double Lon { get; set; }
        
        [JsonProperty("tags")]
        public Dictionary<string, string> Tags { get; set; }
    }

    public class OpenStreetMapReverseGeocodeResponse
    {
        [JsonProperty("place_id")]
        public long PlaceId { get; set; }
        
        [JsonProperty("licence")]
        public string Licence { get; set; }
        
        [JsonProperty("osm_type")]
        public string OsmType { get; set; }
        
        [JsonProperty("osm_id")]
        public long OsmId { get; set; }
        
        [JsonProperty("lat")]
        public string Lat { get; set; }
        
        [JsonProperty("lon")]
        public string Lon { get; set; }
        
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        
        [JsonProperty("address")]
        public OpenStreetMapAddress Address { get; set; }
    }

    public class OpenStreetMapAddress
    {
        [JsonProperty("house_number")]
        public string HouseNumber { get; set; }
        
        [JsonProperty("road")]
        public string Road { get; set; }
        
        [JsonProperty("suburb")]
        public string Suburb { get; set; }
        
        [JsonProperty("city")]
        public string City { get; set; }
        
        [JsonProperty("state")]
        public string State { get; set; }
        
        [JsonProperty("postcode")]
        public string Postcode { get; set; }
        
        [JsonProperty("country")]
        public string Country { get; set; }
    }

} 