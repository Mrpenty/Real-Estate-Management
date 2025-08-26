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
    public class Conversation_GetAllByUserIdAsync_Tests : ConversationTestBase
    {
        [TestMethod]
        public async Task WhenRepoReturnsEmpty_ReturnsEmptyList()
        {
            Repo.Setup(r => r.GetAllByUserIdAsync(123)).ReturnsAsync(new List<Conversation>());

            var result = (await Svc.GetAllByUserIdAsync(123)).ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task WhenRepoReturnsNull_TreatAsEmpty()
        {
            Repo.Setup(r => r.GetAllByUserIdAsync(123)).ReturnsAsync((IEnumerable<Conversation>)null!);

            var result = (await Svc.GetAllByUserIdAsync(123)).ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task AsLandlord_FillsRenterNameOnly()
        {
            var convs = new List<Conversation>
            {
                new Conversation
                {
                    Id = 1, RenterId = 10, LandlordId = 99,
                    LastMessage = "hi", LastSentAt = DateTime.UtcNow,
                    Renter = new ApplicationUser { Name = "Renter A" },
                    Landlord = new ApplicationUser { Name = "Landlord Z" }
                }
            };
            Repo.Setup(r => r.GetAllByUserIdAsync(99)).ReturnsAsync(convs);

            var dtos = (await Svc.GetAllByUserIdAsync(99)).ToList();

            Assert.AreEqual(1, dtos.Count);
            Assert.AreEqual("Renter A", dtos[0].RenterName);
            Assert.IsNull(dtos[0].LandlordName);
            Assert.AreEqual("hi", dtos[0].LastMessage);
        }

        [TestMethod]
        public async Task AsRenter_FillsLandlordNameOnly()
        {
            var convs = new List<Conversation>
            {
                new Conversation
                {
                    Id = 2, RenterId = 55, LandlordId = 77,
                    LastMessage = "hello", LastSentAt = DateTime.UtcNow,
                    Renter = new ApplicationUser { Name = "Renter B" },
                    Landlord = new ApplicationUser { Name = "Landlord C" }
                }
            };
            Repo.Setup(r => r.GetAllByUserIdAsync(55)).ReturnsAsync(convs);

            var dtos = (await Svc.GetAllByUserIdAsync(55)).ToList();

            Assert.AreEqual(1, dtos.Count);
            Assert.AreEqual("Landlord C", dtos[0].LandlordName);
            Assert.IsNull(dtos[0].RenterName);
        }

        [TestMethod]
        public async Task WhenNeitherMatches_FillsNoNames()
        {
            var convs = new List<Conversation>
            {
                new Conversation
                {
                    Id = 3, RenterId = 1, LandlordId = 2,
                    Renter = new ApplicationUser { Name = "A" },
                    Landlord = new ApplicationUser { Name = "B" }
                }
            };
            Repo.Setup(r => r.GetAllByUserIdAsync(999)).ReturnsAsync(convs);

            var dtos = (await Svc.GetAllByUserIdAsync(999)).ToList();

            Assert.AreEqual(1, dtos.Count);
            Assert.IsNull(dtos[0].RenterName);
            Assert.IsNull(dtos[0].LandlordName);
        }
    }
}
