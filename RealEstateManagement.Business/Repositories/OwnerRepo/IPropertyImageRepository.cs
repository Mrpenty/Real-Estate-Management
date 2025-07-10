using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.OwnerRepo
{
    public interface IPropertyImageRepository
    {
        Task<PropertyImage> AddImageAsync(PropertyImage image);
        Task<bool> HasAnyImage(int id);
        Task<bool> PropertyExistsAsync(int propertyId);
        Task<PropertyImage> UpdateImageAsync(PropertyImage updatedImage);

    }

}
