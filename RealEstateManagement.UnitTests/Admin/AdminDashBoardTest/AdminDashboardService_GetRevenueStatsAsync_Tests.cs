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
        public async Task GetRevenueStatsAsync_LogsErrorAndThrows_WhenRepositoryFails()
        {
            // Arrange
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 31);
            var ex = new Exception("db fail");
            Repo.Setup(r => r.GetRevenueStatsAsync(start, end)).ThrowsAsync(ex);

            // Act & Assert
            var thrown = await Assert.ThrowsExceptionAsync<Exception>(() => Svc.GetRevenueStatsAsync(start, end));
            Assert.AreSame(ex, thrown);
            VerifyErrorLogged(Logger, "Error getting revenue stats", Times.Once());
            Repo.Verify(r => r.GetRevenueStatsAsync(start, end), Times.Once());
        }
        [TestMethod]
        public async Task GetDailyStatsAsync_ShouldLogError_WhenRepositoryThrows()
        {
            Repo.Setup(r => r.GetDailyStatsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("DB failed"));

            await Assert.ThrowsExceptionAsync<Exception>(() =>
                Svc.GetDailyStatsAsync(DateTime.Now, DateTime.Now));

            VerifyErrorLogged(Logger, "Error getting daily stats", Times.Once()); // ✅ Có log lỗi
        }
    }
}
