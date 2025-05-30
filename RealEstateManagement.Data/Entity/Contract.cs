namespace RealEstateManagement.Data.Entity
{
    public class Contract
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Deposit { get; set; }
        public decimal RentAmount { get; set; }
        public string Status { get; set; } // active, expired, cancelled
        public DateTime CreatedAt { get; set; }

        public Booking Booking { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
