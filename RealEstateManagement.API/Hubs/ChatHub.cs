using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Nest;
using RealEstateManagement.Data.Entity.Messages;
using System;

namespace RealEstateManagement.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly RentalDbContext _context;

        public ChatHub(RentalDbContext context)
        {
            _context = context;
        }
        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
            int userId = int.Parse(Context.UserIdentifier); 
            ChatConnectionManager.AddUser(conversationId, userId);
            await MarkAsRead(conversationId, userId.ToString());
        }
        public async Task LeaveConversation(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
            int userId = int.Parse(Context.UserIdentifier);
            ChatConnectionManager.RemoveUser(conversationId, userId);
        }
        //Đã đọc tin nhắn
        public async Task MarkAsRead(string conversationId, string userId)
        {
            int convId = int.Parse(conversationId);
            int uid = int.Parse(userId);

            var messages = await _context.Message
                .Where(m => m.ConversationId == convId && m.SenderId != uid && !m.IsRead)
                .OrderBy(m => m.SentAt)
                .ToListAsync();


            if (messages.Any())
            {
                foreach (var msg in messages)
                {
                    msg.IsRead = true;
                }

                await _context.SaveChangesAsync();

                // 🔥 Chỉ gửi ID của tin nhắn cuối cùng đã đọc
                var lastMessage = messages.Last();
                await Clients.User(lastMessage.SenderId.ToString()).SendAsync(
                    "MessageRead",
                    convId,
                    uid,
                    lastMessage.Id
                );
            }
        }

        public async Task SendMessage(string conversationId, string senderId, string content)
        {
            int convId = int.Parse(conversationId);
            int userId = int.Parse(senderId);

            //Tạo message mới
            var message = new Message
            {
                ConversationId = convId,
                SenderId = userId,
                Content = content,
                SentAt = DateTime.UtcNow,
                IsRead = false,
                NotificationSent = false
            };
            _context.Message.Add(message);
            //Cập nhật conversation
            var conversation = await _context.Conversation.FirstOrDefaultAsync(c => c.Id == convId);
            if (conversation != null)
            {
                conversation.LastSentAt = message.SentAt; 
                conversation.LastMessage = content;
            }
            await _context.SaveChangesAsync(); 
            //Gửi tin nhắn vào gr của server 
            await Clients.Group(conversationId).SendAsync("ReceiveMessage", new
            {
                Id = message.Id,
                SenderId = message.SenderId,
                Content = message.Content,
                SentAt = message.SentAt,
                ConversationId = message.ConversationId,
                IsRead = false
            });
            //Gửi cập nhật conversation list
            await Clients.Group(conversationId).SendAsync("ConversationUpdated", new
            {
                ConversationId = conversationId,
                LastMessage = content,
                SentAt = message.SentAt,
                SenderId = senderId
            });
            //// ✅ Nếu người nhận đang mở conversation thì xem như đã đọc
            //int receiverId = (conversation.RenterId == userId) ? conversation.LandlordId : conversation.RenterId;

            //var connections = ChatConnectionManager.GetConnections(conversationId);
            //if (connections.Contains(receiverId))
            //{
            //    message.IsRead = true;
            //    await _context.SaveChangesAsync();

            //    await Clients.Group(conversationId).SendAsync("MessageRead", conversationId, receiverId, message.Id);
            //}

            // ✅ Nếu người nhận đang mở cuộc trò chuyện, thì gọi lại hàm MarkAsRead để trigger event "MessageRead"
            int receiverId = (conversation.RenterId == userId) ? conversation.LandlordId : conversation.RenterId;
            var connections = ChatConnectionManager.GetConnections(conversationId);
            if (connections.Contains(receiverId))
            {
                // ✅ gọi MarkAsRead để cập nhật toàn bộ logic
                await MarkAsRead(conversationId, receiverId.ToString());
            }

        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                // Duyệt toàn bộ conversation, remove userId
                ChatConnectionManager.RemoveUserFromAllConversations(int.Parse(userId));
            }

            await base.OnDisconnectedAsync(exception);
        }

    }


}
