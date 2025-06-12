using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RealEstateManagement.Business.DTO;
using RealEstateManagement.Business.DTO.AuthDTO;
using RealEstateManagement.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace RealEstateManagement.Business.Repositories.Token
{
    public class TokenRepository : ITokenRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenRepository> _logger;

        public TokenRepository(UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<TokenRepository> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }


        public async Task<TokenDTO> CreateJWTTokenAsync(ApplicationUser user, bool populateExp)
        {
            var signingCredentials = GetSigningCreadentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            user.RefreshToken = refreshToken;

            if (populateExp)
            {
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            }

            await _userManager.UpdateAsync(user);


            return new TokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }


        public async Task<TokenDTO> RefreshJWTTokenAsync(TokenDTO tokenDTO)
        {
            var principal = GetClaimsPrincipalFromExpiredToken(tokenDTO.AccessToken);

            var email = principal.FindFirstValue(JwtRegisteredClaimNames.Email) ?? principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                throw new SecurityTokenException("Email claim not found in token.");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user is null || user.RefreshToken != tokenDTO.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new SecurityTokenException("Invalid or expired refresh token.");
            }

            return await CreateJWTTokenAsync(user, populateExp: false);
        }

        public void SetTokenCookie(TokenDTO tokenDTO, HttpContext context)
        {
            context.Response.Cookies.Append("accessToken", tokenDTO.AccessToken,
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddDays(5),
                        HttpOnly = true,
                        IsEssential = true,
                        Secure = false,
                        SameSite = SameSiteMode.Lax
                    }
                );

            context.Response.Cookies.Append("refreshToken", tokenDTO.RefreshToken,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax
                }
            );
        }

        public void DeleteTokenCookie(HttpContext context)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            };

            context.Response.Cookies.Delete("accessToken", cookieOptions);
            context.Response.Cookies.Delete("refreshToken", cookieOptions);
        }











        private SigningCredentials GetSigningCreadentials()
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var signingKey = new SymmetricSecurityKey(key);
            return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Name, user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT:ExpiryMinutes"])),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JWT");

            var TokenValidationParameters = new TokenValidationParameters
            {

                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
                ValidateLifetime = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token, TokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid Token");
            }

            return principal;
            }

        }

    }
