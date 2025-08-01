﻿using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Location;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
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

        //Lấy 1 property theo id
        Task<Property> GetPropertyByIdAsync(int id);
        Task<IEnumerable<Property>> FilterByPriceAsync([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice);
        Task<IEnumerable<Property>> FilterByAreaAsync(decimal? minArea, decimal? maxArea);
        //Sắp xếp nâng cao
        Task<IEnumerable<Property>> FilterAdvancedAsync(PropertyFilterDTO filter);
        //So sánh property (tối đa là 3)
        Task<IEnumerable<Property>> ComparePropertiesAsync(List<int> ids);

        //Lấy nhiều property để so sánh
        Task<List<Property>> GetPropertiesByIdsAsync(List<int> ids);

        Task<List<ProvinceDTO>> GetListLocationAsync();

        Task<IEnumerable<Property>> FilterByTypeAsync(string type);
        //Lấy tất cả property theo landlordId
        Task<ApplicationUser?> GetUserByIdAsync(int userId);
        Task<List<Property>> GetPropertiesByLandlordIdAsync(int landlordId);
    }
}
