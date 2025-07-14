using RealEstateManagement.Business.DTO.Interaction;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Repositories.TenantInteraction;
using RealEstateManagement.Data.Entity.AddressEnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.TenantInteraction
{

    public class InteractionService : IInteractionService
    {
        private readonly IInteractionRepository _interactionRepository;
        public InteractionService(IInteractionRepository interactionRepository)
        {
            _interactionRepository = interactionRepository;
        }
        public async Task<ProfileLandlordDTO> GetProfileLandlordByIdAsync(int landlordId)
        {
            var landlord = await _interactionRepository.GetProfileLandlordByIdAsync(landlordId);
            return new ProfileLandlordDTO
            {
                id = landlord.Id,
                Name = landlord.Name,
                Email = landlord.Email,
                PhoneNumber = landlord.PhoneNumber,
                Avatar = landlord.ProfilePictureUrl,
                Properties = landlord.Properties?.Select(p => new HomePropertyDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Area = p.Area,
                    Price = p.Price,
                    Street = p.Address.Street.Name,
                    Province = p.Address.Province.Name,
                    Ward = p.Address.Ward.Name,
                    DetailedAddress = p.Address.DetailedAddress,
                    StreetId = p.Address.StreetId,
                    ProvinceId = p.Address.ProvinceId,
                    WardId = p.Address.WardId
                }).ToList()
            };
        }
    }
}
