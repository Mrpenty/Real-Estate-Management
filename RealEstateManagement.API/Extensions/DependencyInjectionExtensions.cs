using RealEstateManagement.Business.Repositories.AddressRepo;
using RealEstateManagement.Business.Repositories.Chat.Conversations;
using RealEstateManagement.Business.Repositories.Chat.Messages;
using RealEstateManagement.Business.Repositories.FavoriteRepository;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Repositories.Package;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Repositories.Repository;
using RealEstateManagement.Business.Repositories.SearchProperties;
using RealEstateManagement.Business.Repositories.Token;
using RealEstateManagement.Business.Services;
using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Business.Services.Chat.Conversations;
using RealEstateManagement.Business.Services.Chat.Messages;
using RealEstateManagement.Business.Services.Favorite;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Business.Services.PromotionPackages;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Business.Services.SearchProperties;
using RealEstateManagement.Business.Services.UploadPicService;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Business.Services.Wallet;
using RealEstateManagement.Business.Repositories.NewsRepository;
using RealEstateManagement.Business.Services.NewsService;
using RealEstateManagement.Business.Repositories.NotificationRepository;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Business.Repositories.Reviews;
using RealEstateManagement.Business.Services.Reviews;
using RealEstateManagement.Business.Services.Admin;
using RealEstateManagement.Business.Repositories.Admin;
using RealEstateManagement.Business.Repositories.AmenityRepo;
using RealEstateManagement.Business.Repositories.PropertyTypeRepository;
using RealEstateManagement.Business.Services.PropertyTypeService;

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
            services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
            // Property Repository
            services.AddScoped<IPropertyPostRepository, PropertyPostRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IPropertyImageRepository, PropertyImageRepository>();
            services.AddScoped<IRentalContractRepository, RentalContractRepository>();
            services.AddScoped<IOwnerPropertyRepository, OwnerPropertyRepository>();
            services.AddScoped<IAmenityRepository, AmenityRepository>();
            services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();

            // Some support Repository
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ISearchProRepo, SearchProRepo>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IPromotionPackageRepository, PromotionPackageRepository>();
            services.AddScoped<IPropertyPromotionRepository, PropertyPromotionRepository>();
            services.AddScoped<ISliderRepository, SliderRepository>();
            //Chat Repository
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();

            //Notification Repository
            services.AddScoped<INotificationRepository, NotificationRepository>();

          

            //----Service----\\

            //Property Services
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IPropertyPostService, PropertyPostService>();
            services.AddScoped<IPropertyImageService, PropertyImageService>();
            services.AddScoped<IOwnerPropertyService, OwnerPropertyService>();
            services.AddScoped<IRentalContractService, RentalContractService>();
            services.AddScoped<ISliderService, SliderService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();

            //user and auth services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ISmsService, TwilioSmsService>();
            services.AddScoped<IProfileService, ProfileService>();

            //some suport services
            services.AddScoped<IUploadPicService, UploadPicService>();
            services.AddScoped<ISearchProService, SearchProService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IPromotionPackageService, PromotionPackageService>();
            services.AddScoped<IPropertyPromotionService, PropertyPromotionService>();

            //Chat Serivce
            services.AddScoped<IConversationService, ConversationService>();
            services.AddScoped<IMessageService, MessageService>();
            // News
            services.AddScoped<INewsRepository, NewsRepository>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<INewsImageRepository, NewsImageRepository>();
            services.AddScoped<INewImageService, NewImageService>();

            services.AddScoped<OpenAIService>();


            //Notification Service
            services.AddScoped<INotificationService, NotificationService>();

            //Admin Dashboard Service
            services.AddScoped<IAdminDashboardService, AdminDashboardService>();


            // Tenant Interaction Service
            //services.AddScoped<IInteractionService, InteractionService>();
            //services.AddScoped<IInteractionRepository, InteractionRepository>();



            //Wallet
            services.AddScoped<WalletService>();
            services.AddScoped<QRCodeService>();

            //Interested Property
            services.AddScoped<IInterestedPropertyRepository, InterestedPropertyRepository>();
            services.AddScoped<IInterestedPropertyService, InterestedPropertyService>();


            // logic kiểm tra và cập nhật bài hết hạn
            services.AddHostedService<ExpirePostBackgroundService>();
            //Review
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReviewService, ReviewService>();

            // AI Recommendation Services
            services.AddScoped<IAIRecommendationService, AIRecommendationService>();
            
            // Use OpenStreetMapService instead of Google Maps (free, no API key required)
            services.AddScoped<IOpenStreetMapService, OpenStreetMapService>();
           // services.AddScoped<IGoogleMapsService, OpenStreetMapService>(); // Alias for compatibility
            
            services.AddScoped<IDeviceLocationService, DeviceLocationService>();

            return services;
        }
    }
}
