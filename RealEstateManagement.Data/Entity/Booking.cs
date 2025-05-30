using System.Diagnostics.Contracts;

namespace RealEstateManagement.Data.Entity
{
    public class Booking
    {
        public int Id { get; set; }
        public int RenterId { get; set; }
        public int PropertyId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; } // pending, approved, rejected, completed
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DepositStatus { get; set; } // pending, paid
        public DateTime CreatedAt { get; set; }

        public ApplicationUser Renter { get; set; }
        public Property Property { get; set; }
        public Contract Contract { get; set; }
    }
}
