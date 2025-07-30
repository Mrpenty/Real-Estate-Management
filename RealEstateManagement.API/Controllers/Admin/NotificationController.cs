using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Business.Services.NotificationService;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
   // [Authorize(Roles = "Admin")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDTO createDto)
        {
            try
            {
                var result = await _notificationService.CreateNotificationAsync(createDto);
                return Ok(new { success = true, data = result, message = "Notification created successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while creating the notification" });
            }
        }

        [HttpPost("send-to-all")]
        public async Task<IActionResult> SendToAllUsers([FromBody] CreateNotificationDTO createDto)
        {
            try
            {
                var result = await _notificationService.SendNotificationToAllUsersAsync(createDto);
                return Ok(new { success = true, message = "Notification sent to all users successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while sending the notification" });
            }
        }

        [HttpPost("send-to-renters")]
        public async Task<IActionResult> SendToRenters([FromBody] CreateNotificationDTO createDto)
        {
            try
            {
                var result = await _notificationService.SendNotificationToRentersAsync(createDto);
                return Ok(new { success = true, message = "Notification sent to renters successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while sending the notification" });
            }
        }

        [HttpPost("send-to-landlords")]
        public async Task<IActionResult> SendToLandlords([FromBody] CreateNotificationDTO createDto)
        {
            try
            {
                var result = await _notificationService.SendNotificationToLandlordsAsync(createDto);
                return Ok(new { success = true, message = "Notification sent to landlords successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while sending the notification" });
            }
        }

        [HttpPost("send-to-specific")]
        public async Task<IActionResult> SendToSpecificUsers([FromBody] CreateNotificationDTO createDto)
        {
            try
            {
                var result = await _notificationService.SendNotificationToSpecificUsersAsync(createDto);
                return Ok(new { success = true, message = "Notification sent to specific users successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while sending the notification" });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateNotification([FromBody] UpdateNotificationDTO updateDto)
        {
            try
            {
                var result = await _notificationService.UpdateNotificationAsync(updateDto);
                return Ok(new { success = true, data = result, message = "Notification updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while updating the notification" });
            }
        }

        [HttpDelete("delete/{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            try
            {
                var result = await _notificationService.DeleteNotificationAsync(notificationId);
                if (result)
                {
                    return Ok(new { success = true, message = "Notification deleted successfully" });
                }
                return NotFound(new { success = false, message = "Notification not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while deleting the notification" });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var notifications = await _notificationService.GetAllNotificationsAsync();
                return Ok(new { success = true, data = notifications });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving notifications" });
            }
        }

        [HttpGet("by-audience/{audience}")]
        public async Task<IActionResult> GetNotificationsByAudience(string audience)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationsByAudienceAsync(audience);
                return Ok(new { success = true, data = notifications });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving notifications" });
            }
        }

        [HttpGet("{notificationId}")]
        public async Task<IActionResult> GetNotificationById(int notificationId)
        {
            try
            {
                var notification = await _notificationService.GetNotificationByIdAsync(notificationId);
                if (notification != null)
                {
                    return Ok(new { success = true, data = notification });
                }
                return NotFound(new { success = false, message = "Notification not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving the notification" });
            }
        }
    }
} 