using Moq;
using RealEstateManagement.Business.DTO.AdminDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Admin.AdminDashBoardTest
{
    [TestClass]
    public class AdminDashboardService_GetUserStatsAsync_Tests : AdminDashboardServiceTestBase
    {
        [TestMethod]
        public async Task GetUserStatsAsync_ReturnsData_WhenRepositorySucceeds()
        {
            // Arrange
            var expected = new List<UserStatsDTO>
            {
                new UserStatsDTO { Role = "Admin", Count = 2, ActiveCount = 2, VerifiedCount = 1 }
            };
            Repo.Setup(r => r.GetUserStatsAsync()).ReturnsAsync(expected);

            // Act
            var result = await Svc.GetUserStatsAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Admin", result[0].Role);
            VerifyErrorLogged(Logger, "Error getting user stats", Times.Never());
            Repo.Verify(r => r.GetUserStatsAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetUserStatsAsync_LogsErrorAndThrows_WhenRepositoryFails()
        {
            // Arrange
            var ex = new InvalidOperationException("db timeout");
            Repo.Setup(r => r.GetUserStatsAsync()).ThrowsAsync(ex);

            // Act & Assert
            var thrown = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => Svc.GetUserStatsAsync());
            Assert.AreSame(ex, thrown);
            VerifyErrorLogged(Logger, "Error getting user stats", Times.Once());
            Repo.Verify(r => r.GetUserStatsAsync(), Times.Once());
        }
    }
}
