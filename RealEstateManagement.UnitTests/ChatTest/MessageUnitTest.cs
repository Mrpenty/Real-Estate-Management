using Moq;
using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Business.Repositories.Chat.Messages;
using RealEstateManagement.Business.Services.Chat.Messages;
using RealEstateManagement.Data.Entity.Messages;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.ChatTest
{
    [TestClass]
    public class MessageUnitTest
    {
            
        private Mock<IMessageRepository> _mockRepo = null!;
        private MessageService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IMessageRepository>();
            _service = new MessageService(_mockRepo.Object);
        }

        // ----------------------------
        // SendMessageAsync
        // ----------------------------

        [TestMethod]
        public async Task SendMessageAsync_ValidInput_CreatesMessage_WithCorrectFields()
        {
            var dto = new MessageDTO { ConversationId = 5, Content = "Hello world" };
            var senderId = 42;

            Message captured = null!;
            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Message>()))
                     .Callback<Message>(m => captured = m)
                     .ReturnsAsync((Message m) => { m.Id = 100; return m; });

            var before = DateTime.UtcNow;
            var result = await _service.SendMessageAsync(dto, senderId);
            var after = DateTime.UtcNow;

            Assert.IsNotNull(result);
            Assert.AreEqual(100, result.Id);

            Assert.IsNotNull(captured);
            Assert.AreEqual(dto.ConversationId, captured.ConversationId);
            Assert.AreEqual(senderId, captured.SenderId);
            Assert.AreEqual(dto.Content, captured.Content);
            Assert.IsFalse(captured.IsRead);
            Assert.IsFalse(captured.NotificationSent);
            Assert.IsTrue(captured.SentAt >= before && captured.SentAt <= after, "SentAt phải set theo UTC tại thời điểm gửi");

            _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Message>()), Times.Once);
        }

        [TestMethod]
        public async Task SendMessageAsync_RepositoryThrows_PropagatesException()
        {
            _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Message>()))
                     .ThrowsAsync(new InvalidOperationException("DB error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _service.SendMessageAsync(new MessageDTO { ConversationId = 1, Content = "x" }, 7));
        }

        // ----------------------------
        // GetMessagesByConversationAsync
        // ----------------------------

        [TestMethod]
        public async Task GetMessagesByConversationAsync_ReturnsMappedDtos_And_RespectsSkipTake()
        {
            var conversationId = 10;
            var skip = 5;
            var take = 3;

            var messages = new List<Message>
            {
                new Message
                {
                    Id = 1,
                    ConversationId = conversationId,
                    SenderId = 7,
                    Sender = new ApplicationUser { UserName = "alice", ProfilePictureUrl = "a.jpg" },
                    Content = "Hi",
                    IsRead = true,
                    SentAt = DateTime.UtcNow.AddMinutes(-10)
                },
                new Message
                {
                    Id = 2,
                    ConversationId = conversationId,
                    SenderId = 8,
                    Sender = new ApplicationUser { UserName = "bob", ProfilePictureUrl = "b.jpg" },
                    Content = "Hello",
                    IsRead = false,
                    SentAt = DateTime.UtcNow.AddMinutes(-9)
                }
            };

            _mockRepo.Setup(r => r.GetByConversationIdAsync(conversationId, skip, take))
                     .ReturnsAsync(messages);

            var dtos = (await _service.GetMessagesByConversationAsync(conversationId, skip, take)).ToList();

            Assert.AreEqual(2, dtos.Count);

            var d1 = dtos[0];
            Assert.AreEqual(1, d1.Id);
            Assert.AreEqual(conversationId, d1.ConversationId);
            Assert.AreEqual(7, d1.SenderId);
            Assert.AreEqual("alice", d1.SenderName);
            Assert.AreEqual("a.jpg", d1.SenderAvatar);
            Assert.AreEqual("Hi", d1.Content);
            Assert.IsTrue(d1.IsRead);

            var d2 = dtos[1];
            Assert.AreEqual(2, d2.Id);
            Assert.AreEqual("bob", d2.SenderName);
            Assert.AreEqual("Hello", d2.Content);
            Assert.IsFalse(d2.IsRead);

            _mockRepo.Verify(r => r.GetByConversationIdAsync(conversationId, skip, take), Times.Once);
        }

        [TestMethod]
        public async Task GetMessagesByConversationAsync_EmptyList_ReturnsEmpty()
        {
            _mockRepo.Setup(r => r.GetByConversationIdAsync(99, 0, 20))
                     .ReturnsAsync(new List<Message>());

            var dtos = await _service.GetMessagesByConversationAsync(99);

            Assert.IsNotNull(dtos);
            Assert.AreEqual(0, dtos.Count());
        }

        [TestMethod]
        public async Task GetMessagesByConversationAsync_NullFromRepo_ReturnsEmpty()
        {
            _mockRepo.Setup(r => r.GetByConversationIdAsync(88, 0, 20))
                     .ReturnsAsync((IEnumerable<Message>)null);

            var dtos = await _service.GetMessagesByConversationAsync(88);

            Assert.IsNotNull(dtos);
            Assert.AreEqual(0, dtos.Count());
        }

        // ----------------------------
        // GetMessageByIdAsync
        // ----------------------------

        [TestMethod]
        public async Task GetMessageByIdAsync_Found_ReturnsEntity()
        {
            var msg = new Message { Id = 123, Content = "hello" };

            _mockRepo.Setup(r => r.GetByIdAsync(123)).ReturnsAsync(msg);

            var result = await _service.GetMessageByIdAsync(123);

            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);
            Assert.AreEqual("hello", result.Content);
        }

        [TestMethod]
        public async Task GetMessageByIdAsync_NotFound_ThrowsKeyNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(404)).ReturnsAsync((Message)null);

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
                _service.GetMessageByIdAsync(404));
        }

        // ----------------------------
        // DeleteMessageAsync
        // ----------------------------

        [TestMethod]
        public async Task DeleteMessageAsync_Found_CallsDeleteOnce()
        {
            var msg = new Message { Id = 9 };

            _mockRepo.Setup(r => r.GetByIdAsync(9)).ReturnsAsync(msg);
            _mockRepo.Setup(r => r.DeleteAsync(msg)).Returns(Task.CompletedTask);
            // Nếu DeleteAsync trả về Task<Message>:
            // _mockRepo.Setup(r => r.DeleteAsync(msg)).ReturnsAsync(msg);

            await _service.DeleteMessageAsync(9);

            _mockRepo.Verify(r => r.DeleteAsync(msg), Times.Once);
        }

        [TestMethod]
        public async Task DeleteMessageAsync_NotFound_ThrowsKeyNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Message)null);

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(() =>
                _service.DeleteMessageAsync(999));
        }

        // ----------------------------
        // UpdateMessageAsync
        // ----------------------------

        [TestMethod]
        public async Task UpdateMessageAsync_Null_ThrowsArgumentNull()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                _service.UpdateMessageAsync(null));
        }

        [TestMethod]
        public async Task UpdateMessageAsync_Valid_CallsRepoUpdateOnce()
        {
            var msg = new Message { Id = 3, Content = "edited" };

            _mockRepo.Setup(r => r.UpdateAsync(msg)).Returns(Task.CompletedTask);
            // Nếu UpdateAsync trả về Task<Message>:
            // _mockRepo.Setup(r => r.UpdateAsync(msg)).ReturnsAsync(msg);

            await _service.UpdateMessageAsync(msg);

            _mockRepo.Verify(r => r.UpdateAsync(msg), Times.Once);
        }
    }
}

