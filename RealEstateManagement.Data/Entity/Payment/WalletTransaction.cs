using RealEstateManagement.Data.Entity.User;

namespace RealEstateManagement.Data.Entity.Payment
{
    public class WalletTransaction
    {
        public int Id { get; set; }
        public int WalletId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } // VNPAY, MOMO, Thanh toán
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Wallet Wallet { get; set; }
    }
}
