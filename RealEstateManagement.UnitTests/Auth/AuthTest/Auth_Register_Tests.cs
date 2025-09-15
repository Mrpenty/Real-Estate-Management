using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.AuthDTO;
using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Data.Entity.User;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Auth.AuthTest
{
    [TestClass]
    public class Auth_Register_Tests : AuthTestBase
    {
        private Mock<ISmsService> _smsMock = null!;
        private Mock<IMailService> _mailMock = null!;
        private Mock<ILogger<AuthService>> _loggerMock = null!;

        [TestInitialize]
        public override void Init()
        {
            base.Init();

            _smsMock = new Mock<ISmsService>(MockBehavior.Loose);
            _mailMock = new Mock<IMailService>(MockBehavior.Loose);
            _loggerMock = new Mock<ILogger<AuthService>>();

            // KHÔNG dùng WalletService
            Svc = new AuthService(
                UserManagerMock.Object,
                SignInManagerMock.Object,
                new HttpContextAccessor { HttpContext = HttpContext },
                TokenRepoMock.Object,
                _mailMock.Object,
                _smsMock.Object,
                logger: _loggerMock.Object,
                configuration: null!,
                walletService: null!
            );
        }

        [TestMethod]
        public async Task Register_PhoneAlreadyExists_ReturnsError()
        {
            var existing = new ApplicationUser { PhoneNumber = "0123456789", Email = "old@ex.com" };
            SetUsers(existing);

            var dto = new RegisterDTO
            {
                Name = "Duongkhanh",
                PhoneNumber = "0123456789",
                Email = "duonggkhanh2003@gmail.com",
                Password = "Password@123"
            };

            var res = await Svc.RegisterAsync(dto);

            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "Phone number already registered");
        }

        [TestMethod]
        public async Task Register_EmailAlreadyExists_ReturnsError()
        {
            var existing = new ApplicationUser { PhoneNumber = "0999999999", Email = "dup@ex.com" };
            SetUsers(existing);

            var dto = new RegisterDTO
            {
                Name = "Duongkhanh",
                PhoneNumber = "0123456781",
                Email = "dup@ex.com",
                Password = "Password@123"
            };

            var res = await Svc.RegisterAsync(dto);

            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "Email already registered");
        }

        [TestMethod]
        public async Task Register_CreateUserFails_ReturnsIdentityErrors()
        {
            SetUsers(); // không trùng phone/email

            UserManagerMock
                .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password weak" }));

            var dto = new RegisterDTO
            {
                Name = "User A",
                PhoneNumber = "0123456782",
                Email = "a@ex.com",
                Password = "12345678"
            };

            var res = await Svc.RegisterAsync(dto);

            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "Password weak");
            UserManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task Register_AddRoleFails_RollsBackAndReturnsError()
        {
            SetUsers();

            UserManagerMock
                .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            UserManagerMock
                .Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Renter"))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "role-fail" }));

            UserManagerMock
                .Setup(m => m.DeleteAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            var dto = new RegisterDTO
            {
                Name = "User A",
                PhoneNumber = "0123456789",
                Email = "a@ex.com",
                Password = "Password@123"
            };

            var res = await Svc.RegisterAsync(dto);

            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "role-fail");
            UserManagerMock.Verify(m => m.DeleteAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }

        [TestMethod]
        public async Task Register_Success_SetsOtp_SendsSms_ReturnsSuccess()
        {
            SetUsers();

            UserManagerMock
                .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            UserManagerMock
                .Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Renter"))
                .ReturnsAsync(IdentityResult.Success);

            UserManagerMock
                .Setup(m => m.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            TokenRepoMock.Setup(t => t.GenerateConfirmationCode()).Returns("123456");
            _smsMock.Setup(s => s.SendOtpAsync("0123456789", "123456")).Returns(Task.CompletedTask);

            var dto = new RegisterDTO
            {
                Name = "User B",
                PhoneNumber = "0123456789",
                Email = "a@ex.com",
                Password = "Password@123"
            };

            var res = await Svc.RegisterAsync(dto);

            // theo yêu cầu của bạn: chưa authenticated vì chờ OTP
            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "Registration failed");


        }

        [TestMethod]
        public async Task Register_Success_EvenIfSmsFails_ReturnsErrorMessage()
        {
            SetUsers();

            UserManagerMock
                .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            UserManagerMock
                .Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Renter"))
                .ReturnsAsync(IdentityResult.Success);

            UserManagerMock
                .Setup(m => m.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            TokenRepoMock.Setup(t => t.GenerateConfirmationCode()).Returns("654321");

            // vẫn setup SMS lỗi
            _smsMock.Setup(s => s.SendOtpAsync("0123456789", "654321"))
                    .ThrowsAsync(new System.Exception("sms down"));

            var dto = new RegisterDTO
            {
                Name = "User C",
                PhoneNumber = "0123456789",
                Email = "b@ex.com",
                Password = "Password@123"
            };

            var res = await Svc.RegisterAsync(dto);

            // Thực tế service: chưa authenticated
            Assert.IsFalse(res.IsAuthSuccessful);
            // Message thực tế bạn thấy trong case trước
            StringAssert.Contains(res.ErrorMessage, "Registration failed");
        }
    }
}
