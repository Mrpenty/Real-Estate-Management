using RealEstateManagement.Business.DTO.PropertyTypeDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.PropertyTypeService
{
    public interface IPropertyTypeService
    {
        Task<IEnumerable<PropertyTypeGet>> GetAllAsync();
        Task<PropertyTypeGet?> GetByIdAsync(int id);
        Task<PropertyTypeGet> CreateAsync(PropertyTypeGet dto);
        Task<PropertyTypeGet?> UpdateAsync(int id, PropertyTypeGet dto);
        Task<bool> DeleteAsync(int id);
    }
}
