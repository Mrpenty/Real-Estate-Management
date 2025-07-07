using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity.PropertyEntity;
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
        Task<PropertyPost> GetPostByIdAsync(int postId);
        Task<object> GetPostDetailForAdminAsync(int postId);
        Task<object> GetPostsByStatusAsync(string status, int page, int pageSize);
        Task<bool> UpdatePostStatusAsync(int id, string status);
    }

}
