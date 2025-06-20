using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class RentalContractDetailDto
    {
        public int Id { get; set; }

        public int PropertyPostId { get; set; }
        public int LandlordId { get; set; }
        public int? RenterId { get; set; }

        public decimal DepositAmount { get; set; }
        public decimal MonthlyRent { get; set; }
        public int ContractDurationMonths { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;

        public RentalContract.ContractStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
    }

}
