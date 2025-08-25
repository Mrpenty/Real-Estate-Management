using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Business.Repositories.Chat.Messages;
using RealEstateManagement.Business.Services.Chat.Messages;
using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.ChatTest.MessageTest
{
    [TestClass]
    public class Message_SendMessageAsync_Tests : MessageTestBase
    {
        [TestMethod]
        public async Task CreatesMessageWithCorrectFields_AndReturnsCreated()
        {
            var dto = new MessageDTO { ConversationId = 10, Content = "hello" };
            var senderId = 7;

            Message captured = null!;
            var created = new Message
            {
                Id = 123,
                ConversationId = 10,
                SenderId = senderId,
                Content = "hello",
                SentAt = DateTime.UtcNow,
                IsRead = false,
                NotificationSent = false
            };

            Repo.Setup(r => r.CreateAsync(It.IsAny<Message>()))
                .Callback<Message>(m => captured = m)
                .ReturnsAsync(created);

            var before = DateTime.UtcNow;
            var result = await Svc.SendMessageAsync(dto, senderId);
            var after = DateTime.UtcNow;

            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);

            Assert.IsNotNull(captured);
            Assert.AreEqual(10, captured.ConversationId);
            Assert.AreEqual(7, captured.SenderId);
            Assert.AreEqual("hello", captured.Content);
            Assert.IsFalse(captured.IsRead);
            Assert.IsFalse(captured.NotificationSent);
            Assert.IsTrue(captured.SentAt >= before && captured.SentAt <= after);

            Repo.Verify(r => r.CreateAsync(It.IsAny<Message>()), Times.Once);
        }

        [TestMethod]
        public async Task PropagatesException_WhenRepositoryThrows()
        {
            var dto = new MessageDTO { ConversationId = 1, Content = "x" };

            Repo.Setup(r => r.CreateAsync(It.IsAny<Message>()))
                .ThrowsAsync(new InvalidOperationException("db error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.SendMessageAsync(dto, 1));
        }

        // Lưu ý: hiện service không check null dto (sẽ NRE nếu truyền null).
        // Nếu muốn, bạn có thể thêm guard trong service. Ở đây mình chỉ minh hoạ:
        [TestMethod]
        public async Task NullDto_CurrentImplementation_ThrowsNullReference()
        {
            await Assert.ThrowsExceptionAsync<NullReferenceException>(
                () => Svc.SendMessageAsync(null!, 1));
        }
    }
}
