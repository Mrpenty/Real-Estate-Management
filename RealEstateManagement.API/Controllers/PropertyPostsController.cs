using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services.OwnerService;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyPostsController : ControllerBase
    {
        private readonly IPropertyPostService _propertyPostService;
        private readonly ILogger<PropertyPostsController> _logger;

        public PropertyPostsController(IPropertyPostService propertyPostService, ILogger<PropertyPostsController> logger)
        {
            _propertyPostService = propertyPostService;
            _logger = logger;
        }

        [HttpPost]
       // [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> CreatePropertyPost([FromBody] PropertyCreateRequestDto dto)
        {
            try
            {
                // ✅ An toàn hơn khi lấy id từ claim
                var userIdClaim = User.FindFirst("id");
                if (userIdClaim == null)
                    return Unauthorized("Không tìm thấy thông tin người dùng");

                if (!int.TryParse(userIdClaim.Value, out var landlordId))
                    return Unauthorized("ID người dùng không hợp lệ");

                var postId = await _propertyPostService.CreatePropertyPostAsync(dto, landlordId);
                return CreatedAtAction(nameof(GetPostById), new { id = postId }, new { id = postId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi tạo bài đăng.", chiTiet = ex.Message });
            }
            
        }


        // ✅ Có sẵn để CreatedAtAction dùng được
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            // Sau này có thể thêm logic lấy chi tiết bài post
            return Ok(new { id });
        }
    }
}
