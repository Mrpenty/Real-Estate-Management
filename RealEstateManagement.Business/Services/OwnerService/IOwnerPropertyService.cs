using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.OwnerService
{
    public interface IOwnerPropertyService
    {
        IQueryable<OwnerPropertyDto> GetPropertiesByLandlordQueryable(int landlordId);
        Task<OwnerPropertyDto> GetPropertyByIdAsync(int id, int landlordId);
        Task UpdatePropertyAsync(OwnerUpdatePropertyDto dto, int landlordId);
        Task DeletePropertyAsync(int id, int landlordId);
    }
}
