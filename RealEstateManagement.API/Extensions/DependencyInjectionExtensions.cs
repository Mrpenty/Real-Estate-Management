using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Repositories.Token;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Business.Services.Chat;
using RealEstateManagement.Business.Repositories.Chat;


namespace RealEstateManagement.API.Extensions
{

    public static class DependencyInjectionExtensions
        {
            public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
            {
            services.AddScoped<ITokenRepository, TokenRepository>();

            services.AddScoped<IAuthService, AuthService>();


            services.AddScoped<IMailService, MailService>();

            services.AddScoped<ISmsService, TwilioSmsService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IChatRepository, ChatRepository>();


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
    