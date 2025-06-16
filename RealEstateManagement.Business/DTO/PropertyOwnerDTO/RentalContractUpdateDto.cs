using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class RentalContractUpdateDto
    {
        public decimal DepositAmount { get; set; }
        public decimal MonthlyRent { get; set; }
        public int ContractDurationMonths { get; set; }
        public DateTime? StartDate { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
    }
}
