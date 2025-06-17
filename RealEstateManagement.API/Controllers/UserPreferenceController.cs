using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPreferenceController : ControllerBase
    {
        //private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserPreferenceController(UserManager<ApplicationUser> userManager)
        {

            _userManager = userManager;
        }
        [HttpGet("get-user-info")]
        public IActionResult GetUserInfoFromToken()
        {
            try
            {
                var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(accessToken);

                var userId = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                var email =
                    token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ??
                    token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ??
                    token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
                var username = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;

                return Ok(new { userId, email, username });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi đọc token: {ex.Message}");
            }
        }


    }
}
