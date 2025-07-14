using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Payment;

namespace RealEstateManagement.Data.Data.DataSeed.PaymentSeed
{
    public static class WalletTransactionSeed
    {
        public static void SeedWalletTransactions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WalletTransaction>().HasData(
                new WalletTransaction
                {
                    Id = 1,
                    WalletId = 1,
                    Amount = 100000,
                    Type = "Deposit",
                    Description = "Nạp thử",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
