using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.OwnerService
{
    public interface IPropertyPostService
    {
        Task<int> CreatePropertyPostAsync(PropertyCreateRequestDto dto, int landlordId);
    }

}
