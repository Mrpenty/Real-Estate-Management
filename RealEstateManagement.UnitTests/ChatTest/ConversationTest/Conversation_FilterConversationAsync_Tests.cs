using Moq;
using RealEstateManagement.Data.Entity.Messages;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.ChatTest.ConversationTest
{
    [TestClass]
    public class Conversation_FilterConversationAsync_Tests : ConversationTestBase
    {
        [TestMethod]
        public async Task MapsBasicFields_AndPassesParameters()
        {
            var userId = 5;
            var term = "rent";
            var skip = 10;
            var take = 5;

            var convs = new List<Conversation>
            {
                new Conversation
                {
                    Id = 100, PropertyId = 200, RenterId = 5, LandlordId = 7,
                    Renter = new ApplicationUser { Name = "Renter X" },
                    Landlord = new ApplicationUser { Name = "Land Y" }
                }
            };

            Repo.Setup(r => r.FilterConversationAsync(userId, term, skip, take)).ReturnsAsync(convs);

            var result = (await Svc.FilterConversationAsync(userId, term, skip, take)).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(100, result[0].Id);
            Assert.AreEqual(200, result[0].PropertyId);
            Assert.AreEqual("Renter X", result[0].RenterName);
            Assert.AreEqual("Land Y", result[0].LandlordName);

            Repo.Verify(r => r.FilterConversationAsync(userId, term, skip, take), Times.Once);
        }

        [TestMethod]
        public async Task WhenRepoReturnsEmpty_ReturnsEmptyList()
        {
            Repo.Setup(r => r.FilterConversationAsync(1, "x", 0, 10))
                .ReturnsAsync(new List<Conversation>());

            var result = (await Svc.FilterConversationAsync(1, "x", 0, 10)).ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task WhenRepoThrows_PropagatesException()
        {
            Repo.Setup(r => r.FilterConversationAsync(1, "x", 0, 10))
                .ThrowsAsync(new InvalidOperationException("db err"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.FilterConversationAsync(1, "x", 0, 10));
        }
    }
}
