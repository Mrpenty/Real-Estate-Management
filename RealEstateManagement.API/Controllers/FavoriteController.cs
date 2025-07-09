using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Services.Favorite;
using RealEstateManagement.Business.Services.Properties;
using System.IdentityModel.Tokens.Jwt;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favorteService)
        {
            _favoriteService = favorteService;
        }
        [HttpPost("add-favorite")]
        [Authorize]
        public async Task<IActionResult> AddToFavorite([FromBody] FavoritePropertyDTO dto)
        {
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Đăng nhập trước khi thêm vào danh sách yêu thích");
            
            var result = await _favoriteService.AddToFavoriteAsync(userId, dto.PropertyId);
            if (!result)
                return BadRequest("Đã có trong danh sách yêu thích hoặc dữ liệu không hợp lệ.");

            return Ok("Đã thêm vào danh sách yêu thích.");

        }
        [HttpDelete("favorite")]
        [Authorize]
        public async Task<IActionResult> RemoveFavoriteProperty([FromBody] FavoritePropertyDTO dto)
        {
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Đăng nhập trước khi xóa khỏi danh sách yêu thích");
            var result = await _favoriteService.RemoveFavoritePropertyAsync(userId, dto.PropertyId);
            if (!result) return NotFound("Không tìm thấy mục yêu thích để xóa.");
            return Ok("Đã xóa bất động sản khỏi danh sách yêu thích.");
        }
        [HttpGet("all-favorite")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<HomePropertyDTO>>> GetAllFavorites()
        {
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Đăng nhập trước khi hiện danh sách yêu thích");

            var properties = await _favoriteService.AllFavoritePropertyAsync(userId);

            if (properties == null || !properties.Any())
                return NotFound("Không có bất động sản yêu thích nào");

            return Ok(properties);
        }

    }
}
