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
            var res = await Svc.LoginAsync(new LoginDTO { LoginIdentifier = "0123", Password = "x" });
            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "Invalid Phone Number");
        }

        [TestMethod]
        public async Task Login_PhoneNotVerified_ReturnsMustVerify()
        {
            var u = new ApplicationUser { PhoneNumber = "0123", PhoneNumberConfirmed = false };
            SetUsers(u);

            var res = await Svc.LoginAsync(new LoginDTO { LoginIdentifier = "0123", Password = "x" });
            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "not verified");
        }

        [TestMethod]
        public async Task Login_InvalidPassword_ReturnsInvalid()
        {
            var u = new ApplicationUser { PhoneNumber = "0123", PhoneNumberConfirmed = true };
            SetUsers(u);
            SetupPasswordSignIn(u, "bad", SignInResult.Failed);

            var res = await Svc.LoginAsync(new LoginDTO { LoginIdentifier = "0123", Password = "bad" });
            Assert.IsFalse(res.IsAuthSuccessful);
            StringAssert.Contains(res.ErrorMessage, "Invalid phone number or password");
        }

        [TestMethod]
        public async Task Login_Success_ReturnsToken_AndSetsCookie()
        {
            var u = new ApplicationUser { PhoneNumber = "0123", PhoneNumberConfirmed = true };
            SetUsers(u);
            SetupPasswordSignIn(u, "ok", SignInResult.Success);

            var token = new TokenDTO { AccessToken = "jwt" };
            TokenRepoMock.Setup(t => t.CreateJWTTokenAsync(u, true)).ReturnsAsync(token);

            var res = await Svc.LoginAsync(new LoginDTO { LoginIdentifier = "0123", Password = "ok" });

            Assert.IsTrue(res.IsAuthSuccessful);
            Assert.AreEqual("jwt", res.Token);
            TokenRepoMock.Verify(t => t.SetTokenCookie(token, HttpContext), Times.Once);
        }
    }
}
