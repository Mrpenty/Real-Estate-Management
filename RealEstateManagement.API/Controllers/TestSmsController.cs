using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.Services.Mail;

namespace RealEstateManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestSmsController : ControllerBase
    {
        private readonly ISmsService _smsService;

        public TestSmsController(ISmsService smsService)
        {
            _smsService = smsService;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {
            try
            {
                await _smsService.SendOtpAsync(request.PhoneNumber, request.Otp);
                return Ok(new { success = true, message = "OTP sent successfully via Zalo" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("send-property-inquiry")]
        public async Task<IActionResult> SendPropertyInquiry([FromBody] PropertyInquiryRequest request)
        {
            try
            {
                await _smsService.SendPropertyInquiryAsync(request.PhoneNumber, request.PropertyTitle, request.ContactPhone);
                return Ok(new { success = true, message = "Property inquiry sent successfully via Zalo" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            try
            {
                await _smsService.SendNotificationAsync(request.PhoneNumber, request.Message);
                return Ok(new { success = true, message = "Notification sent successfully via Zalo" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("send-zalo-message")]
        public async Task<IActionResult> SendZaloMessage([FromBody] ZaloMessageRequest request)
        {
            try
            {
                await _smsService.SendZaloMessageAsync(request.PhoneNumber, request.Message);
                return Ok(new { success = true, message = "Zalo message sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }

    public class SendOtpRequest
    {
        public string PhoneNumber { get; set; }
        public string Otp { get; set; }
    }

    public class PropertyInquiryRequest
    {
        public string PhoneNumber { get; set; }
        public string PropertyTitle { get; set; }
        public string ContactPhone { get; set; }
    }

    public class NotificationRequest
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }

    public class ZaloMessageRequest
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
} 