using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Payment;
using System;

namespace RealEstateManagement.Data.Data.DataSeed.PaymentSeed.PaymentSeed
{
    public static class TransactionSeed
    {
        public static void SeedTransactions(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().HasData(
                new Transaction
                {
                    Id = 1,
                    UserId = 3,
                    Amount = 5000000,
                    TransactionType = "deposit",
                    Description = "Deposit for apartment in District 1",
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}