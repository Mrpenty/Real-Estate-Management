using RealEstateManagement.Business.Repositories.impl;
using RealEstateManagement.Business.Repositories;

namespace RealEstateManagement.API.Extensions
{
  
        public static class DependencyInjectionExtensions
        {
            public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
            {
            services.AddScoped<ITokenRepository, TokenRepository>();


            return services;
            }
        }
    
}
    