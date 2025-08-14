using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity.PropertyEntity;
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

        Task<PropertyImage> UpdateImageAsync(PropertyImage updatedImage);

        Task<bool> ClearAllImagesAsync(int propertyId);

        Task<bool> DeleteImageAsync(int propertyId, int imageId);
    }

}
