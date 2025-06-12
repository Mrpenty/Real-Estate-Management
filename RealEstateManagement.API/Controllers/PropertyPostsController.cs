using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services.OwnerService;

namespace RealEstateManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyPostsController : ControllerBase
    {
        private readonly IPropertyPostService _propertyPostService;

        public PropertyPostsController(IPropertyPostService propertyPostService)
        {
            _propertyPostService = propertyPostService;
        }

        [HttpPost]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> CreatePost([FromBody] PropertyCreateRequestDto dto)
        {
            // 👇 Lấy id landlord từ token
            var landlordId = int.Parse(User.FindFirst("id").Value);

            var postId = await _propertyPostService.CreatePropertyPostAsync(dto, landlordId);
            return CreatedAtAction(nameof(GetPostById), new { id = postId }, new { id = postId });
        }

        // Optional: Xem lại 1 post (để phục vụ CreatedAtAction)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            // Tạm return 200 đơn giản
            return Ok(new { id });
        }
    }

}
