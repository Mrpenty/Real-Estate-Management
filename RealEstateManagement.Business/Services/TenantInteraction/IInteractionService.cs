using RealEstateManagement.Business.DTO.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.TenantInteraction
{
    public interface IInteractionService
    {
        Task<ProfileLandlordDTO> GetProfileLandlordByIdAsync(int landlordId);
    }
}
