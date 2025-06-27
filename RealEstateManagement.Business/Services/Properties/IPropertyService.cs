using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Location;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.AddressEnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Properties
{
    public interface IPropertyService
    {
        Task<IEnumerable<HomePropertyDTO>> GetAllPropertiesAsync(int? userId = 0);
        //Lấy 1 id
        Task<PropertyDetailDTO> GetPropertyByIdAsync(int id);
        Task<IEnumerable<HomePropertyDTO>> FilterByPriceAsync([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice);
        Task<IEnumerable<HomePropertyDTO>> FilterByAreaAsync([FromQuery] decimal? minArea, [FromQuery] decimal? maxArea);

        Task<IEnumerable<HomePropertyDTO>> FilterAdvancedAsync(PropertyFilterDTO filter);
        //So sánh property (tối đa 3)
        Task<IEnumerable<ComparePropertyDTO>> ComparePropertiesAsync(List<int> ids);

        Task<List<PropertyDetailDTO>> GetPropertiesByIdsAsync(List<int> ids);


        //Elasticsearch
        Task<bool> IndexPropertyAsync(PropertySearchDTO dto);
        Task BulkIndexPropertiesAsync();
        Task<IEnumerable<HomePropertyDTO>> SearchAsync(string keyword);
        Task<IEnumerable<HomePropertyDTO>> SearchAdvanceAsync(int? province = 0, int? ward = 0, int? street = 0,int? userId = 0);

        Task<List<ProvinceDTO>> GetListLocationAsync();

        // Address APIs
        Task<IEnumerable<Province>> GetProvincesAsync();
        Task<IEnumerable<Street>> GetStreetAsync(int wardId);
        Task<IEnumerable<Ward>> GetWardsAsync(int provinces);

       Task<IEnumerable<Amenity>> GetAmenitiesAsync();


    }
}
