using RealEstateManagement.Data.Entity.Messages;

namespace RealEstateManagement.Business.Repositories.Chat
{
    public interface IChatRepository
    {
        Task<Conversation> CreateAsync(Conversation conversation);
        Task<Conversation> GetByUsersAsync(int renterId, int landlordId, int? propertyId);
        Task<Conversation> GetByIdWithDetailsAsync(int id);
    }

}