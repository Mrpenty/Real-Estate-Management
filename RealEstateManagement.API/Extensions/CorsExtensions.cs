namespace RealEstateManagement.API.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("https://localhost:7160", "http://localhost:7160", "https://194.233.81.64:5001", "http://194.233.81.64:5001") 
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });

            return services;
        }

        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseCors("DefaultCorsPolicy");
            }
            else
            {
                app.UseCors("DefaultCorsPolicy");
            }

            return app;
        }
    }
}
