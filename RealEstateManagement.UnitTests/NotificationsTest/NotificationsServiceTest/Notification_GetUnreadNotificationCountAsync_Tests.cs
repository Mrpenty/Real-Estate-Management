using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_GetUnreadNotificationCountAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task ReturnsCount_FromRepository()
        {
            Repo.Setup(r => r.GetUnreadNotificationCountAsync(1)).ReturnsAsync(5);

            var count = await Svc.GetUnreadNotificationCountAsync(1);

            Assert.AreEqual(5, count);
            Repo.Verify(r => r.GetUnreadNotificationCountAsync(1), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
