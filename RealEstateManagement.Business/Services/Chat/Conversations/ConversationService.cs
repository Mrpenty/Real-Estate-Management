using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Business.Repositories.Chat.Conversations;
using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Chat.Conversations
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _repository;


        public ConversationService(IConversationRepository repository)
        {
            _repository = repository;

        }
        public async Task<Conversation> CreateConversationAsync(CreateConversationDTO dto)
        {
            var existing = await _repository.GetByUsersAsync(dto.RenterId, dto.LandlordId, dto.PropertyId);
            if (existing != null)
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
        //public async Task<Conversation> GetDetailsAsync(int id)
        //{
        //    return await _repository.GetByIdWithDetailsAsync(id);
        //}
        public async Task<IEnumerable<ConversationDTO>> GetAllByUserIdAsync(int userId)
        {
            var conversations =  await _repository.GetAllByUserIdAsync(userId);
            return conversations.Select(c => new ConversationDTO
            {
                Id = c.Id,
                RenterId = c.RenterId,
                LandlordId = c.LandlordId,
                LastMessage = c.LastMessage,
                LastSentAt = c.LastSentAt,
                RenterName = userId == c.LandlordId ? c.Renter.Name ?? c.Renter.Email : null,
                LandlordName = userId == c.RenterId ? c.Landlord.Name ?? c.Landlord.Email : null
            });
        }

    }
}
