using RealEstateManagement.Data.Entity;
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
        Task<bool> PropertyExistsAsync(int propertyId);
    }

}
