using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Properties
{
    public interface IInterestedPropertyRepository
    {
        //thê bản ghi vào csdl
        Task<InterestedProperty> AddAsync(InterestedProperty entity);
        //Lấy 1 bản ghi
        Task<InterestedProperty> GetByIdAsync(int id);
        //Lấy bản ghi theo renterId và propertyId
        Task<InterestedProperty> GetByRenterAndPropertyAsync(int renterId, int propertyId);
        //Lấy tất cả bản ghi theo renterId
        Task<IEnumerable<InterestedProperty>> GetByRenterAsync(int renterId);
        //Lấy tất cả người đã quan tâm property theo propertyId
        Task<IEnumerable<InterestedProperty>> GetByPropertyAsync(int propertyId);
        //Cập nhật bản ghi
        Task UpdateAsync(InterestedProperty entity);
        Task DeleteAsync(InterestedProperty entity);
        IQueryable<InterestedProperty> Query(); // For OData
    }
}
