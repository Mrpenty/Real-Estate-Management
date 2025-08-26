using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy;                 
using Yarp.ReverseProxy.Configuration;
var builder = WebApplication.CreateBuilder(args);
var apiBase = builder.Configuration["Api:BaseUrl"] ?? "http://api:8080";
// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPresentation", policy =>
    {
        policy.WithOrigins("https://localhost:7160") // Update this to match your API URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add HttpClient and configure the named client for the API
builder.Services.AddHttpClient("REMApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7031/"); // Make sure this is your API's base URL
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        NameClaimType = JwtRegisteredClaimNames.Sub,
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
        ValidateLifetime = true
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("accessToken"))
            {
                context.Token = context.Request.Cookies["accessToken"];
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            // This stops the default redirection to a login page
            context.HandleResponse();
            // Redirect to your custom login page
            context.Response.Redirect("/Auth/Login?ReturnUrl=" + context.Request.Path);
            return Task.CompletedTask;
        }
    };
});
var routes = new[]
{
    new RouteConfig
    {
        RouteId = "api",
        ClusterId = "api-cluster",
        Match = new RouteMatch { Path = "/api/{**catch-all}" }
    }
};

var clusters = new[]
{
    new ClusterConfig
    {
        ClusterId = "api-cluster",
        Destinations = new Dictionary<string, DestinationConfig>
        {
            ["d1"] = new DestinationConfig { Address = apiBase } // ví dụ http://api:8080
        }
    }
};

builder.Services.AddReverseProxy().LoadFromMemory(routes, clusters);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Use CORS before authentication and authorization
app.UseCors("AllowPresentation");

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
