using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Business.Repositories.Chat.Messages;
using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Chat.Messages
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _repo;

        public MessageService(IMessageRepository repo)
        {
            _repo = repo;
        }

        public async Task<Message> SendMessageAsync(MessageDTO dto, int senderId)
        {
            var message = new Message
            {
                ConversationId = dto.ConversationId,
                SenderId = senderId,
                Content = dto.Content,
                SentAt = DateTime.UtcNow,
                IsRead = false,
                NotificationSent = false
            };

            return await _repo.CreateAsync(message);
        }
        public async Task<IEnumerable<MessageViewDTO>> GetMessagesByConversationAsync(int conversationId)
        {
            var messages = await _repo.GetByConversationIdAsync(conversationId);
            if (messages == null || !messages.Any())
            {
                return new List<MessageViewDTO>();
            }
            return messages.Select(m => new MessageViewDTO
            {
                Id = m.Id,
                ConversationId = m.ConversationId,
                SenderId = m.SenderId,
                SenderName = m.Sender.UserName,     // giả sử ApplicationUser có FullName
                SenderAvatar = m.Sender.ProfilePictureUrl,  // giả sử có AvatarUrl
                Content = m.Content,
                IsRead = m.IsRead,
                SentAt = m.SentAt
            }).ToList();
        }
    }
}
