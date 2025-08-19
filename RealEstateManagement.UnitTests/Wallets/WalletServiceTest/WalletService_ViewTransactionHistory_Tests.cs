using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.Services.Wallet;
using RealEstateManagement.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Wallets.WalletServiceTest
{
    [TestClass]
    public class WalletService_ViewTransactionHistory_Tests
    {
        private RentalDbContext _db;
        private WalletService _svc;

        private RentalDbContext CreateInMemory()
        {
            var opts = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase($"Wallet_ViewHistory_{Guid.NewGuid()}")
                .Options;
            return new RentalDbContext(opts);
        }

        [TestInitialize]
        public void Setup()
        {
            _db = CreateInMemory();
            _svc = new WalletService(_db);
        }

        [TestCleanup]
        public void Cleanup() => _db.Dispose();

        [TestMethod]
        public async Task Return_Transactions_For_Wallet()
        {
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 1, UserId = 10, Balance = 0 });
            _db.WalletTransactions.Add(new WalletTransaction
            {
                WalletId = 1,
                Amount = 10m,
                Type = "X",
                Status = "Success",
                Description = "seed-1",
                CreatedAt = DateTime.UtcNow
            });
            _db.WalletTransactions.Add(new WalletTransaction
            {
                WalletId = 1,
                Amount = -5m,
                Type = "Y",
                Status = "Success",
                Description = "seed-2",
                CreatedAt = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();

            var list = await _svc.ViewTransactionHistory(1);
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public async Task Throw_When_No_Transactions()
        {
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 2, UserId = 20, Balance = 0 });
            await _db.SaveChangesAsync();

            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _svc.ViewTransactionHistory(2));
            StringAssert.Contains(ex.Message, "chưa từng giao dịch");
        }

        [TestMethod]
        public async Task Only_Transactions_Of_Target_Wallet()
        {
            _db.Wallets.AddRange(
                new Data.Entity.Payment.Wallet { Id = 1, UserId = 1, Balance = 0 },
                new Data.Entity.Payment.Wallet { Id = 2, UserId = 2, Balance = 0 }
            );
            _db.WalletTransactions.AddRange(
                new WalletTransaction
                {
                    WalletId = 1,
                    Amount = 1,
                    Type = "A",
                    Status = "Success",
                    Description = "A1",
                    CreatedAt = DateTime.UtcNow
                },
                new WalletTransaction
                {
                    WalletId = 2,
                    Amount = 2,
                    Type = "B",
                    Status = "Success",
                    Description = "B2",
                    CreatedAt = DateTime.UtcNow
                },
                new WalletTransaction
                {
                    WalletId = 1,
                    Amount = 3,
                    Type = "C",
                    Status = "Success",
                    Description = "C3",
                    CreatedAt = DateTime.UtcNow
                }
            );
            await _db.SaveChangesAsync();

            var list = await _svc.ViewTransactionHistory(1);

            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.All(x => x.WalletId == 1));
        }
    }
}
