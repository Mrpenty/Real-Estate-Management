using Microsoft.EntityFrameworkCore;
using RealEstateManagement.API.Extensions;
using RealEstateManagement.Business.Hub;
using RealEstateManagement.Business.Hubs;
using RealEstateManagement.API.Middleware;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;

var builder = WebApplication.CreateBuilder(args);
//signalr để chat
builder.Services.AddSignalR()
    .AddMessagePackProtocol(); // thêm MessagePack
// Nếu cần CORS cho client khác domain
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Domain frontend
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddIdentityServices();

//
//
//
builder.Services.AddAuthenticationServices(builder.Configuration);


builder.Services.AddCorsServices(builder.Configuration, builder.Environment);
//Elasticsearch
builder.Services.AddElasticsearch(builder.Configuration);

builder.Services.AddSwaggerServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDependencyInjectionServices();
var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chathub"); // Route truy cập từ client
});

app.UseErrorHandlingMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add CORS before other middleware
app.UseCorsPolicy(app.Environment);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwaggerServices(app.Environment);
app.Run();
