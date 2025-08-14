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
    public class DeviceLocationController : ControllerBase
    {
        private readonly IDeviceLocationService _deviceLocationService;

        public DeviceLocationController(IDeviceLocationService deviceLocationService)
        {
            _deviceLocationService = deviceLocationService;
        }

        /// <summary>
        /// Lấy vị trí hiện tại của thiết bị
        /// </summary>
        /// <param name="deviceId">ID của thiết bị</param>
        /// <returns>Thông tin vị trí</returns>
        [HttpGet("current")]
        [AllowAnonymous]
        public async Task<ActionResult<LocationDTO>> GetCurrentLocation([FromQuery] string deviceId)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    return BadRequest("DeviceId không được để trống");
                }

                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                var location = await _deviceLocationService.GetDeviceLocationAsync(deviceId, ipAddress);
                
                return Ok(location);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật vị trí thiết bị
        /// </summary>
        /// <param name="request">Thông tin vị trí mới</param>
        /// <returns>Kết quả cập nhật</returns>
        [HttpPost("update")]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateLocation([FromBody] UpdateLocationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrEmpty(request.DeviceId))
                {
                    return BadRequest("DeviceId không được để trống");
                }

                var success = await _deviceLocationService.UpdateDeviceLocationAsync(
                    request.DeviceId, 
                    request.Latitude, 
                    request.Longitude, 
                    request.Address
                );

                if (success)
                {
                    return Ok(new { message = "Cập nhật vị trí thành công" });
                }
                else
                {
                    return BadRequest("Không thể cập nhật vị trí. Vui lòng kiểm tra tọa độ.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Lấy lịch sử vị trí gần đây của thiết bị
        /// </summary>
        /// <param name="deviceId">ID của thiết bị</param>
        /// <param name="count">Số lượng vị trí muốn lấy</param>
        /// <returns>Danh sách vị trí gần đây</returns>
        [HttpGet("history")]
        [AllowAnonymous]
        public async Task<ActionResult> GetLocationHistory([FromQuery] string deviceId, [FromQuery] int count = 5)
        {
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    return BadRequest("DeviceId không được để trống");
                }

                if (count <= 0 || count > 20)
                {
                    return BadRequest("Số lượng vị trí phải nằm trong khoảng 1-20");
                }

                var locations = await _deviceLocationService.GetRecentLocationsAsync(deviceId, count);
                
                return Ok(new
                {
                    deviceId = deviceId,
                    totalLocations = locations.Count,
                    locations = locations
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Lấy vị trí từ địa chỉ IP
        /// </summary>
        /// <param name="ipAddress">Địa chỉ IP</param>
        /// <returns>Thông tin vị trí</returns>
        [HttpGet("from-ip")]
        [AllowAnonymous]
        public async Task<ActionResult<LocationDTO>> GetLocationFromIP([FromQuery] string ipAddress)
        {
            try
            {
                if (string.IsNullOrEmpty(ipAddress))
                {
                    return BadRequest("IP Address không được để trống");
                }

                var location = await _deviceLocationService.GetLocationFromIPAsync(ipAddress);
                
                if (location != null)
                {
                    return Ok(location);
                }
                else
                {
                    return NotFound("Không thể xác định vị trí từ IP Address");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Xác thực tọa độ vị trí
        /// </summary>
        /// <param name="latitude">Vĩ độ</param>
        /// <param name="longitude">Kinh độ</param>
        /// <returns>Kết quả xác thực</returns>
        [HttpGet("validate")]
        [AllowAnonymous]
        public async Task<ActionResult> ValidateLocation([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                var isValid = await _deviceLocationService.ValidateLocationAsync(latitude, longitude);
                
                return Ok(new
                {
                    latitude = latitude,
                    longitude = longitude,
                    isValid = isValid,
                    message = isValid ? "Tọa độ hợp lệ" : "Tọa độ không hợp lệ"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class UpdateLocationRequest
    {
        public string DeviceId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
    }
} 