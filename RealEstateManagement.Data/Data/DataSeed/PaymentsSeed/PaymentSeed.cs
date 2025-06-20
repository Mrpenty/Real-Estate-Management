using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Payment;
using System;

namespace RealEstateManagement.Data.Data.DataSeed.PaymentsSeed
{
    public static class PaymentSeed
    {
        public static void SeedPayments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().HasData(
                new Payment
                {
                    Id = 1,
                    ContractId = 1,
                    Amount = 5000000,
                    PaymentMethod = "Momo",
                    Status = "completed",
                    PaidAt = DateTime.Now,
                    TransactionId = 1
                }
            );
        }
    }
}