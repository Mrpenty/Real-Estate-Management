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
                    return NotFound("No properties found.");

                return Ok(properties);
            }
            catch (Exception ex)
            {
                // Ghi log nếu có hệ thống log (khuyên dùng)
                // _logger.LogError(ex, "Failed to get homepage properties");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPropertyById(int id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
            {
                throw new KeyNotFoundException();
            }
            return Ok(property);
        }
        [HttpGet("filter-by-price")]
        public async Task<ActionResult<IEnumerable<HomePropertyDTO>>> FilterByPrice(decimal minPrice, decimal maxPrice)
        {
            try
            {
                var properties = await _propertyService.FilterByPriceAsync(minPrice, maxPrice);

                if (properties == null || !properties.Any())
                    return NotFound("No properties found.");

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
