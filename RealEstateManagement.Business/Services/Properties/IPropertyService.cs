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
        Task<PropertyDetailDTO> GetPropertyByIdAsync(int id);
        Task<IEnumerable<HomePropertyDTO>> FilterByPriceAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<HomePropertyDTO>> FilterByAreaAsync(decimal minArea, decimal maxArea);

    }
}
