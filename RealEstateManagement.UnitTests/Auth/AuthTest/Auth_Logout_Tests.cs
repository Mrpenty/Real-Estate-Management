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
        public async Task Logout_Calls_DeleteTokenCookie_And_SignOut()
        {
            // Act
            await Svc.LogoutAsync();

            // Assert
            TokenRepoMock.Verify(t => t.DeleteTokenCookie(HttpContext), Times.Once);
            SignInManagerMock.Verify(s => s.SignOutAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Logout_Still_SignsOut_When_HttpContext_Is_Null()
        {
            // Arrange: giả lập không có HttpContext
            HttpAccessorMock.SetupGet(h => h.HttpContext).Returns((HttpContext)null);

            // Act
            await Svc.LogoutAsync();

            // Assert
            // Xác nhận vẫn cố gắng xóa cookie (tham số có thể null, dùng It.IsAny để không vướng nullable ref)
            TokenRepoMock.Verify(t => t.DeleteTokenCookie(It.IsAny<HttpContext>()), Times.Once);
            // Và luôn gọi SignOut
            SignInManagerMock.Verify(s => s.SignOutAsync(), Times.Once);
        }
    }
}
