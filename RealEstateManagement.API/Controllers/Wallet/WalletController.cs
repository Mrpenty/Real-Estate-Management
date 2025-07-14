using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Business.Services.Wallet;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers.Wallet
{
    [Authorize(Roles = "Renter,Landlord,Admin")]
    [ApiController]
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
            var userId = GetCurrentUserId(); // tự viết extension lấy ID từ token
            var balance = await _walletService.GetBalanceAsync(userId);
            return Ok(balance);
        }

    }
}
