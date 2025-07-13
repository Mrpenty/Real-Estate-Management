using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Wallet
{
    public class WalletService
    {
        private readonly RentalDbContext _context;

        public WalletService(RentalDbContext context)
        {

            _context = context;
        }

        public async Task<decimal> GetBalanceAsync(int userId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null)
            {
                var allWallets = await _context.Wallets.ToListAsync();
                throw new Exception($"Wallet not found for UserId={userId}. Current wallets in DB: {allWallets.Count}");
            }

            return wallet.Balance;
        }

        public async Task CreateWalletAsync(int userId)
        {
            var wallet = new RealEstateManagement.Data.Entity.Payment.Wallet
            {
                UserId = userId,
                Balance = 0
            };

            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
        }

    }
}
