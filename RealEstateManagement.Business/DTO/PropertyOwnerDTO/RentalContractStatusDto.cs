using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class RentalContractStatusDto
    {
        public int ContractId { get; set; }
        public RentalContract.ContractStatus Status { get; set; } = RentalContract.ContractStatus.Pending;
    }
}
