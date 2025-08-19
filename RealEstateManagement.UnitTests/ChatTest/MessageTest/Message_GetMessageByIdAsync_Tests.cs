using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.Messages;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.ChatTest.MessageTest
{
    [TestClass]
    public class Message_GetMessageByIdAsync_Tests : MessageTestBase
    {
        [TestMethod]
        public async Task ReturnsMessage_WhenFound()
        {
            var msg = new Message { Id = 42, Content = "ok" };
            Repo.Setup(r => r.GetByIdAsync(42)).ReturnsAsync(msg);

            var result = await Svc.GetMessageByIdAsync(42);

            Assert.IsNotNull(result);
            Assert.AreEqual(42, result.Id);
            Repo.Verify(r => r.GetByIdAsync(42), Times.Once);
        }

        [TestMethod]
        public async Task ThrowsKeyNotFound_WhenNull()
        {
            Repo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync((Message)null!);

            var ex = await Assert.ThrowsExceptionAsync<KeyNotFoundException>(
                () => Svc.GetMessageByIdAsync(7));

            StringAssert.Contains(ex.Message, "Message not found");
            Repo.Verify(r => r.GetByIdAsync(7), Times.Once);
        }
    }
}
