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
    public class WalletService_GetBalanceAsync_Tests
    {
        private RentalDbContext _db;
        private WalletService _svc;

        private RentalDbContext CreateInMemory()
        {
            var opts = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase($"Wallet_GetBalance_{Guid.NewGuid()}")
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
        public async Task Return_Balance_When_Wallet_Exists()
        {
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 1, UserId = 10, Balance = 123.45m });
            await _db.SaveChangesAsync();

            var bal = await _svc.GetBalanceAsync(10);

            Assert.AreEqual(123.45m, bal);
        }

        [TestMethod]
        public async Task Throw_When_Wallet_Not_Found()
        {
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 1, UserId = 99, Balance = 1m });
            await _db.SaveChangesAsync();

            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _svc.GetBalanceAsync(10));
            StringAssert.Contains(ex.Message, "Wallet not found");
        }

        [TestMethod]
        public async Task Return_Correct_Balance_With_Multiple_Wallets()
        {
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 1, UserId = 1, Balance = 10m });
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 2, UserId = 2, Balance = 20m });
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 3, UserId = 3, Balance = 30m });
            await _db.SaveChangesAsync();

            var bal = await _svc.GetBalanceAsync(2);

            Assert.AreEqual(20m, bal);
        }
    }
}
