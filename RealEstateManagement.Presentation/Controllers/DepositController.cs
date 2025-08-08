using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateManagement.Presentation.Controllers
{
    public class DepositController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Action để xử lý khi PayOS redirect về sau thanh toán thành công
        [HttpGet("Deposit/Success")] // Hoặc đơn giản là [HttpGet("Success")] nếu có [Route] trên controller
        public IActionResult Success([FromQuery] long orderCode, [FromQuery] string code, [FromQuery] string desc)
        {
            ViewBag.OrderCode = orderCode; // Truyền orderCode sang View
            ViewBag.Message = "Giao dịch đã được ghi nhận."; // Có thể thêm logic kiểm tra status chi tiết hơn
            return View(); // Trả về Views/Deposit/Success.cshtml
        }

        // Action để xử lý khi PayOS redirect về sau thanh toán bị hủy/thất bại
        [HttpGet("Deposit/Cancel")] // Hoặc đơn giản là [HttpGet("Cancel")]
        public IActionResult Cancel([FromQuery] long orderCode, [FromQuery] string code, [FromQuery] string desc)
        {
            ViewBag.OrderCode = orderCode;
            ViewBag.Message = $"Giao dịch đã bị hủy hoặc không thành công: {desc}";
            return View(); // Trả về Views/Deposit/Cancel.cshtml
        }
    }
}
