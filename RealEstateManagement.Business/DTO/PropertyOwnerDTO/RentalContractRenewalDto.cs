using System;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.Business.DTO.PropertyOwnerDTO
{
    public class RentalContractRenewalDto
    {
        public decimal? ProposedMonthlyRent { get; set; }
        public int? ProposedContractDurationMonths { get; set; }
        public RentalContract.PaymentCycleType? ProposedPaymentCycle { get; set; }
        public int? ProposedPaymentDayOfMonth { get; set; }
        public DateTime? ProposedEndDate { get; set; }
        public DateTime? ProposedAt { get; set; }
        public bool? RenterApproved { get; set; }

    }
}
