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
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            // Không cho phép tự chat với chính mình
            if (dto.RenterId == dto.LandlordId)
                throw new ArgumentException("Renter and Landlord cannot be the same user.", nameof(dto));
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
            var convs = await _repository.GetAllByUserIdAsync(userId)
            ?? Enumerable.Empty<Conversation>(); // <- chống null
            //var conversations =  await _repository.GetAllByUserIdAsync(userId);
            return convs.Select(c => new ConversationDTO
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
        public async Task<IEnumerable<ConversationDTO?>> FilterConversationAsync(int userId, string searchTerm, int skip = 0, int take = 5)
        {
            var conversations = await _repository.FilterConversationAsync(userId, searchTerm, skip, take);

            return conversations.Select(c => new ConversationDTO
            {
                Id = c.Id,
                PropertyId = c.PropertyId,
                RenterId = c.RenterId,
                LandlordId = c.LandlordId,
                RenterName = c.Renter?.Name,
                LandlordName = c.Landlord?.Name,

            });
        }

    }
}
