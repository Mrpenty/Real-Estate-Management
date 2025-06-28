using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Messages;

namespace RealEstateManagement.Business.Repositories.Chat
{
    public class ChatRepository : IChatRepository
    {
        private readonly RentalDbContext _context;

        public ChatRepository(RentalDbContext context)
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
                .Include(c => c.Renter)
                .Include(c => c.Landlord)
                .Include(c => c.Property)
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
    }
}