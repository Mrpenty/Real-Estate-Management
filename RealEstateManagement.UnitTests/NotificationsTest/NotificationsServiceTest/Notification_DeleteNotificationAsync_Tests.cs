using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_DeleteNotificationAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task ReturnsTrue_WhenRepositoryReturnsTrue()
        {
            Repo.Setup(r => r.DeleteNotificationAsync(10)).ReturnsAsync(true);

            var ok = await Svc.DeleteNotificationAsync(10);

            Assert.IsTrue(ok);
            Repo.Verify(r => r.DeleteNotificationAsync(10), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task ReturnsFalse_WhenRepositoryReturnsFalse()
        {
            Repo.Setup(r => r.DeleteNotificationAsync(99)).ReturnsAsync(false);

            var ok = await Svc.DeleteNotificationAsync(99);

            Assert.IsFalse(ok);
            Repo.Verify(r => r.DeleteNotificationAsync(99), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task PropagatesException_WhenRepositoryThrows()
        {
            Repo.Setup(r => r.DeleteNotificationAsync(1))
                .ThrowsAsync(new InvalidOperationException("DB err"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.DeleteNotificationAsync(1));

            Repo.Verify(r => r.DeleteNotificationAsync(1), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
