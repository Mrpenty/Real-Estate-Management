using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.Services.Wallet;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers.Wallet
{
    public class DepositController : ControllerBase
    {
        public readonly QRCodeService _walletService;
        private readonly RentalDbContext _context;
        
        public DepositController (QRCodeService walletService, RentalDbContext context)
        {
            _walletService = walletService;
            _context = context;
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

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] decimal amount)
        {
            var userId = GetCurrentUserId();
            await _walletService.DepositAsync(userId, amount);
            return Ok(new { message = $"Nạp {amount} VNĐ thành công" });
        }

    }
}
