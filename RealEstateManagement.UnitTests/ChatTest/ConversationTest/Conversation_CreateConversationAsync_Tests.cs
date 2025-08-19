using Moq;
using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.ChatTest.ConversationTest
{
    [TestClass]
    public class Conversation_CreateConversationAsync_Tests : ConversationTestBase
    {
        [TestMethod]
        public async Task WhenDtoIsNull_ThrowsArgumentNullException_AndNoRepoCalls()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => Svc.CreateConversationAsync(null!));

            Repo.Verify(r => r.GetByUsersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            Repo.Verify(r => r.CreateAsync(It.IsAny<Conversation>()), Times.Never);
        }

        [TestMethod]
        public async Task WhenRenterEqualsLandlord_ThrowsArgumentException_AndNoRepoCalls()
        {
            var dto = new CreateConversationDTO { RenterId = 7, LandlordId = 7, PropertyId = 30 };

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => Svc.CreateConversationAsync(dto));

            Repo.Verify(r => r.GetByUsersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            Repo.Verify(r => r.CreateAsync(It.IsAny<Conversation>()), Times.Never);
        }

        [TestMethod]
        public async Task WhenExisting_ReturnsExisting_AndNotCreate()
        {
            var dto = new CreateConversationDTO { RenterId = 1, LandlordId = 2, PropertyId = 3 };
            var existing = new Conversation { Id = 99, RenterId = 1, LandlordId = 2, PropertyId = 3 };

            Repo.Setup(r => r.GetByUsersAsync(1, 2, 3)).ReturnsAsync(existing);

            var result = await Svc.CreateConversationAsync(dto);

            Assert.IsNotNull(result);
            Assert.AreEqual(99, result.Id);
            Repo.Verify(r => r.CreateAsync(It.IsAny<Conversation>()), Times.Never);
        }

        [TestMethod]
        public async Task WhenNotExisting_CreatesWithCorrectFields_AndReturnsCreated()
        {
            var dto = new CreateConversationDTO { RenterId = 10, LandlordId = 20, PropertyId = 30 };
            Repo.Setup(r => r.GetByUsersAsync(10, 20, 30)).ReturnsAsync((Conversation)null);

            Conversation captured = null!;
            var created = new Conversation { Id = 123, RenterId = 10, LandlordId = 20, PropertyId = 30, CreatedAt = DateTime.UtcNow };

            Repo.Setup(r => r.CreateAsync(It.IsAny<Conversation>()))
                .Callback<Conversation>(c => captured = c)
                .ReturnsAsync(created);

            var before = DateTime.UtcNow;
            var result = await Svc.CreateConversationAsync(dto);
            var after = DateTime.UtcNow;

            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);
            Assert.IsNotNull(captured);
            Assert.AreEqual(10, captured.RenterId);
            Assert.AreEqual(20, captured.LandlordId);
            Assert.AreEqual(30, captured.PropertyId);
            Assert.IsTrue(captured.CreatedAt >= before && captured.CreatedAt <= after);

            Repo.Verify(r => r.CreateAsync(It.IsAny<Conversation>()), Times.Once);
        }

        [TestMethod]
        public async Task WhenRepositoryThrows_PropagatesException()
        {
            var dto = new CreateConversationDTO { RenterId = 1, LandlordId = 2, PropertyId = 3 };
            Repo.Setup(r => r.GetByUsersAsync(1, 2, 3)).ReturnsAsync((Conversation)null);
            Repo.Setup(r => r.CreateAsync(It.IsAny<Conversation>()))
                .ThrowsAsync(new InvalidOperationException("db error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => Svc.CreateConversationAsync(dto));
        }

    }
}
