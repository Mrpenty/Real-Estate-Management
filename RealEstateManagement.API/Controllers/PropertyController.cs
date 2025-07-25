﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Services.Favorite;
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
        //[Authorize(Roles = "Renter")]
        public async Task<ActionResult<IEnumerable<HomePropertyDTO>>> GetHomepageProperties()
        {
            try
            {
                var properties = await _propertyService.GetAllPropertiesAsync(0);

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
        //[Authorize(Roles = "Renter")]
        public async Task<ActionResult> GetPropertyById(int id, [FromQuery] int userId = 0)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id, userId);
            if (property == null)
            {
                return NotFound("Không tìm thấy bất động sản nào");
            }
            return Ok(property);
        }
        // Sắp xếp theo price
        [HttpGet("filter-by-price")]

        //[Authorize(Roles = "Renter")]
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

        //[Authorize(Roles = "Renter")]
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
            if (filter.Provinces.Count == 1 && filter.Provinces[0] == 0) filter.Provinces = new List<int>();
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

        // GET: api/PropertySearch/search?query=apartment

        //[HttpGet("search-advanced")]
        //public async Task<IActionResult> SearchAdvanced([FromQuery] string province = "", [FromQuery] string ward = "", [FromQuery] string street = "", [FromQuery] int? userId = 0)
        //{
        //    var results = await _propertyService.SearchAdvanceAsync(province, ward, street, userId);
        //    return Ok(results);
        //}

        [HttpGet("list-location")]
        public async Task<IActionResult> ListLocation()
        {
            var results = await _propertyService.GetListLocationAsync();
            return Ok(results);
        }

        [HttpGet("properties-renter")]
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> GetPropertiesByRenter()
        {
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Đăng nhập trước lấy danh sách");
            var result = await _propertyService.GetPropertiesByUserAsync(userId);
            return Ok(result);
        }


        [HttpGet("provinces")]
        public async Task<IActionResult> GetProvinces()
        {
            var provinces = await _propertyService.GetProvincesAsync();
            return Ok(provinces);
        }

        [HttpGet("wards/{provinceId}")]
        public async Task<IActionResult> GetWards(int provinceId)
        {
            var wards = await _propertyService.GetWardsAsync(provinceId);
            return Ok(wards);
        }

        [HttpGet("streets/{wardId}")]
        public async Task<IActionResult> GetStreets(int wardId)
        {
            var streets = await _propertyService.GetStreetAsync(wardId);
            return Ok(streets);
        }

        [HttpGet("amenities")]
        public async Task<IActionResult> GetAmenities()
        {
            var amenities = await _propertyService.GetAmenitiesAsync();
            return Ok(amenities);
        }
        [HttpGet("filter-by-type")]
        public async Task<IActionResult> FilterByType([FromQuery] string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return BadRequest("Bạn phải nhập loại bất động sản.");
            }

            try
            {
                var properties = await _propertyService.FilterByTypeAsync(type);

                if (properties == null || !properties.Any())
                {
                    return NotFound("Không tìm thấy bất động sản nào với loại đã cung cấp.");
                }

                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}