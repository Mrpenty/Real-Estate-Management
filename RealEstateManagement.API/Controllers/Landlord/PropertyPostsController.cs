using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers.Landlord
{
    [Authorize(Roles = "Landlord")]
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyPostsController : ControllerBase
    {
        private readonly IPropertyPostService _propertyPostService;
        private readonly ILogger<PropertyPostsController> _logger;
        private readonly RentalDbContext _context;

        public PropertyPostsController(IPropertyPostService propertyPostService, ILogger<PropertyPostsController> logger)
        {
            _propertyPostService = propertyPostService;
            _logger = logger;
        }

        //Landlord tạo 1 bài đăng mới với status Draft
        [HttpPost]
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

                var propertyId = await _propertyPostService.CreatePropertyPostAsync(dto, landlordId);
                return Ok(new { propertyId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating property post");
                return StatusCode(500, "An error occurred while creating the property post");
            }
        }

        // ✅ Có sẵn để CreatedAtAction dùng được
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _propertyPostService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound();
            return Ok(new { id = post.Id, propertyId = post.PropertyId });
        }

        [HttpPut("{postId}/continue")]
        public async Task<IActionResult> ContinueDraft(int postId, [FromBody] ContinuePropertyPostDto dto)
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
                return Unauthorized("Không tìm thấy thông tin người dùng");

            if (!int.TryParse(userIdClaim.Value, out var landlordId))
                return Unauthorized("ID người dùng không hợp lệ");
            dto.PostId = postId; // Bắt buộc fix
            await _propertyPostService.ContinueDraftPostAsync(dto, landlordId);
            return NoContent();
        }

    }
}
