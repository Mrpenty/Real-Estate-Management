using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Data.Entity.Messages;

namespace RealEstateManagement.Business.Services.Chat
{
    public interface IChatService
    {
        Task<Conversation> CreateConversationAsync(ConversationDTO dto);
        Task<Conversation> GetDetailsAsync(int id);
    }
}