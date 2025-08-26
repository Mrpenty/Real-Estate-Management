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
    public class WalletService_CreateWalletAsync_Tests
    {
        private RentalDbContext _db;
        private WalletService _svc;

        private RentalDbContext CreateInMemory()
        {
            var opts = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase($"Wallet_Create_{Guid.NewGuid()}")
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
        public async Task Create_Wallet_With_Zero_Balance()
        {
            await _svc.CreateWalletAsync(77);

            var wallet = await _db.Wallets.SingleAsync();
            Assert.AreEqual(77, wallet.UserId);
            Assert.AreEqual(0m, wallet.Balance);
        }

        [TestMethod]
        public async Task Create_Multiple_Wallets_For_Different_Users()
        {
            await _svc.CreateWalletAsync(1);
            await _svc.CreateWalletAsync(2);

            var count = await _db.Wallets.CountAsync();
            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public async Task Persisted_In_Db()
        {
            await _svc.CreateWalletAsync(10);
            var exists = await _db.Wallets.AnyAsync(w => w.UserId == 10);
            Assert.IsTrue(exists);
        }
    }
}
