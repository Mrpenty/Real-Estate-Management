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
        //[Authorize(Roles = "Renter")]
        public async Task<ActionResult<IEnumerable<HomePropertyDTO>>> FilterByPrice(decimal? minPrice, decimal? maxPrice)
        {
            try
            {
                if ((minPrice.HasValue && minPrice < 0) || (maxPrice.HasValue && maxPrice < 0))
                    return BadRequest("Giá tiền không hợp lệ.");

                if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
                    return BadRequest("Giá trị minPrice phải nhỏ hơn hoặc bằng maxPrice.");

                var properties = await _propertyService.FilterByPriceAsync(minPrice, maxPrice);

                if (properties == null || !properties.Any())
                    return NotFound("Không tìm thấy bất động sản nào");

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
            try
            {
                if ((minArea.HasValue && minArea < 0) || (maxArea.HasValue && maxArea < 0))
                    return BadRequest("Diện tích không hợp lệ.");

                if (minArea.HasValue && maxArea.HasValue && minArea > maxArea)
                    return BadRequest("Giá trị minArea phải nhỏ hơn hoặc bằng maxArea.");

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
        //[Authorize(Roles = "Renter")]
        public async Task<IActionResult> FilterAdvanced(PropertyFilterDTO filter)
        {
            if (filter.MinPrice == 0) filter.MinPrice = null;
            if (filter.MaxPrice == 0) filter.MaxPrice = null;
            // Tương tự với Area và Bedrooms
            if (filter.MinArea == 0) filter.MinArea = null;
            if (filter.MaxArea == 0) filter.MaxArea = null;
            if (filter.MinBedrooms == 0) filter.MinBedrooms = null;
            if (filter.MaxBedrooms == 0) filter.MaxBedrooms = null;
            if ((filter.MinPrice.HasValue && filter.MinPrice < 0) ||
                (filter.MaxPrice.HasValue && filter.MaxPrice < 0) ||
                (filter.MinPrice.HasValue && filter.MaxPrice.HasValue && filter.MinPrice > filter.MaxPrice))
                return BadRequest("Giá không hợp lệ.");

            if ((filter.MinArea.HasValue && filter.MinArea < 0) ||
                (filter.MaxArea.HasValue && filter.MaxArea < 0) ||
                (filter.MinArea.HasValue && filter.MaxArea.HasValue && filter.MinArea > filter.MaxArea))
                return BadRequest("Diện tích không hợp lệ.");

            if ((filter.MinBedrooms.HasValue && filter.MinBedrooms < 0) ||
                (filter.MaxBedrooms.HasValue && filter.MaxBedrooms < 0) ||
                (filter.MinBedrooms.HasValue && filter.MaxBedrooms.HasValue && filter.MinBedrooms > filter.MaxBedrooms))
                return BadRequest("Số phòng ngủ không hợp lệ.");

            var result = await _propertyService.FilterAdvancedAsync(filter);
            if (result == null || !result.Any())
                return NotFound("Không tìm thấy bất động sản phù hợp.");

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
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> AddToFavorite([FromBody] FavoritePropertyDTO dto)
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
                return Unauthorized("Đăng nhập trước khi thêm vào danh sách yêu thích");
            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized("ID người dùng không hợp lệ");
            //var userIdClaim = int.Parse(User.FindFirst("id").Value);
            var result = await _propertyService.AddToFavoriteAsync(userId, dto.PropertyId);
            if (!result)
                return BadRequest("Đã có trong danh sách yêu thích hoặc dữ liệu không hợp lệ.");

            return Ok("Đã thêm vào danh sách yêu thích.");

        }
    }
}
