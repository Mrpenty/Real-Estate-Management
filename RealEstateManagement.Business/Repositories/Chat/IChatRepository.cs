using RealEstateManagement.Data.Entity.Messages;

namespace RealEstateManagement.Business.Repositories.Chat
{
    public interface IChatRepository
    {
        Task<Message> SaveMessageAsync(Message message);
        Task<List<Message>> GetMessagesByConversationIdAsync(int conversationId);
    }
}