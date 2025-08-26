using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Business.Services.Wallet;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers.Landlord
{
    [Authorize(Roles = "Landlord")]
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyPostsController : ControllerBase
    {
        private readonly IPropertyPostService _propertyPostService;
        private readonly ILogger<PropertyPostsController> _logger;
        private readonly RentalDbContext _context;
        private readonly OpenAIService _openAIService;
        private readonly WalletService _walletService;

        public PropertyPostsController(IPropertyPostService propertyPostService, ILogger<PropertyPostsController> logger, OpenAIService openAIService, WalletService walletService)
        {
            _propertyPostService = propertyPostService;
            _logger = logger;
            _openAIService = openAIService;
            _walletService = walletService;
        }

        //Landlord tạo 1 bài đăng mới với status Draft
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePropertyPost([FromBody] PropertyCreateRequestDto dto)
        {
            try
            {
                // ✅ An toàn hơn khi lấy id từ claim
                var userIdClaim = User.FindFirst("id");
                if (userIdClaim == null)
                    return Unauthorized("Không tìm thấy thông tin người dùng");

                if (!int.TryParse(userIdClaim.Value, out var landlordId))
                    return Unauthorized("ID người dùng không hợp lệ");

                var propertyId = await _propertyPostService.CreatePropertyPostAsync(dto, landlordId);
                return Ok(new { propertyId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating property post");
                return StatusCode(500, "An error occurred while creating the property post");
            }
        }

        // ✅ Có sẵn để CreatedAtAction dùng được
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _propertyPostService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound();
            return Ok(new { id = post.Id, propertyId = post.PropertyId });
        }

        [HttpPut("{postId}/continue")]
        public async Task<IActionResult> ContinueDraft(int postId, [FromBody] ContinuePropertyPostDto dto)
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
                return Unauthorized("Không tìm thấy thông tin người dùng");

            if (!int.TryParse(userIdClaim.Value, out var landlordId))
                return Unauthorized("ID người dùng không hợp lệ");
            dto.PostId = postId; // Bắt buộc fix
            await _propertyPostService.ContinueDraftPostAsync(dto, landlordId);
            return NoContent();
        }

        [HttpPost("suggest-description")]
        public async Task<IActionResult> SuggestDescription([FromBody] RealEstateDescriptionRequest dto)
        {
            try
            {
                const decimal COST_PER_AI_SUGGESTION = 100m;
                var userIdClaim = User.FindFirst("id");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var landlordId))
                {
                    _logger.LogWarning("Unauthorized access attempt to AI suggestion: User ID claim missing or invalid.");
                    return Unauthorized(new { message = "Không tìm thấy thông tin người dùng hoặc ID không hợp lệ.", code = "USER_NOT_IDENTIFIED" });
                }
                var deductionSuccessful = await _walletService.DeductBalanceAsync(landlordId, COST_PER_AI_SUGGESTION, "Phí gợi ý mô tả AI");

                if (!deductionSuccessful)
                {
                    _logger.LogInformation($"User {landlordId} attempted AI suggestion but had insufficient balance.");
                    return BadRequest(new { message = "Số dư trong ví của bạn không đủ để sử dụng tính năng này. Vui lòng nạp thêm tiền.", code = "INSUFFICIENT_BALANCE" });
                }

                var result = await _openAIService.AskGPTAsync(dto);
                return Ok(new { suggestedDescription = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi nội bộ khi xử lý yêu cầu AI của bạn.", detailedError = ex.Message, code = "INTERNAL_SERVER_ERROR" });
            }
        }
    }
}
