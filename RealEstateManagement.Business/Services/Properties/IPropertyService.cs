using RealEstateManagement.Business.DTO.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Properties
{
    public interface IPropertyService
    {
        Task<IEnumerable<HomePropertyDTO>> GetAllPropertiesAsync();
        //Lấy 1 id
        Task<PropertyDetailDTO> GetPropertyByIdAsync(int id);
        Task<IEnumerable<HomePropertyDTO>> FilterByPriceAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<HomePropertyDTO>> FilterByAreaAsync(decimal minArea, decimal maxArea);
        //So sánh property (tối đa 3)
        Task<IEnumerable<ComparePropertyDTO>> ComparePropertiesAsync(List<int> ids);
        Task<List<PropertyDetailDTO>> GetPropertiesByIdsAsync(List<int> ids);

    }
}
