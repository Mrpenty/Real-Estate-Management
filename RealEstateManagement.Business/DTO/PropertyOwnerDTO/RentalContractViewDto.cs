using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class RentalContractViewDto
    {
        public int Id { get; set; }
        public int PropertyPostId { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal MonthlyRent { get; set; }
        public int ContractDurationMonths { get; set; }
        public RentalContract.PaymentCycleType PaymentCycle { get; set; }
        public int PaymentDayOfMonth { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PaymentMethod { get; set; }
        public string ContactInfo { get; set; }
        public RentalContract.ContractStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
    }
}
