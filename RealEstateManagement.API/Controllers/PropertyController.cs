using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Services.Properties;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }
        [HttpGet("homepage-allproperty")]
        public async Task<ActionResult<IEnumerable<HomePropertyDTO>>> GetHomepageProperties()
        {
            try
            {
                var properties = await _propertyService.GetAllPropertiesAsync();

                if (properties == null || !properties.Any())
                    return NotFound("Không tìm thấy bất động sản nào");

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPropertyById(int id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
            {
                return NotFound("Không tìm thấy bất động sản nào");
            }
            return Ok(property);
        }
        [HttpGet("filter-by-price")]
        public async Task<ActionResult<IEnumerable<HomePropertyDTO>>> FilterByPrice(decimal minPrice, decimal maxPrice)
        {
            try
            {
                if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
                    return BadRequest("Khoảng giá không hợp lệ.");
                var properties = await _propertyService.FilterByPriceAsync(minPrice, maxPrice);

                if (properties == null || !properties.Any())
                    return NotFound("Không tìm thấy bất động sản nào trong khoảng giá.");

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("filter-by-area")]
        public async Task<ActionResult<IEnumerable<HomePropertyDTO>>> FilterByArea(decimal minArea, decimal maxArea)
        {
            try
            {
                if (minArea < 0 || maxArea < 0 || minArea > maxArea)
                {
                    return BadRequest("Diện tích không hợp lệ.");
                }
                var properties = await _propertyService.FilterByAreaAsync(minArea, maxArea);

                if (properties == null || !properties.Any())
                    return NotFound("Không tìm thấy bất động sản nào");

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
