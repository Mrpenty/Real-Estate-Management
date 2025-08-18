using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Business.Services.Wallet;
using RealEstateManagement.Data.Entity.Payment;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers.Wallet
{
   // [Authorize(Roles = "Renter,Landlord")]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly WalletService _walletService;

        public WalletController(WalletService walletService)
        {
            _walletService = walletService;
        }
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Không tìm thấy thông tin người dùng");

            if (!int.TryParse(userIdClaim.Value, out var Id))
                throw new UnauthorizedAccessException("ID người dùng không hợp lệ");

            return Id;
        }


        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            try
            {
                var userId = GetCurrentUserId(); // tự viết extension lấy ID từ token
            var balance = await _walletService.GetBalanceAsync(userId);
                var walletId = await _walletService.GetWalletIdByUserIdAsync(userId);
                return Ok(new { balance = balance, walletId = walletId });
            }
            catch (Exception ex)
            {
                // Trả về lỗi JSON chuẩn để frontend dễ xử lý
                return StatusCode(500, new { message = "Đã xảy ra lỗi nội bộ khi lấy số dư ví.", detailedError = ex.Message, code = "INTERNAL_SERVER_ERROR" });
            }
        }

        [HttpGet("transaction-history")]
        public async Task<IActionResult> GetTransactionHistory([FromQuery] int walletId)
        {
            try
            {
                var transactions = await _walletService.ViewTransactionHistory(walletId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
