using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Business.Repositories.Token;

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
    