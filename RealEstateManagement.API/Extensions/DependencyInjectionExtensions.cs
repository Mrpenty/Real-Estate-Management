using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Repositories.Token;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Business.Repositories.AddressRepo;
using RealEstateManagement.Business.Repositories.Repository;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Business.Repositories.Chat;
using RealEstateManagement.Business.Services.Chat;
using RealEstateManagement.Business.Services.UploadPicService;

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
            // Generic Repository
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));

             //---Repository--\\
            services.AddScoped<ITokenRepository, TokenRepository>();

            // Property Repository
            services.AddScoped<IPropertyPostRepository, PropertyPostRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
            services.AddScoped<IRentalContractRepository, RentalContractRepository>();

            // Some support Repository
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ISearchProRepo, SearchProRepo>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();


            //----Service----\\

            //Property Services
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IPropertyPostService, PropertyPostService>();
            services.AddScoped<IPropertyImageService, PropertyImageService>();
            services.AddScoped<IPropertyImageService, PropertyImageService>();

            //user and auth services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ISmsService, SmsService>();
            services.AddScoped<IProfileService, ProfileService>();

            //some suport services
            services.AddScoped<IUploadPicService, UploadPicService>();
            services.AddScoped<ISearchProService, SearchProService>();
            services.AddScoped<IFavoriteService, FavoriteService>();


            return services;
        }
    }
}
    