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
        [TestMethod]
        public async Task ReturnsZero_WhenRepositoryReturnsZero()
        {
            Repo.Setup(r => r.GetUnreadNotificationCountAsync(2)).ReturnsAsync(0);

            var count = await Svc.GetUnreadNotificationCountAsync(2);

            Assert.AreEqual(0, count);
            Repo.Verify(r => r.GetUnreadNotificationCountAsync(2), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task PropagatesException_WhenRepositoryThrows()
        {
            Repo.Setup(r => r.GetUnreadNotificationCountAsync(3))
                .ThrowsAsync(new InvalidOperationException("DB err"));

            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.GetUnreadNotificationCountAsync(3));

            Assert.AreEqual("DB err", ex.Message);
            Repo.Verify(r => r.GetUnreadNotificationCountAsync(3), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

    }
}
