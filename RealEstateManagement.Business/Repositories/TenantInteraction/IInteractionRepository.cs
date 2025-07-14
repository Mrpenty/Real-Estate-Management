using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.TenantInteraction
{
    public interface IInteractionRepository
    {
        Task<ApplicationUser> GetProfileLandlordByIdAsync(int landlordId);
    }
}
