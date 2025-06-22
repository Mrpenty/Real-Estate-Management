using RealEstateManagement.Business.DTO.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.SearchProperties
{
    public interface ISearchProRepo
    {
        //Elasticsearch
        Task<bool> IndexPropertyAsync(PropertySearchDTO dto);
        Task BulkIndexPropertiesAsync(IEnumerable<PropertySearchDTO> properties);
        Task<List<int>> SearchPropertyIdsAsync(string keyword);
    }
}
