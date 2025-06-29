using Humanizer;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Business.Services.Chat;
using RealEstateManagement.Business.Services.Chat.Conversations;
using RealEstateManagement.Business.Services.Chat.Messages;
using System.IdentityModel.Tokens.Jwt;

namespace RealEstateManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        private readonly IMessageService _messageService;

        public ChatController(IConversationService conversationService, IMessageService messageService)
        {
            _conversationService = conversationService;
            _messageService = messageService;
        }

        //Tạo cuộc trò chuyện mới
        [HttpPost("create-conversation")]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationDTO dto)
        {
            if (dto == null || dto.RenterId <= 0 || dto.LandlordId <= 0 || dto.PropertyId <= 0)
            {
                return BadRequest("Invalid conversation data.");
            }

            try
            {
                var conversation = await _conversationService.CreateConversationAsync(dto);
                if (conversation == null)
                {
                    return NotFound("Conversation already exists or could not be created.");
                }
                return Ok(conversation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Lấy danh sách tin nhắn theo conversationId
        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetMessagesByConversation(int conversationId)
        {
            var messages = await _messageService.GetMessagesByConversationAsync(conversationId);
            return Ok(messages);
        }

        //Hiện danh sách conversation của user
        [HttpGet("List-Conversation")]
        public async Task<IActionResult> GetUserConversations()
        {
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Đăng nhập trước khi xem tin nhắn");

            var result = await _conversationService.GetAllByUserIdAsync(userId);

            return Ok(result);
        }

        //Gửi tin nhắn
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageDTO dto)
        {
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out var senderId))
                return Unauthorized("Invalid token");
            var message = await _messageService.SendMessageAsync(dto, senderId);

            return Ok(message);
        }
    }
}
