using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.Services.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Wallets.WalletServiceTest
{
    [TestClass]
    public class WalletService_DeductBalanceAsync_Tests
    {
        private RentalDbContext _db;
        private WalletService _svc;

        private RentalDbContext CreateInMemory()
        {
            var opts = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase($"Wallet_Deduct_{Guid.NewGuid()}")
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
        public async Task Throw_When_Amount_Not_Positive()
        {
            await _db.Wallets.AddAsync(new Data.Entity.Payment.Wallet { Id = 1, UserId = 10, Balance = 100m });
            await _db.SaveChangesAsync();

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _svc.DeductBalanceAsync(10, 0m, "zero"));
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => _svc.DeductBalanceAsync(10, -5m, "negative"));
        }

        [TestMethod]
        public async Task Throw_When_Wallet_Not_Found()
        {
            await Assert.ThrowsExceptionAsync<Exception>(() => _svc.DeductBalanceAsync(99, 10m, "x"));
        }

        [TestMethod]
        public async Task Return_False_When_Insufficient_Balance()
        {
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 1, UserId = 10, Balance = 5m });
            await _db.SaveChangesAsync();

            var ok = await _svc.DeductBalanceAsync(10, 10m, "too much");

            Assert.IsFalse(ok);

            var wallet = await _db.Wallets.SingleAsync(w => w.Id == 1);
            Assert.AreEqual(5m, wallet.Balance);                  // không đổi
            Assert.AreEqual(0, await _db.WalletTransactions.CountAsync()); // không ghi transaction
        }

        [TestMethod]
        public async Task Return_True_And_Write_Transaction_When_Sufficient()
        {
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 2, UserId = 20, Balance = 100m });
            await _db.SaveChangesAsync();

            var ok = await _svc.DeductBalanceAsync(20, 30m, "rent");

            Assert.IsTrue(ok);

            var wallet = await _db.Wallets.SingleAsync(w => w.Id == 2);
            Assert.AreEqual(70m, wallet.Balance);

            var tx = await _db.WalletTransactions.SingleAsync();
            Assert.AreEqual(2, tx.WalletId);
            Assert.AreEqual(-30m, tx.Amount);               // số âm = khoản chi
            Assert.AreEqual("Deduct", tx.Type);
            Assert.AreEqual("Success", tx.Status);
            Assert.AreEqual("rent", tx.Description);
            Assert.IsTrue(tx.CreatedAt > DateTime.MinValue);
        }
    }
}
