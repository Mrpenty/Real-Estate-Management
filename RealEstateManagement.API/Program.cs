using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using RealEstateManagement.API.Extensions;
using RealEstateManagement.API.Hubs;
using RealEstateManagement.API.Middleware;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, ".env");
DotNetEnv.Env.Load(envPath);

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
;
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
app.UseCorsPolicy(app.Environment);

app.UseErrorHandlingMiddleware();
app.UseStaticFiles();


// Add CORS before other middleware
//app.UseCorsPolicy(app.Environment);

//app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub", options =>
{
    options.CloseOnAuthenticationExpiration = true;
});
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//    endpoints.MapHub<ChatHub>("/chatHub", options =>
//    {
//        options.CloseOnAuthenticationExpiration = true;
//    });
//});
app.UseSwaggerServices(app.Environment);
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();
