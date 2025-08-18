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
        Task<RentalContractViewDto> GetByPostIdAsync(int id);
        Task AddAsync(RentalContractCreateDto dto, int ownerId, int propertyPostId);
        Task UpdateStatusAsync(RentalContractStatusDto statusDto);
        Task UpdateContractAsync(int contractId, RentalContractUpdateDto dto);
        Task DeleteAsync(int id);

        Task<RentalPaymentResultDto> PayAsync(int contractId);

        Task ProposeRenewalAsync(int contractId, RentalContractRenewalDto dto);

        Task RespondRenewalAsync(int contractId, bool approve);

        Task<RentalContractRenewalDto?> GetRenewalProposalAsync(int contractId);

    }

}
