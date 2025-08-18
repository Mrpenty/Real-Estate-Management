using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RealEstateManagement.Business.Services
{
    public class MockGoogleMapsService : IGoogleMapsService
    {
        private readonly IConfiguration _configuration;

        public MockGoogleMapsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GoogleMapsGeocodeResponse> GeocodeAddressAsync(string address)
        {
            // Mock response cho testing
            await Task.Delay(100); // Simulate API call delay

            var mockResponse = new GoogleMapsGeocodeResponse
            {
                Status = "OK",
                Results = new List<GeocodeResult>
                {
                    new GeocodeResult
                    {
                        FormattedAddress = address,
                        Geometry = new Geometry
                        {
                            Location = new Location
                            {
                                Lat = GetMockLatitude(address),
                                Lng = GetMockLongitude(address)
                            }
                        }
                    }
                }
            };

            return mockResponse;
        }

        public async Task<GoogleMapsPlacesResponse> GetNearbyPlacesAsync(double latitude, double longitude, double radius, string type = null)
        {
            await Task.Delay(100);

            var mockPlaces = new List<PlaceResult>();
            
            // Generate mock places based on type
            if (string.IsNullOrEmpty(type) || type == "restaurant")
            {
                mockPlaces.Add(new PlaceResult
                {
                    Name = "Nhà hàng ABC",
                    Vicinity = "123 Đường ABC, Quận 1",
                    Rating = 4.5,
                    UserRatingsTotal = 120,
                    Geometry = new Geometry
                    {
                        Location = new Location
                        {
                            Lat = latitude + 0.001,
                            Lng = longitude + 0.001
                        }
                    }
                });
            }

            if (string.IsNullOrEmpty(type) || type == "school")
            {
                mockPlaces.Add(new PlaceResult
                {
                    Name = "Trường Tiểu học XYZ",
                    Vicinity = "456 Đường XYZ, Quận 1",
                    Rating = 4.8,
                    UserRatingsTotal = 89,
                    Geometry = new Geometry
                    {
                        Location = new Location
                        {
                            Lat = latitude - 0.001,
                            Lng = longitude + 0.002
                        }
                    }
                });
            }

            if (string.IsNullOrEmpty(type) || type == "hospital")
            {
                mockPlaces.Add(new PlaceResult
                {
                    Name = "Bệnh viện Đa khoa",
                    Vicinity = "789 Đường DEF, Quận 1",
                    Rating = 4.7,
                    UserRatingsTotal = 156,
                    Geometry = new Geometry
                    {
                        Location = new Location
                        {
                            Lat = latitude + 0.002,
                            Lng = longitude - 0.001
                        }
                    }
                });
            }

            return new GoogleMapsPlacesResponse
            {
                Status = "OK",
                Results = mockPlaces
            };
        }

        public async Task<GoogleMapsDirectionsResponse> GetDirectionsAsync(double originLat, double originLng, double destLat, double destLng, string mode = "driving")
        {
            await Task.Delay(100);

            var distance = CalculateDistance(originLat, originLng, destLat, destLng);
            var duration = distance * 2; // 2 minutes per km

            return new GoogleMapsDirectionsResponse
            {
                Status = "OK",
                Routes = new List<Route>
                {
                    new Route
                    {
                        Legs = new List<Leg>
                        {
                            new Leg
                            {
                                Distance = new Distance
                                {
                                    Text = $"{distance:F1} km",
                                    Value = (int)(distance * 1000)
                                },
                                Duration = new Duration
                                {
                                    Text = $"{duration:F0} phút",
                                    Value = (int)(duration * 60)
                                },
                                StartAddress = $"Vị trí bắt đầu ({originLat:F6}, {originLng:F6})",
                                EndAddress = $"Vị trí đích ({destLat:F6}, {destLng:F6})"
                            }
                        }
                    }
                }
            };
        }

        public async Task<GoogleMapsDistanceMatrixResponse> GetDistanceMatrixAsync(double originLat, double originLng, List<double[]> destinations, string mode = "driving")
        {
            await Task.Delay(100);

            var elements = new List<Element>();
            foreach (var dest in destinations)
            {
                var distance = CalculateDistance(originLat, originLng, dest[0], dest[1]);
                elements.Add(new Element
                {
                    Distance = new Distance
                    {
                        Text = $"{distance:F1} km",
                        Value = (int)(distance * 1000)
                    },
                    Duration = new Duration
                    {
                        Text = $"{distance * 2:F0} phút",
                        Value = (int)(distance * 2 * 60)
                    },
                    Status = "OK"
                });
            }

            return new GoogleMapsDistanceMatrixResponse
            {
                Status = "OK",
                Elements = elements
            };
        }

        private double GetMockLatitude(string address)
        {
            // Return mock coordinates based on address content
            if (address.ToLower().Contains("hà nội"))
                return 21.0285;
            if (address.ToLower().Contains("tp hcm") || address.ToLower().Contains("hồ chí minh"))
                return 10.8231;
            if (address.ToLower().Contains("đà nẵng"))
                return 16.0544;
            if (address.ToLower().Contains("huế"))
                return 16.4637;
            
            return 21.0285; // Default to Hanoi
        }

        private double GetMockLongitude(string address)
        {
            if (address.ToLower().Contains("hà nội"))
                return 105.8542;
            if (address.ToLower().Contains("tp hcm") || address.ToLower().Contains("hồ chí minh"))
                return 106.6297;
            if (address.ToLower().Contains("đà nẵng"))
                return 108.2022;
            if (address.ToLower().Contains("huế"))
                return 107.5909;
            
            return 105.8542; // Default to Hanoi
        }

        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in km
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
    }
} 