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
                        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
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

                            if (!string.IsNullOrEmpty(authHeader))
                            {
                                var prefix = authHeader.Length >= 7 ? authHeader.Substring(0, 7) : authHeader;
                                logger?.LogInformation("OnMessageReceived: Authorization header prefix: '{Prefix}'", prefix);
                            }

                            if (!string.IsNullOrEmpty(authHeader))
                            {
                                string token;

                                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                                {
                                    token = authHeader.Substring("Bearer ".Length).Trim();
                                    logger?.LogInformation("OnMessageReceived: Token extracted from Authorization header with Bearer prefix");
                                }
                                else
                                {
                                    token = authHeader.Trim();
                                    logger?.LogInformation("OnMessageReceived: Token extracted from Authorization header (raw token, no Bearer prefix)");
                                }

                                if (!string.IsNullOrEmpty(token))
                                {
                                    ctx.Token = token;
                                    logger?.LogInformation("OnMessageReceived: Token set successfully");
                                }
                                else
                                {
                                    logger?.LogWarning("OnMessageReceived: Token is empty after extraction");
                                    ctx.NoResult();
                                }
                            }
                            else
                            {
                                logger?.LogWarning("OnMessageReceived: No Authorization header found");
                                ctx.NoResult();
                            }

                            return Task.CompletedTask;
                        },






                    };
                });

            return services;
        }
    }
}
