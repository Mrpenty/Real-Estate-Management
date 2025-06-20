using Microsoft.EntityFrameworkCore;
using RealEstateManagement.API.Extensions;
using RealEstateManagement.API.Hub;
using RealEstateManagement.API.Middleware;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;

var builder = WebApplication.CreateBuilder(args);
//signalr để chat
builder.Services.AddSignalR()
    .AddMessagePackProtocol(); 


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddCorsServices(builder.Configuration, builder.Environment);


builder.Services.AddSwaggerServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDependencyInjectionServices();
var app = builder.Build();
//app.UseRouting();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//    endpoints.MapHub<NotificationHub>("/notificationHub"); // Route truy cập từ client
//});

app.UseErrorHandlingMiddleware();

app.UseCorsPolicy(app.Environment);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerServices(app.Environment);
app.Run();
