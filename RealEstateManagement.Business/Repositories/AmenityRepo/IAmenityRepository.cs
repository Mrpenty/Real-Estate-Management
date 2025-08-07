using RealEstateManagement.Business.Repositories.Repository;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.AmenityRepo
{
    public interface IAmenityRepository : IRepositoryAsync<Amenity>
    {
        Task<List<Amenity>> GetPagedAsync(int skip, int take);
        Task<int> GetTotalCountAsync();
    }
}
