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

        //Xem tiền của mình
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

        //Lấy WalletId
        public async Task<int?> GetWalletIdByUserIdAsync(int userId)
        {
            return await _context.Wallets
                .Where(w => w.UserId == userId)
                .Select(w => (int?)w.Id)
                .FirstOrDefaultAsync();
        }

        //Tạo ví tiền khi tạo tài khoản
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

        //User Xem lịch sử giao dịch
        public async Task<List<WalletTransaction>> ViewTransactionHistory(int walletId)
        {
            var list = await _context.WalletTransactions
                .Where(d => d.WalletId == walletId)
                .ToListAsync();
            if (list == null || list.Count == 0)
            {
                throw new Exception("Tài khoản này chưa từng giao dịch tiền");
            }
            return list;
        }
    }
}
