using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_MarkAllNotificationsAsReadAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task ReturnsTrue_WhenRepositoryReturnsTrue()
        {
            Repo.Setup(r => r.MarkAllNotificationsAsReadAsync(1)).ReturnsAsync(true);

            var ok = await Svc.MarkAllNotificationsAsReadAsync(1);

            Assert.IsTrue(ok);
            Repo.Verify(r => r.MarkAllNotificationsAsReadAsync(1), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
