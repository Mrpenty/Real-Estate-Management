using Microsoft.EntityFrameworkCore;

namespace RealEstateManagement.API.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RentalDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("MyCnn"),
                    sql =>
                    {
                        // Migrations nằm trong project Data của bạn
                        sql.MigrationsAssembly("RealEstateManagement.Data");

                        // Retry khi DB mới khởi động/chưa sẵn sàng
                        sql.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(5),
                            errorNumbersToAdd: null);

                        // (tuỳ chọn) tăng timeout lệnh
                        sql.CommandTimeout(60);
                    }));

            return services;
        }
    }
}
