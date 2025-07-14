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
    }
}
