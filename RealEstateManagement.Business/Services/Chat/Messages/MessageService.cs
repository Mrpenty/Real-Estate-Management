using Microsoft.AspNetCore.SignalR;
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
        public async Task<IEnumerable<MessageViewDTO>> GetMessagesByConversationAsync(int conversationId, int skip = 0, int take = 20)
        {
            var messages = await _repo.GetByConversationIdAsync(conversationId, skip, take);
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
        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id) ?? throw new KeyNotFoundException("Message not found.");
        }
        public async Task DeleteMessageAsync(int id)
        {
            var message = await _repo.GetByIdAsync(id);
            if (message == null)
            {
                throw new KeyNotFoundException("Message not found.");
            }
            await _repo.DeleteAsync(message);
        }
        public async Task UpdateMessageAsync(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Message cannot be null.");
            }
            await _repo.UpdateAsync(message);
            }
    }
}
