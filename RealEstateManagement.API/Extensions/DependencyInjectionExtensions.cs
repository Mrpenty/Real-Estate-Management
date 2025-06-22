using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Repositories.Token;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Business.Repositories.SearchProperties;
using RealEstateManagement.Business.Services.SearchProperties;
using RealEstateManagement.Business.Repositories.FavoriteRepository;
using RealEstateManagement.Business.Services.Favorite;
namespace RealEstateManagement.API.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
        {
            // Repository
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IPropertyPostRepository, PropertyPostRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();

            // Service
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IPropertyPostService, PropertyPostService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ISmsService, TwilioSmsService>();

            services.AddScoped<ISearchProRepo, SearchProRepo>();
            services.AddScoped<ISearchProService, SearchProService>();

            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IFavoriteService, FavoriteService>();

            services.AddScoped<IPropertyPostRepository, PropertyPostRepository>();
            services.AddScoped<IPropertyPostService, PropertyPostService>();
            services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
            services.AddScoped<IPropertyImageService, PropertyImageService>();
            services.AddScoped<IRentalContractRepository, RentalContractRepository>();
            services.AddScoped<IRentalContractService, RentalContractService>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IPropertyService, PropertyService>();


            return services;
        }
    }
}
    