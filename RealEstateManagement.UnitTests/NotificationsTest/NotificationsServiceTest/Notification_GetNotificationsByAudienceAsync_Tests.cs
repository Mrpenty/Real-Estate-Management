using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.Notification;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.NotificationsTest.NotificationsServiceTest
{
    [TestClass]
    public class Notification_GetNotificationsByAudienceAsync_Tests : NotificationsServiceTestBase
    {
        [TestMethod]
        public async Task FiltersByAudience_AndMaps()
        {
            var list = new List<Notification>
            {
                new Notification { Id=10, Title="R1", Audience="renters" }
            };
            Repo.Setup(r => r.GetNotificationsByAudienceAsync("renters"))
                .ReturnsAsync(list);

            var dtos = (await Svc.GetNotificationsByAudienceAsync("renters")).ToList();

            Assert.AreEqual(1, dtos.Count);
            Assert.AreEqual(10, dtos[0].Id);
            Assert.AreEqual("renters", dtos[0].Audience);

            Repo.Verify(r => r.GetNotificationsByAudienceAsync("renters"), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
        [TestMethod]
        public async Task ReturnsEmpty_WhenNoNotificationsFound()
        {
            // Params
            var audience = "landlords";

            // Arrange
            Repo.Setup(r => r.GetNotificationsByAudienceAsync(audience))
                .ReturnsAsync(new List<Notification>());

            // Act
            var dtos = (await Svc.GetNotificationsByAudienceAsync(audience)).ToList();

            // Return
            Assert.IsNotNull(dtos);
            Assert.AreEqual(0, dtos.Count);

            // Verify
            Repo.Verify(r => r.GetNotificationsByAudienceAsync(audience), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task PropagatesException_WhenRepositoryThrows()
        {
            // Params
            var audience = "admins";

            // Arrange
            Repo.Setup(r => r.GetNotificationsByAudienceAsync(audience))
                .ThrowsAsync(new InvalidOperationException("DB error")); // LogMessage / Exception message

            // Act + Assert (Exception + LogMessage)
            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.GetNotificationsByAudienceAsync(audience));

            Assert.AreEqual("DB error", ex.Message);

            // Verify
            Repo.Verify(r => r.GetNotificationsByAudienceAsync(audience), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

    }
}
