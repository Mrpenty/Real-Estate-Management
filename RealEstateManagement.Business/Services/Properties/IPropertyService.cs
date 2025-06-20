using Microsoft.AspNetCore.Mvc;
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
        Task<IEnumerable<HomePropertyDTO>> FilterByPriceAsync([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice);
        Task<IEnumerable<HomePropertyDTO>> FilterByAreaAsync([FromQuery] decimal? minArea, [FromQuery] decimal? maxArea);

        Task<IEnumerable<HomePropertyDTO>> FilterAdvancedAsync(PropertyFilterDTO filter);
        //So sánh property (tối đa 3)
        Task<IEnumerable<ComparePropertyDTO>> ComparePropertiesAsync(List<int> ids);
        Task<bool> AddToFavoriteAsync(int userId, int propertyId);
        Task<List<PropertyDetailDTO>> GetPropertiesByIdsAsync(List<int> ids);


        //Elasticsearch
        Task<bool> IndexPropertyAsync(PropertySearchDTO dto);
        Task BulkIndexPropertiesAsync();
        Task<IEnumerable<HomePropertyDTO>> SearchAsync(string keyword);

    }
}
