using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net.payOS.Types;
using RealEstateManagement.Business.Services.Wallet;
using RealEstateManagement.Data.Entity.Payment;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers.Wallet
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        public readonly QRCodeService _payOS;
        private readonly RentalDbContext _context;

        public DepositController(QRCodeService payOS, RentalDbContext context)
        {
            _payOS = payOS;
            _context = context;
        }


        [HttpPost("deposit")]
        public async Task<IActionResult> CreatePayment(
    [FromQuery] int userId,
    [FromQuery] decimal amount)
        {
            try
            {
                var url = await _payOS.CreatePaymentUrl(userId, amount);
                return Ok(new { url });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("check-payment-status")]
        public async Task<IActionResult> CheckPaymentStatus([FromQuery] long orderCode)
        {
            try
            {
                var result = await _payOS.CheckAndUpdateDepositStatusAsync(orderCode);
                return Ok(new { success = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("Success")] // action method lắng nghe "Success" => /api/Deposit/Success
        public async Task<IActionResult> PaymentSuccess(
        [FromQuery] long orderCode,
        [FromQuery] string code,
        [FromQuery] string desc)
        {
            // ... logic xử lý và redirect đến frontend ...
            return Redirect($"https://localhost:7160/Deposit/Success?orderCode={orderCode}&status={code}&desc={desc}");
        }

        [HttpGet("Cancel")] // tương tự cho /api/Deposit/Cancel
        public IActionResult PaymentCancel(
            [FromQuery] long orderCode,
            [FromQuery] string code,
            [FromQuery] string desc)
        {
            // ... logic xử lý và redirect đến frontend ...
            return Redirect("https://localhost:7160/Deposit/Cancel?orderCode=" + orderCode + "&message=" + desc);
        }

        //[HttpPost("payos_webhook")]
        //public async Task<IActionResult> PayOSWebhook([FromBody] WebhookType body)
        //{
        //    try
        //    {
        //        // Xác thực chữ ký và parse ra data
        //        WebhookData data = _payOS.VerifyPaymentWebhookData(body);

        //        // Xử lý cập nhật DB
        //        await _payOS.ProcessPayOSWebhookAsync(data);

        //        // Trả về OK để PayOS không gửi lại webhook nữa
        //        return Ok(new { code = 0, message = "Webhook processed successfully" });
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Webhook error: " + e.Message);
        //        return Ok(new { code = -1, message = "Failed to process webhook" });
        //    }
        //}

        //[HttpPost("confirm-webhook")]
        //public async Task<IActionResult> ConfirmWebhook()
        //{
        //    try
        //    {
        //        await _payOS.ConfirmWebhook();
        //        return Ok(new { code = 0, message = "Webhook confirmed" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { code = -1, message = ex.Message });
        //    }
        //}
    }
}
