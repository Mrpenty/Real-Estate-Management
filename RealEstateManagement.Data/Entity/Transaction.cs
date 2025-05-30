namespace RealEstateManagement.Data.Entity
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } // deposit, rent, promotion, refund
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public ApplicationUser User { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
