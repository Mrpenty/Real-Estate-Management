using Humanizer;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Business.Services.Chat.Conversations;
using RealEstateManagement.Business.Services.Chat.Messages;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using RealEstateManagement.API.Hubs;

namespace RealEstateManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        private readonly IMessageService _messageService;
        private readonly RentalDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;
        public ChatController(IConversationService conversationService, IMessageService messageService, RentalDbContext context, IHubContext<ChatHub> hubContext)
        {
            _conversationService = conversationService;
            _messageService = messageService;
            _context = context;
            _hubContext = hubContext;
        }

        //Tạo cuộc trò chuyện mới
        [HttpPost("Create-Conversation")]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationDTO dto)
        {
            if (dto == null || dto.RenterId <= 0 || dto.LandlordId <= 0)
            {
                return BadRequest("Invalid conversation data.");
            }

            try { 
                var conversation = await _conversationService.CreateConversationAsync(dto);
                if (conversation == null)
                {
                    return StatusCode(500, "Could not create conversation.");
                }
                return Ok(new {id = conversation.Id});
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //Lấy danh sách tin nhắn theo conversationId
        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetMessagesByConversation(int conversationId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            var messages = await _messageService.GetMessagesByConversationAsync(conversationId, skip, take);
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
        [HttpGet("conversation/{conversationId}/count")]
        public async Task<IActionResult> GetMessageCount(int conversationId)
        {
            int count = await _context.Message
                .Where(m => m.ConversationId == conversationId)
                .CountAsync();

            return Ok(count);
        }
        ////Gửi tin nhắn
        //[HttpPost("send")]
        //public async Task<IActionResult> SendMessage([FromBody] MessageDTO dto)
        //{
        //    var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        //    var handler = new JwtSecurityTokenHandler();
        //    var token = handler.ReadJwtToken(accessToken);
        //    var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        //    if (!int.TryParse(userIdClaim, out var senderId))
        //        return Unauthorized("Invalid token");
        //    var message = await _messageService.SendMessageAsync(dto, senderId);

        //    return Ok(message);
        //}
        [HttpDelete("delete-message/{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Message.FindAsync(id);
            if (message == null)
                return NotFound();

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            // Gửi thông báo đến tất cả user trong cuộc trò chuyện
            await _hubContext.Clients.Group(message.ConversationId.ToString())
                .SendAsync("MessageDeleted", message.ConversationId, message.Id);

            return NoContent();
        }
        [HttpPut("edit-message/{id}")]
        public async Task<IActionResult> EditMessage(int id, [FromBody] string newContent)
        {
            var message = await _messageService.GetMessageByIdAsync(id);
            if (message == null)
                return NotFound();

            message.Content = newContent;
            await _messageService.UpdateMessageAsync(message);

            // Gửi tới tất cả client trong cuộc trò chuyện
            await _hubContext.Clients
                .Group(message.ConversationId.ToString())
                .SendAsync("MessageEdited", message.Id, newContent);

            return Ok();
        }
        [HttpGet("search-conversation")]
        public async Task<IActionResult> SearchConversations([FromQuery] string searchTerm, [FromQuery] int skip = 0, [FromQuery] int take = 5)
        {
            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);
            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Đăng nhập trước khi tìm kiếm");

            var conversations = await _conversationService.FilterConversationAsync(userId, searchTerm, skip, take);
            return Ok(conversations);
        }
    }
}
