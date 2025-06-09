using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Properties
{
    public interface IPropertyRepository
    {
        Task<IEnumerable<Property>> GetAllAsync();
        Task<Property> GetPropertyByIdAsync(int id);
        Task<IEnumerable<Property>> FilterByPriceAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<Property>> FilterByAreaAsync(decimal minArea, decimal maxArea);
    }
}
