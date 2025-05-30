using Microsoft.EntityFrameworkCore;

namespace RealEstateManagement.API.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
           services.AddDbContext<RentalDbContext>(options =>
           {
               options.UseSqlServer(configuration.GetConnectionString("MyCnn"),
                   sql => sql.MigrationsAssembly("RealEstateManagement.Data")
                   );
           });
            return services;

        }

       
    }
}
