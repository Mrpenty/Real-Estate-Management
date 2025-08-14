using Humanizer;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Business.Services.Chat.Conversations;
using RealEstateManagement.Business.Services.Chat.Messages;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using RealEstateManagement.API.Hubs;
using RealEstateManagement.Data.Entity.Messages;
using RealEstateManagement.Data.Entity.User;

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

        // Helper method để xử lý JWT token
        private (bool isValid, int userId, string errorMessage) ValidateJwtToken()
        {
            try
            {
                // Kiểm tra Authorization header
                if (!Request.Headers.ContainsKey("Authorization"))
                    return (false, 0, "Thiếu token xác thực");

                var authHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                    return (false, 0, "Token không hợp lệ");

                var accessToken = authHeader.Replace("Bearer ", "");
                if (string.IsNullOrEmpty(accessToken))
                    return (false, 0, "Token không được để trống");

                // Xử lý JWT token
                JwtSecurityToken token;
                try
                {
                    var handler = new JwtSecurityTokenHandler();
                    token = handler.ReadJwtToken(accessToken);
                }
                catch (Exception)
                {
                    return (false, 0, "Token không hợp lệ");
                }

                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                    return (false, 0, "Không thể xác định người dùng từ token");

                return (true, userId, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, 0, $"Lỗi xử lý token: {ex.Message}");
            }
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

        // Tạo cuộc trò chuyện hỗ trợ mới
        [HttpPost("create-support-conversation")]
        public async Task<IActionResult> CreateSupportConversation([FromBody] CreateSupportConversationDTO dto)
        {
            try
            {
                // Validate JWT token
                var (isValid, userId, errorMessage) = ValidateJwtToken();
                if (!isValid)
                    return Unauthorized(errorMessage);

                var supportConversation = new Conversation
                {
                    RenterId = userId,
                    LandlordId = 1, // 1 means admin user (support conversation)
                    PropertyId = null,
                    CreatedAt = DateTime.UtcNow,
                    LastMessage = dto.InitialMessage,
                    LastSentAt = DateTime.UtcNow
                };

                _context.Conversation.Add(supportConversation);
                await _context.SaveChangesAsync();

                // Tạo tin nhắn đầu tiên
                var initialMessage = new Message
                {
                    ConversationId = supportConversation.Id,
                    SenderId = userId,
                    Content = dto.InitialMessage,
                    SentAt = DateTime.UtcNow,
                    IsRead = false
                };

                _context.Message.Add(initialMessage);
                await _context.SaveChangesAsync();

                // Gửi thông báo đến admin qua SignalR
                await _hubContext.Clients.All.SendAsync("NewSupportRequest", new
                {
                    ConversationId = supportConversation.Id,
                    Subject = "Yêu cầu hỗ trợ",
                    UserName = _context.Users.Where(u => u.Id == userId).Select(u => u.Name).FirstOrDefault()
                });

                return Ok(new { 
                    success = true, 
                    conversationId = supportConversation.Id,
                    message = "" // Bỏ thông báo thành công
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        //Lấy danh sách tin nhắn theo conversationId
        [HttpGet("conversation/{conversationId}")]
        public async Task<IActionResult> GetMessagesByConversation(int conversationId, [FromQuery] int skip = 0, [FromQuery] int take = 20)
        {
            var messages = await _messageService.GetMessagesByConversationAsync(conversationId, skip, take);
            return Ok(messages);
        }

        // Lấy tin nhắn của cuộc trò chuyện hỗ trợ
        [HttpGet("support-conversation/{conversationId}/messages")]
        public async Task<IActionResult> GetSupportMessages(int conversationId, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            try
            {
                // Validate JWT token
                var (isValid, userId, errorMessage) = ValidateJwtToken();
                if (!isValid)
                    return Unauthorized(errorMessage);

                // Kiểm tra quyền truy cập
                var conversation = await _context.Conversation
                    .FirstOrDefaultAsync(c => c.Id == conversationId && c.LandlordId == 1);
                
                if (conversation == null)
                    return NotFound("Không tìm thấy cuộc trò chuyện hỗ trợ");

                // Kiểm tra quyền truy cập: user hoặc admin
                var isAdmin = await _context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == 1);
                if (conversation.RenterId != userId && !isAdmin)
                    return Forbid("Bạn không có quyền truy cập cuộc trò chuyện này");

                var messages = await _context.Message
                    .Where(m => m.ConversationId == conversationId)
                    .OrderByDescending(m => m.SentAt)
                    .Skip(skip)
                    .Take(take)
                    .Select(m => new SupportMessageDTO
                    {
                        Id = m.Id,
                        SupportConversationId = m.ConversationId,
                        SenderId = m.SenderId,
                        Content = m.Content,
                        SentAt = m.SentAt,
                        IsRead = m.IsRead,
                        IsFromAdmin = m.SenderId != conversation.RenterId, // Admin if not the user
                        SenderName = m.Sender.Name,
                        SenderRole = m.SenderId != conversation.RenterId ? "Admin" : "User"
                    })
                    .ToListAsync();

                // Sắp xếp lại theo thứ tự thời gian
                messages = messages.OrderBy(m => m.SentAt).ToList();

                // Đánh dấu tin nhắn đã đọc
                var unreadMessages = await _context.Message
                    .Where(m => m.ConversationId == conversationId && 
                                !m.IsRead && 
                                m.SenderId != userId)
                    .ToListAsync();

                foreach (var message in unreadMessages)
                {
                    message.IsRead = true;
                }
                await _context.SaveChangesAsync();

                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        //Hiện danh sách conversation của user
        [HttpGet("List-Conversation")]
        public async Task<IActionResult> GetUserConversations()
        {
            // Validate JWT token
            var (isValid, userId, errorMessage) = ValidateJwtToken();
            if (!isValid)
                return Unauthorized(errorMessage);

            var result = await _conversationService.GetAllByUserIdAsync(userId);

            return Ok(result);
        }

        // Lấy danh sách cuộc trò chuyện hỗ trợ của user
        [HttpGet("user-support-conversations")]
        public async Task<IActionResult> GetUserSupportConversations()
        {
            try
            {
                // Validate JWT token
                var (isValid, userId, errorMessage) = ValidateJwtToken();
                if (!isValid)
                    return Unauthorized(errorMessage);

                var conversations = await _context.Conversation
                    .Where(c => c.LandlordId == 1 && c.RenterId == userId) // LandlordId = 1 means admin user (support conversation)
                    .OrderByDescending(c => c.LastSentAt ?? c.CreatedAt)
                    .Select(c => new SupportConversationDTO
                    {
                        Id = c.Id,
                        UserId = c.RenterId,
                        AdminId = null,
                        Subject = "Yêu cầu hỗ trợ",
                        Status = "Open",
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.LastSentAt,
                        ResolvedAt = null,
                        UserName = c.Renter.Name,
                        AdminName = null,
                        LastMessage = c.LastMessage ?? "",
                        LastMessageAt = c.LastSentAt,
                        UnreadCount = c.Messages.Count(m => !m.IsRead && m.SenderId != userId)
                    })
                    .ToListAsync();

                return Ok(conversations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        // Admin lấy danh sách tất cả cuộc trò chuyện hỗ trợ
        [HttpGet("admin/all-support-conversations")]
        public async Task<IActionResult> GetAllSupportConversations([FromQuery] string? status = null, [FromQuery] string? priority = null)
        {
            try
            {
                // Validate JWT token
                var (isValid, userId, errorMessage) = ValidateJwtToken();
                if (!isValid)
                    return Unauthorized(errorMessage);

                // Kiểm tra quyền admin (có thể thêm logic kiểm tra role)
                var isAdmin = await _context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == 1);
                if (!isAdmin)
                    return Forbid("Bạn không có quyền admin");

                var query = _context.Conversation.Where(c => c.LandlordId == 1); // Only support conversations (admin)

                var conversations = await query
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(c => new SupportConversationDTO
                    {
                        Id = c.Id,
                        UserId = c.RenterId,
                        AdminId = null,
                        Subject = "Yêu cầu hỗ trợ",
                        Status = "Open",
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.LastSentAt,
                        ResolvedAt = null,
                        UserName = c.Renter.Name,
                        AdminName = null,
                        LastMessage = c.LastMessage ?? "",
                        LastMessageAt = c.LastSentAt,
                        UnreadCount = c.Messages.Count(m => !m.IsRead && m.SenderId != c.RenterId)
                    })
                    .ToListAsync();

                return Ok(conversations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        [HttpGet("conversation/{conversationId}/count")]
        public async Task<IActionResult> GetMessageCount(int conversationId)
        {
            int count = await _context.Message
                .Where(m => m.ConversationId == conversationId)
                .CountAsync();

            return Ok(count);
        }

        // Gửi tin nhắn hỗ trợ
        [HttpPost("support-conversation/{conversationId}/send-message")]
        public async Task<IActionResult> SendSupportMessage(int conversationId, [FromBody] string content)
        {
            try
            {
                // Validate JWT token
                var (isValid, userId, errorMessage) = ValidateJwtToken();
                if (!isValid)
                    return Unauthorized(errorMessage);

                var conversation = await _context.Conversation
                    .FirstOrDefaultAsync(c => c.Id == conversationId && c.LandlordId == 1);
                
                if (conversation == null)
                    return NotFound("Không tìm thấy cuộc trò chuyện hỗ trợ");

                // Kiểm tra quyền gửi tin nhắn: user hoặc admin
                var isAdmin = await _context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == 1);
                if (conversation.RenterId != userId && !isAdmin)
                    return Forbid("Bạn không có quyền gửi tin nhắn trong cuộc trò chuyện này");

                var message = new Message
                {
                    ConversationId = conversationId,
                    SenderId = userId,
                    Content = content,
                    SentAt = DateTime.UtcNow,
                    IsRead = false
                };

                _context.Message.Add(message);
                
                // Cập nhật conversation
                conversation.LastMessage = content;
                conversation.LastSentAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Gửi thông báo qua SignalR
                var isUserAdmin = await _context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == 1);
                await _hubContext.Clients.All.SendAsync("SupportMessageReceived", new
                {
                    ConversationId = conversationId,
                    MessageId = message.Id,
                    Content = content,
                    SenderId = userId,
                    SenderName = _context.Users.Where(u => u.Id == userId).Select(u => u.Name).FirstOrDefault(),
                    SentAt = message.SentAt,
                    IsFromAdmin = isUserAdmin
                });

                return Ok(new { 
                    success = true, 
                    messageId = message.Id,
                    message = "" // Bỏ thông báo thành công
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        // Admin trả lời tin nhắn hỗ trợ
        [HttpPost("admin/support-conversation/{conversationId}/reply")]
        public async Task<IActionResult> AdminReply(int conversationId, [FromBody] string content)
        {
            try
            {
                // Validate JWT token
                var (isValid, adminId, errorMessage) = ValidateJwtToken();
                if (!isValid)
                    return Unauthorized(errorMessage);

                var isAdmin = await _context.UserRoles.AnyAsync(ur => ur.UserId == adminId && ur.RoleId == 1);
                if (!isAdmin)
                    return Forbid("Bạn không có quyền admin");

                var conversation = await _context.Conversation
                    .FirstOrDefaultAsync(c => c.Id == conversationId && c.LandlordId == 1);
                
                if (conversation == null)
                    return NotFound("Không tìm thấy cuộc trò chuyện hỗ trợ");

                var message = new Message
                {
                    ConversationId = conversationId,
                    SenderId = adminId,
                    Content = content,
                    SentAt = DateTime.UtcNow,
                    IsRead = false
                };

                _context.Message.Add(message);
                
                // Cập nhật conversation
                conversation.LastMessage = content;
                conversation.LastSentAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Gửi thông báo qua SignalR
                await _hubContext.Clients.All.SendAsync("SupportMessageReceived", new
                {
                    ConversationId = conversationId,
                    MessageId = message.Id,
                    Content = content,
                    SenderId = adminId,
                    SenderName = _context.Users.Where(u => u.Id == adminId).Select(u => u.Name).FirstOrDefault(),
                    SentAt = message.SentAt,
                    IsFromAdmin = true
                });

                return Ok(new { 
                    success = true, 
                    messageId = message.Id,
                    message = "" // Bỏ thông báo thành công
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        // Admin cập nhật trạng thái cuộc trò chuyện hỗ trợ
        [HttpPut("admin/support-conversation/{conversationId}/status")]
        public async Task<IActionResult> UpdateSupportStatus(int conversationId, [FromBody] UpdateSupportStatusDTO dto)
        {
            try
            {
                // Validate JWT token
                var (isValid, adminId, errorMessage) = ValidateJwtToken();
                if (!isValid)
                    return Unauthorized(errorMessage);

                // Kiểm tra quyền admin
                var isAdmin = await _context.UserRoles.AnyAsync(ur => ur.UserId == adminId && ur.RoleId == 1);
                if (!isAdmin)
                    return Forbid("Bạn không có quyền admin");

                var conversation = await _context.Conversation
                    .FirstOrDefaultAsync(c => c.Id == conversationId && c.LandlordId == 1);
                
                if (conversation == null)
                    return NotFound("Không tìm thấy cuộc trò chuyện hỗ trợ");

                // For now, we'll just update the LastSentAt to indicate status change
                conversation.LastSentAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Gửi thông báo qua SignalR
                await _hubContext.Clients.All.SendAsync("SupportStatusUpdated", new
                {
                    ConversationId = conversationId,
                    Status = "Updated",
                    AdminId = adminId,
                    UpdatedAt = conversation.LastSentAt
                });

                return Ok(new { 
                    success = true, 
                    message = "" // Bỏ thông báo thành công
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

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
            // Validate JWT token
            var (isValid, userId, errorMessage) = ValidateJwtToken();
            if (!isValid)
                return Unauthorized(errorMessage);

            var conversations = await _conversationService.FilterConversationAsync(userId, searchTerm, skip, take);
            return Ok(conversations);
        }
    }
}
