using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.OwnerRepo
{
    public interface IRentalContractRepository
    {
        Task<RentalContract> GetByPostIdAsync(int id); //Xem hợp đồng của bài Post đó
        Task<RentalContract> GetByRentalContractIdAsync(int id); // Lấy RentalContract Id
        Task AddAsync(RentalContract contract, int owner, int propertyPost);
        Task UpdateContractAsync(RentalContract contract );
        Task DeleteAsync(int id);

        Task UpdateStatusAsync(int contractId, RentalContract.ContractStatus status);
    }

}
