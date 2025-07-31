using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nest;
using Net.payOS;
using Net.payOS.Types;
//using RealEstateManagement.Business.DTO.WalletDTO;
using RealEstateManagement.Data.Entity.Payment;
using System.Text.Json;

namespace RealEstateManagement.Business.Services.Wallet
{
    public class QRCodeService
    {
        private readonly RentalDbContext _context;
        private readonly WalletService _walletService;
        private readonly IConfiguration _config;
        private readonly PayOS _payOS;

        public QRCodeService(RentalDbContext context, IConfiguration config, WalletService walletService, PayOS payOS)
        {
            _context = context;
            _config = config;
            _walletService = walletService;
            _payOS = payOS;
        }

        public async Task<string> CreatePaymentUrl(int userId, decimal amount)
        {
            var walletId = await _walletService.GetWalletIdByUserIdAsync(userId);
            if (walletId == null)
                throw new ArgumentException("User không có ví");

            var orderCode = int.Parse(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString().Substring(4, 6));
            var items = new List<ItemData>
    {
        new ItemData(amount.ToString(), 1, (int)amount)
    };

            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: (int)amount,
                description: amount.ToString(),
                items: items,
                cancelUrl: _config["PayOS:CancelUrl"] ?? "https://localhost:7160/api/payos/cancel",
                returnUrl: _config["PayOS:ReturnUrl"] ?? "https://localhost:7160/api/payos/success"
            );

            // Gọi PayOS trước
            var result = await _payOS.createPaymentLink(paymentData);

            // Sau khi thành công mới tạo transaction
            var transaction = new WalletTransaction
            {
                WalletId = walletId.Value,
                Amount = amount,
                Type = "PayOS",
                Description = amount.ToString(),
                Status = "PENDING",
                CreatedAt = DateTime.UtcNow,
                CheckoutUrl = result.checkoutUrl,
                PayOSOrderCode = result.orderCode.ToString()
            };

            _context.WalletTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            return result.ToString();
        }

        public async Task<bool> CheckAndUpdateDepositStatusAsync(long orderCode)
        {
            // 1. Gọi PayOS để lấy status
            var status = await _payOS.getPaymentLinkInformation(orderCode);

            // 2. Nếu chưa thanh toán xong hoặc status null => bỏ qua
            if (string.IsNullOrEmpty(status.status) || status.status != "PAID")
            {
                return false;
            }

            // 3. Tìm lại giao dịch theo orderCode
            var deposit = await _context.WalletTransactions.FirstOrDefaultAsync(d => d.PayOSOrderCode == orderCode.ToString());
            if (deposit == null || deposit.Status == "Success") return false;

            // 4. Cập nhật số dư ví người dùng
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == deposit.WalletId);
            if (wallet == null) throw new Exception("Ví không tồn tại");

            wallet.Balance += deposit.Amount;
            deposit.Status = "Success";

            await _context.SaveChangesAsync();
            return true;
        }


        //public async Task ProcessPayOSWebhookAsync(WebhookData data)
        //{
        //    // Lấy transaction bằng orderCode từ webhook
        //    var transaction = await _context.WalletTransactions
        //        .FirstOrDefaultAsync(x => x.PayOSOrderCode == data.orderCode.ToString());

        //    if (transaction == null)
        //        throw new Exception("Không tìm thấy giao dịch");

        //    // Nếu transaction đã hoàn tất thì bỏ qua
        //    if (transaction.Status == "COMPLETED")
        //        return;

        //    if (data.code == "00")
        //    {
        //        transaction.Status = "COMPLETED";

        //        var wallet = await _context.Wallets.FindAsync(transaction.WalletId);
        //        if (wallet == null)
        //            throw new Exception("Không tìm thấy ví");

        //        wallet.Balance += transaction.Amount;

        //        await _context.SaveChangesAsync();
        //    }
        //    else
        //    {
        //        // Nếu muốn xử lý thêm trạng thái CANCELLED, FAILED,... thì thêm ở đây
        //        transaction.Status = "FAILED";
        //        await _context.SaveChangesAsync();
        //    }
        //}

        //public WebhookData VerifyPaymentWebhookData(WebhookType body)
        //{
        //    return _payOS.verifyPaymentWebhookData(body);
        //}

        //public async Task ConfirmWebhook()
        //{
        //    await _payOS.confirmWebhook("https://localhost:7160/api/Deposit/payos_webhook");
        //}
    }
}
