using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Business.Repositories.Token;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RealEstateManagement.API.Extensions
{

    public static class DependencyInjectionExtensions
        {
            public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
            {
            services.AddScoped<ITokenRepository, TokenRepository>();

            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IPropertyPostRepository, PropertyPostRepository>();
            services.AddScoped<IPropertyPostService, PropertyPostService>();
            services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
            services.AddScoped<IPropertyImageService, PropertyImageService>();
            services.AddScoped<IRentalContractRepository, RentalContractRepository>();
            services.AddScoped<IRentalContractService, RentalContractService>();
            return services;
            }
        }
    
}
    