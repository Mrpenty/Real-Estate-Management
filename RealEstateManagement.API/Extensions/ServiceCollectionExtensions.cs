using Microsoft.Extensions.DependencyInjection;
using RealEstateManagement.Business.Services;

namespace RealEstateManagement.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAIServices(this IServiceCollection services)
        {
            // Đăng ký AI Recommendation Service
            services.AddScoped<IAIRecommendationService, AIRecommendationService>();
            
            // Đăng ký Google Maps Service
            services.AddScoped<IGoogleMapsService, GoogleMapsService>();
            
            // Đăng ký Device Location Service
            services.AddScoped<IDeviceLocationService, DeviceLocationService>();
            
            // Đăng ký Memory Cache cho Device Location
            services.AddMemoryCache();
            
            return services;
        }
    }
} 