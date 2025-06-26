using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace RealEstateManagement.API.Extensions
{
    public static partial class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                        NameClaimType = JwtRegisteredClaimNames.Sub,
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                        ValidateLifetime = true // Đảm bảo kiểm tra thời gian hết hạn
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            var logger = ctx.HttpContext.RequestServices.GetService<ILogger<JwtBearerEvents>>();
                            logger?.LogInformation("OnMessageReceived: Starting token extraction");

                            var authHeader = ctx.Request.Headers["Authorization"].FirstOrDefault();
                            logger?.LogInformation("OnMessageReceived: Authorization header: {AuthHeader}",
                                authHeader != null ? authHeader.Substring(0, Math.Min(50, authHeader.Length)) + "..." : "null");

                            string token = null;
                            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                token = authHeader.Substring("Bearer ".Length).Trim();
                                logger?.LogInformation("OnMessageReceived: Token extracted from Authorization header");
                            }
                            else
                            {
                                // Kiểm tra cookie nếu header không có
                                var accessToken = ctx.Request.Cookies["accessToken"];
                                if (!string.IsNullOrEmpty(accessToken))
                                {
                                    token = accessToken;
                                    logger?.LogInformation("OnMessageReceived: Token extracted from accessToken cookie");
                                }
                            }

                            if (!string.IsNullOrEmpty(token))
                            {
                                ctx.Token = token;
                                logger?.LogInformation("OnMessageReceived: Token set successfully: {Token}", token.Substring(0, Math.Min(50, token.Length)) + "...");
                            }
                            else
                            {
                                logger?.LogWarning("OnMessageReceived: No valid token found in header or cookie");
                                ctx.NoResult();
                            }

                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse(); // Ngăn chuyển hướng tự động
                            context.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetService<ILogger<JwtBearerEvents>>();
                            logger?.LogError("Authentication failed: {Error}", context.Exception.Message);
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }

}

