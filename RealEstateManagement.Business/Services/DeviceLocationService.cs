using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RealEstateManagement.Business.DTO.Location;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RealEstateManagement.Business.Services
{
    public interface IDeviceLocationService
    {
        Task<LocationDTO> GetDeviceLocationAsync(string deviceId, string ipAddress = null);
        Task<bool> UpdateDeviceLocationAsync(string deviceId, double latitude, double longitude, string address = null);
        Task<List<LocationDTO>> GetRecentLocationsAsync(string deviceId, int count = 5);
        Task<LocationDTO> GetLocationFromIPAsync(string ipAddress);
        Task<bool> ValidateLocationAsync(double latitude, double longitude);
        Task<string> GetLocationDescriptionAsync(double latitude, double longitude);
    }

    public class DeviceLocationService : IDeviceLocationService
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly IOpenStreetMapService _openStreetMapService;
        private readonly TimeSpan _locationCacheExpiration = TimeSpan.FromMinutes(30);

        public DeviceLocationService(IMemoryCache cache, IConfiguration configuration, IOpenStreetMapService openStreetMapService)
        {
            _cache = cache;
            _configuration = configuration;
            _openStreetMapService = openStreetMapService;
        }

        public async Task<LocationDTO> GetDeviceLocationAsync(string deviceId, string ipAddress = null)
        {
            var cacheKey = $"device_location_{deviceId}";
            
            // Try to get from cache first
            if (_cache.TryGetValue(cacheKey, out LocationDTO cachedLocation))
            {
                return cachedLocation;
            }

            // If no cached location, try to get from IP address
            if (!string.IsNullOrEmpty(ipAddress))
            {
                var ipLocation = await GetLocationFromIPAsync(ipAddress);
                if (ipLocation != null)
                {
                    // Cache the IP-based location
                    _cache.Set(cacheKey, ipLocation, _locationCacheExpiration);
                    return ipLocation;
                }
            }

            // Return default location (Hanoi)
            var defaultLocation = new LocationDTO
            {
                Latitude = 21.0285,
                Longitude = 105.8542,
                Address = "Hà Nội, Việt Nam",
                City = "Hà Nội",
                District = "Hoàn Kiếm",
                Ward = "Tràng Tiền"
            };

            _cache.Set(cacheKey, defaultLocation, _locationCacheExpiration);
            return defaultLocation;
        }

        public async Task<bool> UpdateDeviceLocationAsync(string deviceId, double latitude, double longitude, string address = null)
        {
            try
            {
                if (!await ValidateLocationAsync(latitude, longitude))
                {
                    return false;
                }

                var location = new LocationDTO
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    Address = address
                };

                // Get additional location details from Google Maps
                try
                {
                    var locationDescription = await GetLocationDescriptionAsync(latitude, longitude);
                    if (!string.IsNullOrEmpty(locationDescription))
                    {
                        location.Address = locationDescription;
                    }
                }
                catch (Exception ex)
                {
                    // Log error but continue
                    Console.WriteLine($"Error getting location description: {ex.Message}");
                }

                // Cache the location
                var cacheKey = $"device_location_{deviceId}";
                _cache.Set(cacheKey, location, _locationCacheExpiration);

                // Store in recent locations
                var recentKey = $"recent_locations_{deviceId}";
                var recentLocations = _cache.Get<List<LocationDTO>>(recentKey) ?? new List<LocationDTO>();
                recentLocations.Insert(0, location);
                
                // Keep only the last 10 locations
                if (recentLocations.Count > 10)
                {
                    recentLocations = recentLocations.GetRange(0, 10);
                }
                
                _cache.Set(recentKey, recentLocations, TimeSpan.FromHours(24));

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating device location: {ex.Message}");
                return false;
            }
        }

        public async Task<List<LocationDTO>> GetRecentLocationsAsync(string deviceId, int count = 5)
        {
            var recentKey = $"recent_locations_{deviceId}";
            var recentLocations = _cache.Get<List<LocationDTO>>(recentKey) ?? new List<LocationDTO>();
            
            return recentLocations.Take(count).ToList();
        }

        public async Task<LocationDTO> GetLocationFromIPAsync(string ipAddress)
        {
            try
            {
                // In a real implementation, you would call an IP geolocation service
                // For now, we'll return sample data based on common IP ranges
                
                if (string.IsNullOrEmpty(ipAddress))
                    return null;

                // Sample IP-based locations for Vietnam
                var sampleLocations = new Dictionary<string, LocationDTO>
                {
                    { "113.160.0.0", new LocationDTO { Latitude = 21.0285, Longitude = 105.8542, City = "Hà Nội" } },
                    { "115.72.0.0", new LocationDTO { Latitude = 10.8231, Longitude = 106.6297, City = "TP. Hồ Chí Minh" } },
                    { "118.68.0.0", new LocationDTO { Latitude = 16.0544, Longitude = 108.2022, City = "Đà Nẵng" } },
                    { "171.224.0.0", new LocationDTO { Latitude = 16.4637, Longitude = 107.5909, City = "Huế" } }
                };

                // Simple IP matching (in real implementation, use proper IP geolocation service)
                foreach (var sample in sampleLocations)
                {
                    if (ipAddress.StartsWith(sample.Key.Substring(0, 7)))
                    {
                        return sample.Value;
                    }
                }

                // Return Hanoi as default for Vietnam IPs
                return new LocationDTO
                {
                    Latitude = 21.0285,
                    Longitude = 105.8542,
                    City = "Hà Nội",
                    Address = "Hà Nội, Việt Nam"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting location from IP: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ValidateLocationAsync(double latitude, double longitude)
        {
            // Validate coordinates are within reasonable bounds
            if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            {
                return false;
            }

            // Check if coordinates are within Vietnam bounds (approximate)
            if (latitude < 8.0 || latitude > 23.5 || longitude < 102.0 || longitude > 110.0)
            {
                // Location is outside Vietnam - you might want to allow this or restrict it
                // For now, we'll allow it but log it
                Console.WriteLine($"Location outside Vietnam: {latitude}, {longitude}");
            }

            return true;
        }

        public async Task<string> GetLocationDescriptionAsync(double latitude, double longitude)
        {
            try
            {
                // Use OpenStreetMap reverse geocoding
                var address = await _openStreetMapService.ReverseGeocodeAsync(latitude, longitude);
                return address;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting location description: {ex.Message}");
            }

            return null;
        }
    }
} 