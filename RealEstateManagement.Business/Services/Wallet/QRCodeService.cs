using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Wallet
{
    public class QRCodeService
    {
        private readonly RentalDbContext _context;

        public QRCodeService(RentalDbContext context)
        {
            _context = context;
        }
        public async Task DepositAsync(int userId, decimal amount)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
                throw new Exception("Wallet not found");

            wallet.Balance += amount;

            _context.WalletTransactions.Add(new WalletTransaction
            {
                WalletId = wallet.Id,
                Amount = amount,
                Type = "Deposit",
                Description = $"User nạp {amount} VNĐ qua QR"
            });

            await _context.SaveChangesAsync();
        }

    }
}
