using Microsoft.Extensions.Logging;
using Moq;
using RealEstateManagement.Business.DTO.AdminDTO;
using RealEstateManagement.Business.Repositories.Admin;
using RealEstateManagement.Business.Services.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Admin.AdminDashBoardTest
{
    [TestClass]
    public class AdminDashboardService_GetRevenueStatsAsync_Tests : AdminDashboardServiceTestBase
    {
        [TestMethod]
        public async Task GetRevenueStatsAsync_ReturnsData_WhenRepositorySucceeds()
        {
            // Arrange
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 31);
            var expected = new List<RevenueStatsDTO>
            {
                new RevenueStatsDTO { Type = "Ads", Amount = 200, TransactionCount = 3 }
            };

            Repo.Setup(r => r.GetRevenueStatsAsync(start, end)).ReturnsAsync(expected);

            // Act
            var result = await Svc.GetRevenueStatsAsync(start, end);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Ads", result[0].Type);
            Assert.AreEqual(200, result[0].Amount);
            VerifyErrorLogged(Logger, "Error getting revenue stats", Times.Never());
            Repo.Verify(r => r.GetRevenueStatsAsync(start, end), Times.Once());
        }

        [TestMethod]
        public async Task GetRevenueStatsAsync_LogsErrorAndThrows_WhenRepositoryReturnsNull()
        {
            // Arrange
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 31);

            Repo.Setup(r => r.GetRevenueStatsAsync(start, end))
                .ReturnsAsync((List<RevenueStatsDTO>?)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                Svc.GetRevenueStatsAsync(start, end));

            VerifyErrorLogged(Logger, "Error getting revenue stats", Times.Once());
            Repo.Verify(r => r.GetRevenueStatsAsync(start, end), Times.Once());
        }

        [TestMethod]
        public async Task GetRevenueStatsAsync_ReturnsEmptyList_WhenRepositoryReturnsEmpty()
        {
            // Arrange
            var start = new DateTime(2025, 1, 2);
            var end = new DateTime(2025, 1, 31);

            Repo.Setup(r => r.GetRevenueStatsAsync(start, end))
                .ReturnsAsync(new List<RevenueStatsDTO>());

            // Act
            var result = await Svc.GetRevenueStatsAsync(start, end);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            VerifyErrorLogged(Logger, "Error getting revenue stats", Times.Never());
            Repo.Verify(r => r.GetRevenueStatsAsync(start, end), Times.Once());
        }

        [TestMethod]
        public async Task GetRevenueStatsAsync_LogsErrorAndThrows_WhenStartDateAfterEndDate()
        {
            // Arrange
            var start = new DateTime(2025, 2, 1);
            var end = new DateTime(2025, 1, 31);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                Svc.GetRevenueStatsAsync(start, end));

            VerifyErrorLogged(Logger, "Error getting revenue stats", Times.Once());
            Repo.Verify(r => r.GetRevenueStatsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
        }
    }
}
