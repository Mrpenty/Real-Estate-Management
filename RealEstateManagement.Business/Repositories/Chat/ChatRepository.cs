using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Chat
{
    public class ChatRepository :IChatRepository
    {
        private readonly RentalDbContext _context;

        public ChatRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<Message> SaveMessageAsync(Message message)
        {
            _context.Message.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<Message>> GetMessagesByConversationIdAsync(int conversationId)
        {
            return await _context.Message
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt)
                .Include(m => m.Sender)
                .ToListAsync();
        }
    }
}
