using Microsoft.AspNetCore.SignalR;
using RealEstateManagement.Business.DTO.Chat;

using RealEstateManagement.Business.Repositories.Chat;
using RealEstateManagement.Data.Entity.Messages;

namespace RealEstateManagement.Business.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _repository;
        

        public ChatService(IChatRepository repository)
        {
            _repository = repository;
            
        }
        public async Task<Conversation> CreateConversationAsync(ConversationDTO dto)
        {
            var existing = await _repository.GetByUsersAsync(dto.RenterId, dto.LandlordId, dto.PropertyId);
            if(existing != null)
            {
                   return existing;
            }
            var conversation = new Conversation
            {
                RenterId = dto.RenterId,
                LandlordId = dto.LandlordId,
                PropertyId = dto.PropertyId,
                CreatedAt = DateTime.UtcNow,
            };

            return await _repository.CreateAsync(conversation);
        }
        public async Task<Conversation> GetDetailsAsync(int id)
        {
            return await _repository.GetByIdWithDetailsAsync(id);
        }
    }


}