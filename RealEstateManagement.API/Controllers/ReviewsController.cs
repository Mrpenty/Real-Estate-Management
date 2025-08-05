using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Review;
using RealEstateManagement.Business.Services.Reviews;

namespace RealEstateManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("add")]
        [Authorize] // Chỉ cho user đăng nhập
        public async Task<IActionResult> AddReview([FromBody] AddReviewDTO dto)
        {
            // Lấy renterId từ JWT hoặc claim nếu cần
            var renterId = int.Parse(User.FindFirst("id").Value);
            var (ok, msg) = await _reviewService.AddReviewAsync(dto.ContractId, dto.PropertyId, renterId, dto.ReviewText, dto.Rating);
            if (!ok) return BadRequest(msg);
            return Ok(msg);
        }

        [HttpPut("edit")]
        [Authorize]
        public async Task<IActionResult> EditReview([FromBody] EditReviewDTO dto)
        {
            var renterId = int.Parse(User.FindFirst("id").Value);
            var (ok, msg) = await _reviewService.EditReviewAsync(dto.ReviewId, renterId, dto.ReviewText);
            if (!ok) return BadRequest(msg);
            return Ok(msg);
        }

        [HttpPost("reply")]
        [Authorize(Roles = "Landlord")] // Chỉ landlord được trả lời
        public async Task<IActionResult> ReplyReview([FromBody] ReplyReviewDTO dto)
        {
            var landlordId = int.Parse(User.FindFirst("id").Value);
            var (ok, msg) = await _reviewService.AddReplyAsync(dto.ReviewId, landlordId, dto.ReplyContent);
            if (!ok) return BadRequest(msg);
            return Ok(msg);
        }

        [HttpPut("reply/edit")]
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> EditReply([FromBody] EditReplyDTO dto)
        {
            var landlordId = int.Parse(User.FindFirst("id").Value);
            var (ok, msg) = await _reviewService.EditReplyAsync(dto.ReplyId, landlordId, dto.ReplyContent);
            if (!ok) return BadRequest(msg);
            return Ok(msg);
        }
    }
}
