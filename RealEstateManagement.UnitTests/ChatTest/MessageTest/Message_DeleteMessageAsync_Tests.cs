using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.Messages;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.ChatTest.MessageTest
{
    [TestClass]
    public class Message_DeleteMessageAsync_Tests : MessageTestBase
    {
        [TestMethod]
        public async Task Deletes_WhenFound()
        {
            var msg = new Message { Id = 5, Content = "bye" };
            Repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(msg);
            Repo.Setup(r => r.DeleteAsync(msg)).Returns(Task.CompletedTask);

            await Svc.DeleteMessageAsync(5);

            Repo.Verify(r => r.GetByIdAsync(5), Times.Once);
            Repo.Verify(r => r.DeleteAsync(msg), Times.Once);
        }

        [TestMethod]
        public async Task ThrowsKeyNotFound_WhenNotFound()
        {
            Repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Message)null!);

            var ex = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
                () => Svc.DeleteMessageAsync(99));

            StringAssert.Contains(ex.Message, "Message not found");
            Repo.Verify(r => r.GetByIdAsync(99), Times.Once);
            Repo.Verify(r => r.DeleteAsync(It.IsAny<Message>()), Times.Never);
        }
        [TestMethod]
        public async Task ThrowsArgumentException_When_Id_Is_NonPositive()
        {
            // Act + Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => Svc.DeleteMessageAsync(0));
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => Svc.DeleteMessageAsync(-1));

            // Không được gọi repo khi id không hợp lệ
            Repo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
            Repo.Verify(r => r.DeleteAsync(It.IsAny<Message>()), Times.Never);
        }

        [TestMethod]
        public async Task PropagatesException_When_RepositoryDeleteFails()
        {
            // Arrange
            var msg = new Message { Id = 7, Content = "oops" };
            Repo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(msg);
            Repo.Setup(r => r.DeleteAsync(msg)).ThrowsAsync(new InvalidOperationException("DB fail"));

            // Act + Assert
            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => Svc.DeleteMessageAsync(7));
            StringAssert.Contains(ex.Message, "DB fail");

            Repo.Verify(r => r.GetByIdAsync(7), Times.Once);
            Repo.Verify(r => r.DeleteAsync(msg), Times.Once);
        }
    }
}
