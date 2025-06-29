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
        public async Task<IEnumerable<Message>> GetByConversationIdAsync(int conversationId)
        {
            return await _context.Message
                .Include(m => m.Sender)
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }
    }
}
