using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Chat.Messages
{
    public class MessageRepository : IMessageRepository
    {
        private readonly RentalDbContext _context;

        public MessageRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<Message> CreateAsync(Message message)
        {
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }
        public async Task<IEnumerable<Message>> GetByConversationIdAsync(int conversationId, int skip = 0, int take = 20)
        {
            var messages = await _context.Message
                .Include(m => m.Sender)
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(m => m.SentAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            messages.Reverse();
            return messages;
        }
        public async Task<Message?> GetByIdAsync(int id)
        {
            return await _context.Message
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
        public async Task DeleteAsync(Message message)
        {
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Message message)
        {
            _context.Message.Update(message);
            await _context.SaveChangesAsync();
        }
        public async Task<(DateTime? lastMessageAt, int? conversationId)> GetLastConversationActivityAsync(int renterId, int landlordId, int propertyId)

        {
            // Tìm đúng cuộc trò chuyện giữa renter-landlord cho property này
            var conv = await _context.Conversation
                .Where(c => c.RenterId == renterId
                         && c.LandlordId == landlordId
                         && c.PropertyId == propertyId)
                .Select(c => new
                {
                    c.Id,
                    c.LastSentAt,     // nếu bạn có cột này thì dùng nó
                    HasLast = c.LastSentAt != null
                })
                .FirstOrDefaultAsync();

            if (conv == null)
                return (null, null);

            // Nếu đã có LastSentAt trên Conversation thì dùng luôn (nhanh nhất)
            if (conv.HasLast)
                return (conv.LastSentAt, conv.Id);

            // Nếu không có LastSentAt, fallback tính từ Messages (giữ entity y nguyên)
            var lastMsgTime = await _context.Message
                .Where(m => m.ConversationId == conv.Id)
                .OrderByDescending(m => m.SentAt)
                .Select(m => (DateTime?)m.SentAt)
                .FirstOrDefaultAsync();

            return (lastMsgTime, conv.Id);
        }
    }
}
