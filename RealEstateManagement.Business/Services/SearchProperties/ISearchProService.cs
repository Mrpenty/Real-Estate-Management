using RealEstateManagement.Business.DTO.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.SearchProperties
{
    public interface ISearchProService
    {
        //Elasticsearch
        Task<bool> IndexPropertyAsync(PropertySearchDTO dto);
        Task BulkIndexPropertiesAsync();
        Task<IEnumerable<HomePropertyDTO>> SearchAsync(string keyword);
    }
}
