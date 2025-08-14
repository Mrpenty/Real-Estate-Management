using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace RealEstateManagement.Business.Services
{
    public interface IGoogleMapsService
    {
        Task<GoogleMapsGeocodeResponse> GeocodeAddressAsync(string address);
        Task<GoogleMapsPlacesResponse> GetNearbyPlacesAsync(double latitude, double longitude, double radius, string type = null);
        Task<GoogleMapsDirectionsResponse> GetDirectionsAsync(double originLat, double originLng, double destLat, double destLng, string mode = "driving");
        Task<GoogleMapsDistanceMatrixResponse> GetDistanceMatrixAsync(double originLat, double originLng, List<double[]> destinations, string mode = "driving");
    }

    public class GoogleMapsService : IGoogleMapsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://maps.googleapis.com/maps/api";

        public GoogleMapsService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["GoogleMaps:ApiKey"];
        }

        public async Task<GoogleMapsGeocodeResponse> GeocodeAddressAsync(string address)
        {
            try
            {
                var url = $"{_baseUrl}/geocode/json?address={Uri.EscapeDataString(address)}&key={_apiKey}";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<GoogleMapsGeocodeResponse>(content);
                }

                throw new Exception($"Google Maps API error: {response.StatusCode} - {content}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error geocoding address: {ex.Message}");
            }
        }

        public async Task<GoogleMapsPlacesResponse> GetNearbyPlacesAsync(double latitude, double longitude, double radius, string type = null)
        {
            try
            {
                var url = $"{_baseUrl}/place/nearbysearch/json?location={latitude},{longitude}&radius={radius}&key={_apiKey}";
                
                if (!string.IsNullOrEmpty(type))
                {
                    url += $"&type={type}";
                }

                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<GoogleMapsPlacesResponse>(content);
                }

                throw new Exception($"Google Maps API error: {response.StatusCode} - {content}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting nearby places: {ex.Message}");
            }
        }

        public async Task<GoogleMapsDirectionsResponse> GetDirectionsAsync(double originLat, double originLng, double destLat, double destLng, string mode = "driving")
        {
            try
            {
                var url = $"{_baseUrl}/directions/json?origin={originLat},{originLng}&destination={destLat},{destLng}&mode={mode}&key={_apiKey}";
                
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<GoogleMapsDirectionsResponse>(content);
                }

                throw new Exception($"Google Maps API error: {response.StatusCode} - {content}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting directions: {ex.Message}");
            }
        }

        public async Task<GoogleMapsDistanceMatrixResponse> GetDistanceMatrixAsync(double originLat, double originLng, List<double[]> destinations, string mode = "driving")
        {
            try
            {
                var destinationsStr = string.Join("|", destinations.Select(d => $"{d[0]},{d[1]}"));
                var url = $"{_baseUrl}/distancematrix/json?origins={originLat},{originLng}&destinations={destinationsStr}&mode={mode}&key={_apiKey}";
                
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<GoogleMapsDistanceMatrixResponse>(content);
                }

                throw new Exception($"Google Maps API error: {response.StatusCode} - {content}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting distance matrix: {ex.Message}");
            }
        }
    }

    // Response Models
    public class GoogleMapsGeocodeResponse
    {
        public string Status { get; set; }
        public List<GeocodeResult> Results { get; set; }
    }

    public class GeocodeResult
    {
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
        public List<AddressComponent> AddressComponents { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
        public string LocationType { get; set; }
        public Viewport Viewport { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Viewport
    {
        public Location Northeast { get; set; }
        public Location Southwest { get; set; }
    }

    public class AddressComponent
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public List<string> Types { get; set; }
    }

    public class GoogleMapsPlacesResponse
    {
        public string Status { get; set; }
        public List<PlaceResult> Results { get; set; }
    }

    public class PlaceResult
    {
        public string Name { get; set; }
        public Geometry Geometry { get; set; }
        public string PlaceId { get; set; }
        public List<string> Types { get; set; }
        public string Vicinity { get; set; }
        public double? Rating { get; set; }
        public int? UserRatingsTotal { get; set; }
    }

    public class GoogleMapsDirectionsResponse
    {
        public string Status { get; set; }
        public List<Route> Routes { get; set; }
    }

    public class Route
    {
        public List<Leg> Legs { get; set; }
        public OverviewPolyline OverviewPolyline { get; set; }
    }

    public class Leg
    {
        public Distance Distance { get; set; }
        public Duration Duration { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }
        public Location StartLocation { get; set; }
        public Location EndLocation { get; set; }
    }

    public class Distance
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class Duration
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class OverviewPolyline
    {
        public string Points { get; set; }
    }

    public class GoogleMapsDistanceMatrixResponse
    {
        public string Status { get; set; }
        public List<Element> Elements { get; set; }
    }

    public class Element
    {
        public Distance Distance { get; set; }
        public Duration Duration { get; set; }
        public string Status { get; set; }
    }
} 