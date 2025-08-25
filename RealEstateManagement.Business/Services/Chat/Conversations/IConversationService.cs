using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Chat.Conversations
{
    public interface IConversationService
    {
        Task<Conversation> CreateConversationAsync(CreateConversationDTO dto);
        //Task<Conversation> GetDetailsAsync(int id);
        Task<IEnumerable<ConversationDTO>> GetAllByUserIdAsync(int userId);
        Task<IEnumerable<ConversationDTO?>> FilterConversationAsync(int userId, string searchTerm, int skip = 0, int take = 5);

        Task HandleInterestAsync(int renterId, int postId);
    }
}
