using System;

namespace RealEstateManagement.Data.Entity
{
    public class RentalContract
    {
        public int Id { get; set; }

        public int PropertyPostId { get; set; }
        public int LandlordId { get; set; }
        public int? RenterId { get; set; }

        public decimal DepositAmount { get; set; }
        public decimal MonthlyRent { get; set; }
        public int ContractDurationMonths { get; set; } // thời hạn hợp đồng
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate => StartDate?.AddMonths(ContractDurationMonths);

        public string PaymentMethod { get; set; }
        public string ContactInfo { get; set; }

        public ContractStatus Status { get; set; } = ContractStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ConfirmedAt { get; set; }

        public PropertyPost PropertyPost { get; set; }
        public ApplicationUser Landlord { get; set; }
        public ApplicationUser Renter { get; set; }

        public enum ContractStatus
        {
            Pending = 0,
            Confirmed = 1,
            Rejected = 2,
            Cancelled = 3
        }
    }
}
