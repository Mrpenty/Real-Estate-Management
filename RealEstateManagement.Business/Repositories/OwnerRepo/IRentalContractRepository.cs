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
        Task<RentalContract> GetByIdAsync(int id); //Cho Detailt
        Task<IEnumerable<RentalContract>> GetByPostIdAsync(int postId); //Lấy tất cả hợp đồng liên quan tới bài đăng đó
        Task AddAsync(RentalContract contract);
        Task UpdateContractAsync(RentalContract contract );
        Task DeleteAsync(int id);

        Task UpdateStatusAsync(int contractId, RentalContract.ContractStatus status);
    }

}
