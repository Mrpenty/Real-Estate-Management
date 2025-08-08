using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Payment;

namespace RealEstateManagement.Data.Data.DataSeed.PaymentSeed
{
    public static class WalletSeed
    {
        public static void SeedWallets(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Wallet>().HasData(
                new Wallet
                {
                    Id = 1,
                    UserId = 2,
                    Balance = 1000000 // Ví khởi tạo 0 đồng
                },
                new Wallet
                {
                    Id = 2,
                    UserId = 5,
                    Balance = 500000 // Ví khởi tạo 0 đồng
                },
                new Wallet
                {
                    Id = 3,
                    UserId = 6,
                    Balance = 500000 // Ví khởi tạo 0 đồng
                }
            );
        }
    }
}
