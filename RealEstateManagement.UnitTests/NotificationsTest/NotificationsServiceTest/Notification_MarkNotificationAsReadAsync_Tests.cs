using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_MarkNotificationAsReadAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task ReturnsTrue_WhenRepositoryReturnsTrue()
        {
            Repo.Setup(r => r.MarkNotificationAsReadAsync(7, 1)).ReturnsAsync(true);

            var ok = await Svc.MarkNotificationAsReadAsync(7, 1);

            Assert.IsTrue(ok);
            Repo.Verify(r => r.MarkNotificationAsReadAsync(7, 1), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task ReturnsFalse_WhenRepositoryReturnsFalse()
        {
            Repo.Setup(r => r.MarkNotificationAsReadAsync(8, 1)).ReturnsAsync(false);

            var ok = await Svc.MarkNotificationAsReadAsync(8, 1);

            Assert.IsFalse(ok);
            Repo.Verify(r => r.MarkNotificationAsReadAsync(8, 1), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
        [TestMethod]
        public async Task PropagatesException_WhenRepositoryThrows()
        {
            Repo.Setup(r => r.MarkNotificationAsReadAsync(9, 2))
                .ThrowsAsync(new InvalidOperationException("DB err"));

            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.MarkNotificationAsReadAsync(9, 2));

            Assert.AreEqual("DB err", ex.Message);
            Repo.Verify(r => r.MarkNotificationAsReadAsync(9, 2), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
