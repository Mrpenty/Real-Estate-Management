using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Moq;
using Net.payOS.Types;
using RealEstateManagement.Business.Services.Wallet;
using RealEstateManagement.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Wallets.QRCodeServiceTest
{
    [TestClass]
    public class QRCodeService_CreatePaymentUrl_Tests
    {
        //private RentalDbContext _db;
        //private WalletService _walletService;
        //private Mock<IConfiguration> _config;
        //private Mock<IPayOSClient> _payOS;
        //private QRCodeService _svc;

        //private RentalDbContext CreateInMemory()
        //{
        //    var options = new DbContextOptionsBuilder<RentalDbContext>()
        //        .UseInMemoryDatabase($"QRCode_Create_{Guid.NewGuid()}")
        //        .Options;
        //    return new RentalDbContext(options);
        //}

        //[TestInitialize]
        //public void Setup()
        //{
        //    _db = CreateInMemory();
        //    _walletService = new WalletService(_db); // dùng thật cho chắc
        //    _config = new Mock<IConfiguration>();
        //    _payOS = new Mock<IPayOSClient>();
        //    _svc = new QRCodeService(_db, _config.Object, _walletService, _payOS.Object);
        //}

        //[TestCleanup]
        //public void Cleanup() => _db.Dispose();

        //[TestMethod]
        //public async Task Success_Saves_Pending_Tx_And_Returns_CheckoutUrl()
        //{
        //    // Có ví cho userId=1
        //    _db.Wallets.Add(new Wallet { Id = 10, UserId = 1, Balance = 0m });
        //    await _db.SaveChangesAsync();

        //    // PayOS trả link thành công
        //    var created = new CreatePaymentResult(
        //        bin: "9704", accountNumber: "123", amount: 500, description: "500",
        //        orderCode: 654321, paymentLinkId: "plink", status: "PENDING",
        //        checkoutUrl: "https://checkout/654321", qrCode: "qr"
        //    );
        //    _payOS.Setup(p => p.CreatePaymentLink(It.IsAny<PaymentData>()))
        //          .ReturnsAsync(created);

        //    var url = await _svc.CreatePaymentUrl(1, 500m);

        //    Assert.AreEqual("https://checkout/654321", url);

        //    var tx = await _db.WalletTransactions.SingleAsync();
        //    Assert.AreEqual(10, tx.WalletId);
        //    Assert.AreEqual(500m, tx.Amount);
        //    Assert.AreEqual("PENDING", tx.Status);
        //    Assert.AreEqual("PayOS", tx.Type);
        //    Assert.AreEqual("https://checkout/654321", tx.CheckoutUrl);
        //    Assert.AreEqual("654321", tx.PayOSOrderCode);
        //}

        //[TestMethod]
        //public async Task Throw_When_User_Has_No_Wallet()
        //{
        //    // Không seed ví cho userId=2
        //    _payOS.Setup(p => p.CreatePaymentLink(It.IsAny<PaymentData>()))
        //         .ThrowsAsync(new Exception("should not be called"));

        //    await Assert.ThrowsExceptionAsync<ArgumentException>(() => _svc.CreatePaymentUrl(2, 100m));
        //    Assert.AreEqual(0, await _db.WalletTransactions.CountAsync());
        //}

        //[TestMethod]
        //public async Task Uses_Default_Cancel_And_ReturnUrl_When_Config_Missing()
        //{
        //    _db.Wallets.Add(new Wallet { Id = 11, UserId = 3, Balance = 0m });
        //    await _db.SaveChangesAsync();

        //    PaymentData captured = null!;
        //    var created = new CreatePaymentResult(
        //        "9704", "123", 100, "100", 123456, "plink", "PENDING", "https://checkout", "qr"
        //    );
        //    _payOS.Setup(p => p.CreatePaymentLink(It.IsAny<PaymentData>()))
        //          .Callback<PaymentData>(pd => captured = pd)
        //          .ReturnsAsync(created);

        //    var _ = await _svc.CreatePaymentUrl(3, 100m);

        //    Assert.AreEqual("https://localhost:7160/api/payos/cancel", captured.cancelUrl);
        //    Assert.AreEqual("https://localhost:7160/api/payos/success", captured.returnUrl);
        //}

        //[TestMethod]
        //public async Task Do_Not_Insert_Tx_If_PayOS_Fails()
        //{
        //    _db.Wallets.Add(new Wallet { Id = 12, UserId = 4, Balance = 0m });
        //    await _db.SaveChangesAsync();

        //    _payOS.Setup(p => p.CreatePaymentLink(It.IsAny<PaymentData>()))
        //         .ThrowsAsync(new Exception("PayOS down"));

        //    await Assert.ThrowsExceptionAsync<Exception>(() => _svc.CreatePaymentUrl(4, 999m));
        //    Assert.AreEqual(0, await _db.WalletTransactions.CountAsync());
        //}
    }
}
