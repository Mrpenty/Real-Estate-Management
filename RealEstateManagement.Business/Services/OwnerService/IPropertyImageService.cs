using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.OwnerService
{
    public interface IPropertyImageService
    {
        Task<PropertyImage> AddImageAsync(int propertyId, PropertyImageCreateDto dto);
    }

}
