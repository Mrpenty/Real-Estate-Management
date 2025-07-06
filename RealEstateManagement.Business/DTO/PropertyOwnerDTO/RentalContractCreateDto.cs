using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class RentalContractCreateDto
    {
        public int PropertyPostId { get; set; }
        public int LandlordId { get; set; }
        public int? RenterId { get; set; }  // Có thể để trống ban đầu

        public decimal DepositAmount { get; set; }
        public decimal MonthlyRent { get; set; }
        public int ContractDurationMonths { get; set; }

        public RentalContract.PaymentCycleType PaymentCycle { get; set; } = RentalContract.PaymentCycleType.Monthly;
        public int PaymentDayOfMonth { get; set; } = 5;


        public DateTime? StartDate { get; set; }

        public string PaymentMethod { get; set; }
        public string ContactInfo { get; set; }
    }


}
