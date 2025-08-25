using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.Chat.Messages;
using RealEstateManagement.Data.Entity.Messages;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.ChatTest.MessageTest
{
    [TestClass]
    public class Message_GetMessagesByConversationAsync_Tests : MessageTestBase
    {
        [TestMethod]
        public async Task ReturnsEmpty_WhenRepositoryReturnsNull()
        {
            Repo.Setup(r => r.GetByConversationIdAsync(99, 0, 20))
                .Returns(Task.FromResult<IEnumerable<Message>>(null));

            var result = (await Svc.GetMessagesByConversationAsync(99)).ToList();

            Assert.AreEqual(0, result.Count);
            Repo.Verify(r => r.GetByConversationIdAsync(99, 0, 20), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsEmpty_WhenRepositoryReturnsEmptyList()
        {
            Repo.Setup(r => r.GetByConversationIdAsync(99, 0, 20))
                .Returns(Task.FromResult<IEnumerable<Message>>(new List<Message>()));

            var result = (await Svc.GetMessagesByConversationAsync(99)).ToList();

            Assert.AreEqual(0, result.Count);
            Repo.Verify(r => r.GetByConversationIdAsync(99, 0, 20), Times.Once);
        }

        [TestMethod]
        public async Task MapsFields_WhenRepositoryReturnsMessages()
        {
            var msgs = new List<Message>
            {
                new Message
                {
                    Id = 1, ConversationId = 10, SenderId = 7,
                    Sender = new ApplicationUser { UserName = "alice", ProfilePictureUrl = "a.png" },
                    Content = "hi", IsRead = true, SentAt = DateTime.UtcNow.AddMinutes(-1)
                },
                new Message
                {
                    Id = 2, ConversationId = 10, SenderId = 8,
                    Sender = new ApplicationUser { UserName = "bob", ProfilePictureUrl = "b.png" },
                    Content = "yo", IsRead = false, SentAt = DateTime.UtcNow
                }
            };

            Repo.Setup(r => r.GetByConversationIdAsync(10, 5, 10))
                .Returns(Task.FromResult<IEnumerable<Message>>(msgs));

            var result = (await Svc.GetMessagesByConversationAsync(10, skip: 5, take: 10)).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(10, result[0].ConversationId);
            Assert.AreEqual(7, result[0].SenderId);
            Assert.AreEqual("alice", result[0].SenderName);
            Assert.AreEqual("a.png", result[0].SenderAvatar);
            Assert.AreEqual("hi", result[0].Content);
            Assert.IsTrue(result[0].IsRead);

            Repo.Verify(r => r.GetByConversationIdAsync(10, 5, 10), Times.Once);
        }
    }
}
