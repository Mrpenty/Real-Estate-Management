using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Services.Properties;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        [Authorize(Roles = "Renter")]
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
        //Lấy property theo id
        [HttpGet("{id}")]
        [Authorize(Roles = "Renter")]
        public async Task<ActionResult> GetPropertyById(int id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
            {
                return NotFound("Không tìm thấy bất động sản nào");
            }
            return Ok(property);
        }
        // Sắp xếp theo price
        [HttpGet("filter-by-price")]
        [Authorize(Roles = "Renter")]
        public async Task<ActionResult<IEnumerable<HomePropertyDTO>>> FilterByPrice([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            if (!minPrice.HasValue || !maxPrice.HasValue)
                return BadRequest("Bạn phải nhập cả minPrice và maxPrice.");
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
        //Sắp xếp theo diện tích
        [HttpGet("filter-by-area")]
        [Authorize(Roles = "Renter")]
        public async Task<ActionResult<IEnumerable<HomePropertyDTO>>> FilterByArea([FromQuery] decimal? minArea, [FromQuery] decimal? maxArea)
        {
            if (!minArea.HasValue || !maxArea.HasValue)
                return BadRequest("Bạn phải nhập cả minArea và maxArea.");
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
        //Filter nâng cao
        [HttpPost("filter-advanced")]
        public async Task<IActionResult> FilterAdvanced([FromBody] PropertyFilterDTO filter)
        {

            var result = await _propertyService.FilterAdvancedAsync(filter);
            return Ok(result);
        }

        //So sánh các property với nhau (tối đa là 3)
        [HttpPost("compare")]
        //[Authorize(Roles = "Renter")]
        public async Task<ActionResult<List<ComparePropertyDTO>>> CompareProperties([FromBody] List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("Vui lòng cung cấp danh sách ID bất động sản để so sánh.");
            }

            if (ids.Count > 3)
                return BadRequest("Chỉ được so sánh tối đa 3 bất động sản cùng lúc.");

            if (ids.Count <= 1)
                return BadRequest("So sánh ít nhất 2 bất động sản");
            // Check for duplicate IDs
            var uniqueIds = ids.Distinct().ToList();
            if (uniqueIds.Count != ids.Count)
            {
                var duplicateIds = ids.GroupBy(x => x)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();
                return BadRequest($"Không thể so sánh cùng một bất động sản. ID trùng lặp: {string.Join(", ", duplicateIds)}");
            }
            try
            {
                var comparedProperties = await _propertyService.ComparePropertiesAsync(ids);

                if (comparedProperties == null || !comparedProperties.Any())
                {
                    return NotFound("Không tìm thấy bất động sản nào với các ID đã cung cấp.");
                }

                return Ok(comparedProperties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }
        [HttpPost("add-favorite")]
        //[Authorize(Roles = "Renter")]
        public async Task<IActionResult> AddToFavorite([FromBody] FavoritePropertyDTO dto)
        {
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Đăng nhập trước khi thêm vào danh sách yêu thích");
            //var userIdClaim = int.Parse(User.FindFirst("id").Value);
            var result = await _propertyService.AddToFavoriteAsync(userId, dto.PropertyId);
            if (!result)
                return BadRequest("Đã có trong danh sách yêu thích hoặc dữ liệu không hợp lệ.");

            return Ok("Đã thêm vào danh sách yêu thích.");

        }

        // GET: api/PropertySearch/search?query=apartment
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
