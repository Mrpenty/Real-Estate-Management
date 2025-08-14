using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Identity;
using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Business.DTO.AuthDTO;
using RealEstateManagement.Data.Entity.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using RealEstateManagement.Business.Repositories.Token;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using RealEstateManagement.Business.Services.Wallet;
using RealEstateManagement.Business.Services.Mail;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace RealEstateManagement.UnitTests
{
    [TestClass]
    public class Login_UnitTest
    {
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<SignInManager<ApplicationUser>> _mockSignInManager;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private Mock<ITokenRepository> _mockTokenRepository;
        private Mock<IMailService> _mockMailService;
        private Mock<ISmsService> _mockSmsService;
        private Mock<ILogger<AuthService>> _mockLogger;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<WalletService> _mockWalletService;
        private AuthService _authService;

        [TestInitialize]
        public void Setup()
        {
            // Cấu hình các đối tượng giả lập (Mocks) cho các dependency
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(
                _mockUserManager.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);

            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockTokenRepository = new Mock<ITokenRepository>();
            _mockSmsService = new Mock<ISmsService>();
            _mockMailService = new Mock<IMailService>();
            _mockLogger = new Mock<ILogger<AuthService>>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockWalletService = new Mock<WalletService>(null, null, null, null);

            // Khởi tạo đối tượng AuthService với các mock đã tạo
            _authService = new AuthService(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockHttpContextAccessor.Object,
                _mockTokenRepository.Object,
                _mockMailService.Object,
                _mockSmsService.Object,
                _mockLogger.Object,
                _mockConfiguration.Object,
                _mockWalletService.Object
            );
        }

        // Test case 1: Đăng nhập thành công với thông tin hợp lệ
        [TestMethod]
        public async Task LoginAsync_ValidCredentials_ReturnsSuccess()
        {
            // Arrange
            var user = new ApplicationUser { PhoneNumber = "0123456789", PhoneNumberConfirmed = true };
            var loginDto = new LoginDTO { LoginIdentifier = "0123456789", Password = "ValidPassword123" };
            var tokenDto = new TokenDTO { AccessToken = "valid_jwt_token" };

            // Giả lập UserManager.Users trả về user
            _mockUserManager.Setup(u => u.Users).Returns(new List<ApplicationUser> { user }.AsQueryable());
            // Giả lập PasswordSignInAsync thành công
            _mockSignInManager.Setup(s => s.PasswordSignInAsync(user, loginDto.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            // Giả lập tạo token thành công
            _mockTokenRepository.Setup(t => t.CreateJWTTokenAsync(user, true)).ReturnsAsync(tokenDto);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.IsTrue(result.IsAuthSuccessful);
            Assert.AreEqual("Login successful", result.ErrorMessage);
            Assert.AreEqual(tokenDto.AccessToken, result.Token);
        }

        // Test case 2: Đăng nhập thất bại do số điện thoại không tồn tại
        [TestMethod]
        public async Task LoginAsync_InvalidPhoneNumber_ReturnsFailure()
        {
            // Arrange
            var loginDto = new LoginDTO { LoginIdentifier = "9999999999", Password = "ValidPassword123" };

            // Giả lập UserManager.Users không tìm thấy user
            _mockUserManager.Setup(u => u.Users).Returns(new List<ApplicationUser>().AsQueryable());

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.IsFalse(result.IsAuthSuccessful);
            Assert.AreEqual("Invalid Phone Number", result.ErrorMessage);
        }

        // Test case 3: Đăng nhập thất bại do số điện thoại chưa được xác thực
        [TestMethod]
        public async Task LoginAsync_PhoneNumberNotConfirmed_ReturnsFailure()
        {
            // Arrange
            var user = new ApplicationUser { PhoneNumber = "0123456789", PhoneNumberConfirmed = false };
            var loginDto = new LoginDTO { LoginIdentifier = "0123456789", Password = "ValidPassword123" };

            _mockUserManager.Setup(u => u.Users).Returns(new List<ApplicationUser> { user }.AsQueryable());

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.IsFalse(result.IsAuthSuccessful);
            Assert.AreEqual("Phone number not verified. Please verify your phone number first.", result.ErrorMessage);
        }

        // Test case 4: Đăng nhập thất bại do sai mật khẩu
        [TestMethod]
        public async Task LoginAsync_InvalidPassword_ReturnsFailure()
        {
            // Arrange
            var user = new ApplicationUser { PhoneNumber = "0123456789", PhoneNumberConfirmed = true };
            var loginDto = new LoginDTO { LoginIdentifier = "0123456789", Password = "InvalidPassword" };

            _mockUserManager.Setup(u => u.Users).Returns(new List<ApplicationUser> { user }.AsQueryable());
            // Giả lập PasswordSignInAsync thất bại
            _mockSignInManager.Setup(s => s.PasswordSignInAsync(user, loginDto.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.IsFalse(result.IsAuthSuccessful);
            Assert.AreEqual("Invalid phone number or password.", result.ErrorMessage);
        }
    }
}