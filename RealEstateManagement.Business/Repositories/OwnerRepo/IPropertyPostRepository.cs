using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.OwnerRepo
{
    public interface IPropertyPostRepository
    {
        Task<int> CreatePropertyPostAsync(Property property, PropertyPost post, List<int> amenityIds);

        Task<PropertyPost> GetByPropertyIdAsync(int propertyId);
        Task UpdateAsync(PropertyPost post);
        Task<PropertyPost> GetByIdAsync(int postId);
        IQueryable <PropertyPost> GetAll();



        Task<PropertyPost> GetPropertyPostByIdAsync(int id, int landlordId);

        Task UpdatePropertyAmenities(int propertyId, List<int> amenityIds);
    }

}
