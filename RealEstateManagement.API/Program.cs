using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using RealEstateManagement.API.Extensions;
using RealEstateManagement.API.Hubs;
using RealEstateManagement.API.Middleware;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;

var builder = WebApplication.CreateBuilder(args);
//signalr để chat
builder.Services.AddSignalR()
    .AddMessagePackProtocol()
    .AddHubOptions<ChatHub>(options =>
    {
        options.EnableDetailedErrors = true;
        options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
        options.HandshakeTimeout = TimeSpan.FromSeconds(15);
    });


builder.Services.AddControllersServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddCorsServices(builder.Configuration, builder.Environment);
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.AddSwaggerServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDependencyInjectionServices();

// ✅ Thêm PayOS từ cấu hình
builder.Services.AddSingleton(new PayOS(
   clientId: builder.Configuration["PayOS:ClientId"],
   apiKey: builder.Configuration["PayOS:ApiKey"],
   checksumKey: builder.Configuration["PayOS:ChecksumKey"]
));


builder.Services.AddElasticsearch(builder.Configuration);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RentalDbContext>();
    db.Database.Migrate();   // áp dụng migrations -> tạo bảng
}
app.UseRouting();
app.UseErrorHandlingMiddleware();

app.UseCorsPolicy(app.Environment);

// Add CORS before other middleware
app.UseCorsPolicy(app.Environment);

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub", options =>
    {
        options.CloseOnAuthenticationExpiration = true;
    });
});
app.MapControllers();
app.UseSwaggerServices(app.Environment);
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();
