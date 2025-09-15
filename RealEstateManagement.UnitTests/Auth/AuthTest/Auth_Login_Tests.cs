using Moq;
using RealEstateManagement.Business.DTO.AuthDTO;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace RealEstateManagement.UnitTests.Auth.AuthTest
{
    [TestClass]
    public class Auth_Login_Tests : AuthTestBase
    {
        [TestMethod]
        public async Task Login_UserNotFound_ReturnsInvalidPhone()
        {
            SetUsers(); // empty
            var res = await Svc.LoginAsync(new LoginDTO { LoginIdentifier = "0123456789", Password = "correct@123" });
            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "Invalid Phone Number");
        }
        [TestMethod]
        public async Task Login_UserNotFound_ReturnsInvalidPhone_1()
        {
            SetUsers(); // empty
            var res = await Svc.LoginAsync(new LoginDTO { LoginIdentifier = " ", Password = "correct@123" });
            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "Invalid Phone Number");
        }
        [TestMethod]
        public async Task Login_PhoneNotVerified_ReturnsMustVerify()
        {
            var u = new ApplicationUser { PhoneNumber = "0123456781", PhoneNumberConfirmed = false };
            SetUsers(u);

            var res = await Svc.LoginAsync(new LoginDTO { LoginIdentifier = "0123456781", Password = "correct@123" });
            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "not verified");
        }

        [TestMethod]
        public async Task Login_InvalidPassword_ReturnsInvalid()
        {
            var u = new ApplicationUser { PhoneNumber = "0123456782", PhoneNumberConfirmed = true };
            SetUsers(u);

            // Mặc định: tất cả password sai → Fail
            SignInManagerMock
                .Setup(s => s.PasswordSignInAsync(u, It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Failed);

            // Ngoại lệ: nếu password đúng thì Success
            SignInManagerMock
                .Setup(s => s.PasswordSignInAsync(u, "correct@123", It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            var res = await Svc.LoginAsync(new LoginDTO { LoginIdentifier = "0123456782", Password = "wrong123" });
            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "Invalid phone number or password");
        }
        [TestMethod]
        public async Task Login_WithCorrectPhoneAndPassword_ReturnsSuccess()
        {
            // Arrange: tạo user có PhoneNumber trùng "0987654321"
            var u = new ApplicationUser
            {
                PhoneNumber = "0987654321",
                PhoneNumberConfirmed = true,
                IsActive = true
            };
            SetUsers(u);

            // Giả lập PasswordSignIn thành công
            SetupPasswordSignIn(u, "mypassword", SignInResult.Success);

            // Giả lập token trả về
            var token = new TokenDTO { AccessToken = "jwt-token-123" };
            TokenRepoMock.Setup(t => t.CreateJWTTokenAsync(u, true)).ReturnsAsync(token);

            // Act: login với đúng số điện thoại và password
            var res = await Svc.LoginAsync(new LoginDTO
            {
                LoginIdentifier = "0987654321",
                Password = "mypassword"
            });

            // Assert
            Assert.IsTrue(res.IsAuthSuccessful);
            Assert.AreEqual("jwt-token-123", res.Token);
            Assert.AreEqual("Login successful", res.ErrorMessage);

            // Verify dependency calls
            SignInManagerMock.Verify(s => s.PasswordSignInAsync(u, "mypassword", false, false), Times.Once);
            TokenRepoMock.Verify(t => t.CreateJWTTokenAsync(u, true), Times.Once);
            TokenRepoMock.Verify(t => t.SetTokenCookie(token, HttpContext), Times.Once);
        }
        [TestMethod]
        public async Task Login_UserIsBanned_ReturnsBannedMessage()
        {
            // Arrange: tạo user có số điện thoại nhưng bị ban
            var u = new ApplicationUser
            {
                PhoneNumber = "0123456783",
                PhoneNumberConfirmed = true,
                IsActive = false // bị ban
            };
            SetUsers(u);

            // Act: gọi LoginAsync
            var res = await Svc.LoginAsync(new LoginDTO
            {
                LoginIdentifier = "0123456783",
                Password = "mypassword"
            });

            // Assert
            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "ban");
        }

    }
}
