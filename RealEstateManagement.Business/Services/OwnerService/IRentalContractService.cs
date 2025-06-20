using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.OwnerService
{
    public interface IRentalContractService
    {
        Task<RentalContract> GetByIdAsync(int id);
        Task<IEnumerable<RentalContract>> GetByPostIdAsync(int postId);
        Task AddAsync(RentalContractCreateDto dto);
        Task UpdateStatusAsync(RentalContractStatusDto statusDto);
        Task UpdateContractAsync(int contractId, RentalContractUpdateDto dto);
        Task DeleteAsync(int id);
    }

}
