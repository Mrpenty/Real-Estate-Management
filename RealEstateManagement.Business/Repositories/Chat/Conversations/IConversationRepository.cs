using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Chat.Conversations
{
    public interface IConversationRepository
    {
        Task<Conversation> CreateAsync(Conversation conversation);
        Task<Conversation> GetByUsersAsync(int renterId, int landlordId, int? propertyId);
        //Task<Conversation> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Conversation>> GetAllByUserIdAsync(int userId);
        Task<IEnumerable<Conversation>> FilterConversationAsync(int userId, string searchTerm, int skip = 0, int take = 5);

        Task<Conversation?> GetConvesationAsync(int renterId, int landlordId);
    }
}
