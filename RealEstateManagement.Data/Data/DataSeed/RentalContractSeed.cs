using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity;
using System;

namespace RealEstateManagement.Data.Data.DataSeed
{
    public static class RentalContractSeed
    {
        public static void SeedRentalContracts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RentalContract>().HasData(
    new RentalContract
    {
        Id = 1,
        PropertyPostId = 1,
        LandlordId = 2,
        RenterId = 3,
        DepositAmount = 2000000,
        MonthlyRent = 5000000,
        ContractDurationMonths = 12,
        PaymentCycle = RentalContract.PaymentCycleType.Monthly,
        PaymentDayOfMonth = 5,
        StartDate = new DateTime(2025, 7, 1),
        PaymentMethod = "Bank Transfer",
        ContactInfo = "renter@example.com | 03345678910",
        Status = RentalContract.ContractStatus.Confirmed,
        CreatedAt = DateTime.Now,
        ConfirmedAt = DateTime.Now
    },
    new RentalContract
    {
        Id = 2,
        PropertyPostId = 2,
        LandlordId = 2,
        RenterId = 4,
        DepositAmount = 1500000,
        MonthlyRent = 2000000,
        ContractDurationMonths = 6,
        PaymentCycle = RentalContract.PaymentCycleType.Quarterly,
        PaymentDayOfMonth = 10,
        StartDate = new DateTime(2025, 8, 1),
        PaymentMethod = "Momo",
        ContactInfo = "renter2@example.com | 0322222222",
        Status = RentalContract.ContractStatus.Pending,
        CreatedAt = DateTime.Now
    }
);

        }
    }
}