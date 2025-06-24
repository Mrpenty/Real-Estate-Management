using RealEstateManagement.Business.Repositories.Repository;
using RealEstateManagement.Data.Entity.AddressEnity;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.AddressRepo
{
    public interface IAddressRepository : IRepositoryAsync<Address>
    {
        Task<int> SaveChangesAsync();
        Task<Address> GetByDetailsAsync(int provinceId, int wardId, string street);
    }
} 