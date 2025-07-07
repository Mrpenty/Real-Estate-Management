using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;

namespace RealEstateManagement.API.Controllers
{
   // [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/Admin/PropertyPosts")]
    public class PropertyPostsController : ControllerBase
    {
        private readonly IPropertyPostService _propertyPostService;
        private readonly ILogger<PropertyPostsController> _logger;

        public PropertyPostsController(IPropertyPostService propertyPostService, ILogger<PropertyPostsController> logger)
        {
            _propertyPostService = propertyPostService;
            _logger = logger;
        }

        // GET: api/Admin/PropertyPosts?status=pending&page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] string? status = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _propertyPostService.GetPostsByStatusAsync(status, page, pageSize);
            return Ok(result);
        }

        // GET: api/Admin/PropertyPosts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostDetail(int id)
        {
            var post = await _propertyPostService.GetPostDetailForAdminAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        // PUT: api/Admin/PropertyPosts/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdatePostStatus(int id, [FromBody] UpdatePostStatusDto dto)
        {
            var result = await _propertyPostService.UpdatePostStatusAsync(id, dto.Status);
            if (!result) return BadRequest("Cập nhật trạng thái thất bại");
            return Ok(new { message = "Cập nhật trạng thái thành công" });
        }
    }

    public class UpdatePostStatusDto
    {
        public string Status { get; set; } = string.Empty; 
    }
} 