using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Business.Services.SearchProperties;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchPropertiesController : ControllerBase
    {
        private readonly ISearchProService _propertyService;

        public SearchPropertiesController(ISearchProService propertyService)
        {
            _propertyService = propertyService;
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var results = await _propertyService.SearchAsync(keyword);
            return Ok(results);
        }
        [HttpPost("index")]
        public async Task<IActionResult> IndexProperty([FromBody] PropertySearchDTO dto)
        {
            var result = await _propertyService.IndexPropertyAsync(dto);
            return result ? Ok("Indexed successfully") : StatusCode(500, "Index failed");
        }

        [HttpPost("index/bulk")]
        public async Task<IActionResult> BulkIndex()
        {
            await _propertyService.BulkIndexPropertiesAsync();
            return Ok("Bulk indexing completed");
        }
    }
}
