using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.API.Hubs;
using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Data.Entity.Messages;
using RealEstateManagement.Data.Entity.User;
using System.IdentityModel.Tokens.Jwt;

namespace RealEstateManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupportController : ControllerBase
    {
        private readonly RentalDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public SupportController(RentalDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // User tạo cuộc trò chuyện hỗ trợ mới
        [HttpPost("create-conversation")]
        public async Task<IActionResult> CreateSupportConversation([FromBody] CreateSupportConversationDTO dto)
        {
            try
            {
                var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(accessToken);
                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                
                if (!int.TryParse(userIdClaim, out int userId))
                    return Unauthorized("Đăng nhập trước khi tạo yêu cầu hỗ trợ");

                var supportConversation = new Conversation
                {
                    RenterId = userId,
                    LandlordId = 0, // 0 means it's a support conversation
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
                    message = "Yêu cầu hỗ trợ đã được tạo thành công" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        // Lấy danh sách cuộc trò chuyện hỗ trợ của user
        [HttpGet("user-conversations")]
        public async Task<IActionResult> GetUserSupportConversations()
        {
            try
            {
                var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(accessToken);
                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                
                if (!int.TryParse(userIdClaim, out int userId))
                    return Unauthorized("Đăng nhập trước khi xem yêu cầu hỗ trợ");

                var conversations = await _context.Conversation
                    .Where(c => c.LandlordId == 0 && c.RenterId == userId) // LandlordId = 0 means support conversation
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

        // Lấy tin nhắn của cuộc trò chuyện hỗ trợ
        [HttpGet("conversation/{conversationId}/messages")]
        public async Task<IActionResult> GetSupportMessages(int conversationId, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            try
            {
                var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(accessToken);
                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                
                if (!int.TryParse(userIdClaim, out int userId))
                    return Unauthorized("Đăng nhập trước khi xem tin nhắn");

                // Kiểm tra quyền truy cập
                var conversation = await _context.Conversation
                    .FirstOrDefaultAsync(c => c.Id == conversationId && c.LandlordId == 0);
                
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
                        MessageType = "Text", // Default type
                        AttachmentUrl = null,
                        IsFromAdmin = m.SenderId != conversation.RenterId, // Admin if not the user
                        SenderName = m.Sender.Name,
                        SenderRole = m.SenderId != conversation.RenterId ? "Admin" : "User"
                    })
                    .OrderBy(m => m.SentAt)
                    .ToListAsync();

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

        // User gửi tin nhắn hỗ trợ
        [HttpPost("conversation/{conversationId}/send-message")]
        public async Task<IActionResult> SendSupportMessage(int conversationId, [FromBody] string content)
        {
            try
            {
                var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(accessToken);
                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                
                if (!int.TryParse(userIdClaim, out int userId))
                    return Unauthorized("Đăng nhập trước khi gửi tin nhắn");

                var conversation = await _context.Conversation
                    .FirstOrDefaultAsync(c => c.Id == conversationId && c.LandlordId == 0);
                
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
                    message = "Tin nhắn đã được gửi thành công" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        // Admin lấy danh sách tất cả cuộc trò chuyện hỗ trợ
        [HttpGet("admin/all-conversations")]
        public async Task<IActionResult> GetAllSupportConversations([FromQuery] string? status = null, [FromQuery] string? priority = null)
        {
            try
            {
                var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(accessToken);
                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                
                if (!int.TryParse(userIdClaim, out int userId))
                    return Unauthorized("Đăng nhập trước khi xem yêu cầu hỗ trợ");

                // Kiểm tra quyền admin (có thể thêm logic kiểm tra role)
                var isAdmin = await _context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == 1);
                if (!isAdmin)
                    return Forbid("Bạn không có quyền admin");

                var query = _context.Conversation.Where(c => c.LandlordId == 0); // Only support conversations

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

        // Admin trả lời tin nhắn hỗ trợ
        [HttpPost("admin/conversation/{conversationId}/reply")]
        public async Task<IActionResult> AdminReply(int conversationId, [FromBody] string content)
        {
            try
            {
                var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(accessToken);
                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                
                if (!int.TryParse(userIdClaim, out int adminId))
                    return Unauthorized("Đăng nhập trước khi trả lời");

                var isAdmin = await _context.UserRoles.AnyAsync(ur => ur.UserId == adminId && ur.RoleId == 1);
                if (!isAdmin)
                    return Forbid("Bạn không có quyền admin");

                var conversation = await _context.Conversation
                    .FirstOrDefaultAsync(c => c.Id == conversationId && c.LandlordId == 0);
                
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
                    message = "Phản hồi đã được gửi thành công" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }

        // Admin cập nhật trạng thái cuộc trò chuyện hỗ trợ
        [HttpPut("admin/conversation/{conversationId}/status")]
        public async Task<IActionResult> UpdateSupportStatus(int conversationId, [FromBody] UpdateSupportStatusDTO dto)
        {
            try
            {
                var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(accessToken);
                var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                
                if (!int.TryParse(userIdClaim, out int adminId))
                    return Unauthorized("Đăng nhập trước khi cập nhật trạng thái");

                // Kiểm tra quyền admin
                var isAdmin = await _context.UserRoles.AnyAsync(ur => ur.UserId == adminId && ur.RoleId == 1);
                if (!isAdmin)
                    return Forbid("Bạn không có quyền admin");

                var conversation = await _context.Conversation
                    .FirstOrDefaultAsync(c => c.Id == conversationId && c.LandlordId == 0);
                
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
                    message = "Trạng thái đã được cập nhật thành công" 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi nội bộ: {ex.Message}");
            }
        }
    }
} 