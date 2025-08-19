using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.ChatTest.MessageTest
{
    [TestClass]
    public class Message_UpdateMessageAsync_Tests : MessageTestBase
    {
        [TestMethod]
        public async Task CallsRepositoryUpdate_WhenValid()
        {
            var msg = new Message { Id = 7, Content = "edit" };
            Repo.Setup(r => r.UpdateAsync(msg)).Returns(Task.CompletedTask);

            await Svc.UpdateMessageAsync(msg);

            Repo.Verify(r => r.UpdateAsync(msg), Times.Once);
        }

        [TestMethod]
        public async Task ThrowsArgumentNull_WhenNullMessage()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                () => Svc.UpdateMessageAsync(null!));

            Repo.Verify(r => r.UpdateAsync(It.IsAny<Message>()), Times.Never);
        }
    }
}
