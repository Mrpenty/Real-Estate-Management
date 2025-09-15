using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Auth.AuthTest
{
    [TestClass]
    public class Auth_Logout_Tests : AuthTestBase
    {
        [TestMethod]
        public async Task Logout_ClearsTokenCookie_AndSignsOut()
        {
            // Act
            await Svc.LogoutAsync();

            // Assert: xoá cookie và gọi SignOut
            TokenRepoMock.Verify(t => t.DeleteTokenCookie(HttpContext), Times.Once);
            SignInManagerMock.Verify(s => s.SignOutAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Logout_PropagatesException_WhenSignOutFails()
        {
            // Arrange
            SignInManagerMock
                .Setup(s => s.SignOutAsync())
                .ThrowsAsync(new InvalidOperationException("failed"));

            // Act + Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => Svc.LogoutAsync());

            // Cookie delete vẫn được gọi trước khi exception (tuỳ implement)
            TokenRepoMock.Verify(t => t.DeleteTokenCookie(HttpContext), Times.Once);
        }
    }
}
