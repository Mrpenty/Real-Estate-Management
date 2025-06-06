namespace RealEstateManagement.Data.Entity
{
    public class Payment
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // Momo, ZaloPay, VNPay
        public string Status { get; set; } // pending, completed, failed
        public DateTime? PaidAt { get; set; }
        public int? TransactionId { get; set; }

        public Transaction Transaction { get; set; }
    }
}
