using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Chat.Conversations
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly RentalDbContext _context;

        public ConversationRepository(RentalDbContext context)
        {
            _context = context;
        }

        public async Task<Conversation> CreateAsync(Conversation conversation)
        {
            _context.Conversation.Add(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }

        public async Task<Conversation> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Conversation
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Conversation?> GetByUsersAsync(int renterId, int landlordId, int? propertyId = null)
        {
            return await _context.Conversation
                .FirstOrDefaultAsync(c =>
                    c.RenterId == renterId &&
                    c.LandlordId == landlordId &&
                    (!propertyId.HasValue || c.PropertyId == propertyId));
        }
        public async Task<IEnumerable<Conversation>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Conversation
                .Where(c => c.RenterId == userId || c.LandlordId == userId)
                .OrderByDescending(c => c.LastSentAt ?? c.CreatedAt)
                .Include(c => c.Renter)
                .Include(c => c.Landlord)
                .Include(c => c.Property)
                .ToListAsync();
        }   
    }
}
