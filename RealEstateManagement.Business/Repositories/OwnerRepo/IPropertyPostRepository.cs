using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.ReportEntity;
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


        Task<PropertyPost> GetByPropertyIdAndOwnerIdAsync(int propertyId, int ownerId);
        Task<PropertyPost> GetPropertyPostByIdAsync(int id, int landlordId);

        Task UpdatePropertyAmenities(int propertyId, List<int> amenityIds);
        Task<PropertyPost?> GetPostWithPropertyAsync(int propertyPostId);
        Task<List<Report>> GetReportsForPostAsync(int propertyPostId);
        Task<int> SaveChangesAsync();
        Task<PropertyPost?> GetPostDetailForAdminAsync(int postId);
        Task<int> CountByStatusAsync(PropertyPost.PropertyPostStatus? status);
        Task<List<PropertyPost>> GetPostsByStatusAsync(PropertyPost.PropertyPostStatus? status, int page, int pageSize);
    }

}
