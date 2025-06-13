using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Services.FavortiteProperties;
using RealEstateManagement.Business.Services.Properties;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPreferenceController : ControllerBase
    {
//        {
//  "loginIdentifier": "renter@example.com",
//  "password": "Renter@123"
//}
    private readonly IUserPreferenceService _userPreferenceService;

        public UserPreferenceController(IUserPreferenceService userPreferenceService)
        {
            _userPreferenceService = userPreferenceService;

        }
        [HttpPost("add")]
        public async Task<IActionResult> AddFavoriteProperty([FromBody] FavoritePropertyDTO dto)
        {
            try
            {
                // Lấy userId từ token
                //var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                int userId = 3;
                var result = await _userPreferenceService.AddFavoritePropertyAsync(userId, dto.PropertyId);

                if (!result)
                    return BadRequest("Bất động sản đã tồn tại trong danh sách yêu thích.");

                return Ok("Đã thêm vào danh sách yêu thích.");
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Đã xảy ra lỗi: {message}");
            }
        }
    }
}
