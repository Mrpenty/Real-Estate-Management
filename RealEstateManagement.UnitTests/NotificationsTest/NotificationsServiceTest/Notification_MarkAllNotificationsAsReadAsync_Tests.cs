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
        [TestMethod]
        public async Task ReturnsFalse_WhenRepositoryReturnsFalse()
        {
            Repo.Setup(r => r.MarkAllNotificationsAsReadAsync(2)).ReturnsAsync(false);

            var ok = await Svc.MarkAllNotificationsAsReadAsync(2);

            Assert.IsFalse(ok);
            Repo.Verify(r => r.MarkAllNotificationsAsReadAsync(2), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task PropagatesException_WhenRepositoryThrows()
        {
            Repo.Setup(r => r.MarkAllNotificationsAsReadAsync(3))
                .ThrowsAsync(new InvalidOperationException("DB err"));

            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.MarkAllNotificationsAsReadAsync(3));

            Assert.AreEqual("DB err", ex.Message);
            Repo.Verify(r => r.MarkAllNotificationsAsReadAsync(3), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
