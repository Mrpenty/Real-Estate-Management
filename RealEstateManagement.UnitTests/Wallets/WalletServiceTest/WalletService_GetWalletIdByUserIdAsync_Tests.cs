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
    public class WalletService_GetWalletIdByUserIdAsync_Tests
    {
        private RentalDbContext _db;
        private WalletService _svc;

        private RentalDbContext CreateInMemory()
        {
            var opts = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase($"Wallet_GetWalletId_{Guid.NewGuid()}")
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
        public async Task Return_Id_When_User_Has_Wallet()
        {
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 5, UserId = 100, Balance = 0m });
            await _db.SaveChangesAsync();

            var id = await _svc.GetWalletIdByUserIdAsync(100);

            Assert.AreEqual(5, id);
        }

        [TestMethod]
        public async Task Return_Null_When_User_Has_No_Wallet()
        {
            var id = await _svc.GetWalletIdByUserIdAsync(999);
            Assert.IsNull(id);
        }

        [TestMethod]
        public async Task Return_Correct_Id_With_Many_Wallets()
        {
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 1, UserId = 1, Balance = 0 });
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 2, UserId = 2, Balance = 0 });
            _db.Wallets.Add(new Data.Entity.Payment.Wallet { Id = 3, UserId = 3, Balance = 0 });
            await _db.SaveChangesAsync();

            var id = await _svc.GetWalletIdByUserIdAsync(3);
            Assert.AreEqual(3, id);
        }
    }
}
