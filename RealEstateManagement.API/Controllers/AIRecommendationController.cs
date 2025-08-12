using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Location;
using RealEstateManagement.Business.Services;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIRecommendationController : ControllerBase
    {
        private readonly IAIRecommendationService _aiRecommendationService;

        public AIRecommendationController(IAIRecommendationService aiRecommendationService)
        {
            _aiRecommendationService = aiRecommendationService;
        }

        /// <summary>
        /// Lấy danh sách bất động sản được AI recommend dựa trên vị trí thiết bị
        /// </summary>
        /// <param name="request">Thông tin vị trí và tiêu chí tìm kiếm</param>
        /// <returns>Danh sách bất động sản được recommend</returns>
        [HttpPost("location-based")]
        [AllowAnonymous]
        public async Task<ActionResult<LocationRecommendationResponse>> GetLocationBasedRecommendations(
            [FromBody] LocationRecommendationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var recommendations = await _aiRecommendationService.GetLocationBasedRecommendationsAsync(request);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Lấy danh sách bất động sản gần vị trí cụ thể
        /// </summary>
        /// <param name="latitude">Vĩ độ</param>
        /// <param name="longitude">Kinh độ</param>
        /// <param name="radiusKm">Bán kính tìm kiếm (km)</param>
        /// <param name="maxResults">Số kết quả tối đa</param>
        /// <returns>Danh sách bất động sản gần đó</returns>
        [HttpGet("nearby")]
        [AllowAnonymous]
        public async Task<ActionResult> GetNearbyProperties(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            [FromQuery] double radiusKm = 10,
            [FromQuery] int maxResults = 20)
        {
            try
            {
                if (latitude < -90 || latitude > 90)
                    return BadRequest("Latitude phải nằm trong khoảng -90 đến 90");

                if (longitude < -180 || longitude > 180)
                    return BadRequest("Longitude phải nằm trong khoảng -180 đến 180");

                if (radiusKm <= 0 || radiusKm > 100)
                    return BadRequest("Bán kính phải nằm trong khoảng 0.1 đến 100 km");

                if (maxResults <= 0 || maxResults > 100)
                    return BadRequest("Số kết quả tối đa phải nằm trong khoảng 1 đến 100");

                var properties = await _aiRecommendationService.GetNearbyPropertiesAsync(
                    latitude, longitude, radiusKm, maxResults);

                return Ok(new
                {
                    properties = properties,
                    totalResults = properties.Count,
                    searchRadius = radiusKm,
                    userLocation = new { latitude, longitude }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin tiện ích gần vị trí
        /// </summary>
        /// <param name="latitude">Vĩ độ</param>
        /// <param name="longitude">Kinh độ</param>
        /// <param name="radiusKm">Bán kính tìm kiếm (km)</param>
        /// <returns>Danh sách tiện ích gần đó</returns>
        [HttpGet("nearby-amenities")]
        [AllowAnonymous]
        public async Task<ActionResult> GetNearbyAmenities(
            [FromQuery] double latitude,
            [FromQuery] double longitude,
            [FromQuery] double radiusKm = 5)
        {
            try
            {
                var amenities = await _aiRecommendationService.GetNearbyAmenitiesAsync(latitude, longitude, radiusKm);
                return Ok(new
                {
                    amenities = amenities,
                    searchRadius = radiusKm,
                    userLocation = new { latitude, longitude }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Lấy thông tin giao thông gần vị trí
        /// </summary>
        /// <param name="latitude">Vĩ độ</param>
        /// <param name="longitude">Kinh độ</param>
        /// <returns>Thông tin giao thông</returns>
        [HttpGet("transportation-info")]
        [AllowAnonymous]
        public async Task<ActionResult> GetTransportationInfo(
            [FromQuery] double latitude,
            [FromQuery] double longitude)
        {
            try
            {
                var transportationInfo = await _aiRecommendationService.GetTransportationInfoAsync(latitude, longitude);
                return Ok(new
                {
                    transportationInfo = transportationInfo,
                    userLocation = new { latitude, longitude }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Tính khoảng cách giữa hai điểm
        /// </summary>
        /// <param name="lat1">Vĩ độ điểm 1</param>
        /// <param name="lon1">Kinh độ điểm 1</param>
        /// <param name="lat2">Vĩ độ điểm 2</param>
        /// <param name="lon2">Kinh độ điểm 2</param>
        /// <returns>Khoảng cách tính bằng km</returns>
        [HttpGet("calculate-distance")]
        [AllowAnonymous]
        public async Task<ActionResult> CalculateDistance(
            [FromQuery] double lat1,
            [FromQuery] double lon1,
            [FromQuery] double lat2,
            [FromQuery] double lon2)
        {
            try
            {
                var distance = await _aiRecommendationService.CalculateDistanceAsync(lat1, lon1, lat2, lon2);
                return Ok(new
                {
                    distanceKm = Math.Round(distance, 2),
                    point1 = new { latitude = lat1, longitude = lon1 },
                    point2 = new { latitude = lat2, longitude = lon2 }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
} 