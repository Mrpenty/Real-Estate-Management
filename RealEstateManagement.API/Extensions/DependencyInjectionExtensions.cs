using RealEstateManagement.Business.Repositories.impl;
using RealEstateManagement.Business.Repositories;
using RealEstateManagement.Business.Services.Auth;

namespace RealEstateManagement.API.Extensions
{
  
        public static class DependencyInjectionExtensions
        {
            public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
            {
            services.AddScoped<ITokenRepository, TokenRepository>();

            services.AddScoped<IAuthService, AuthService>();


            return services;
            }
        }
    
}
    