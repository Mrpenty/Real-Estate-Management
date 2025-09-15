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
    public class AdminDashboardService_GetMonthlyStatsAsync_Tests : AdminDashboardServiceTestBase
    {
        [TestMethod]
        public async Task GetMonthlyStatsAsync_ReturnsData_WhenRepositorySucceeds()
        {
            // Arrange
            var year = 2025;
            var expected = new List<MonthlyStatsDTO>
            {
                new MonthlyStatsDTO { Year = 2025, Month = 1, NewUsers = 10, Revenue = 100, Views = 1000 }
            };
            Repo.Setup(r => r.GetMonthlyStatsAsync(year)).ReturnsAsync(expected);

            // Act
            var result = await Svc.GetMonthlyStatsAsync(year);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2025, result[0].Year);
            Assert.AreEqual(1, result[0].Month);
            VerifyErrorLogged(Logger, "Error getting monthly stats", Times.Never());
            Repo.Verify(r => r.GetMonthlyStatsAsync(year), Times.Once());
        }

        [TestMethod]
        public async Task GetMonthlyStatsAsync_LogsErrorAndThrows_WhenRepositoryReturnsNull()
        {
            // Arrange
            var year = 2025;
            Repo.Setup(r => r.GetMonthlyStatsAsync(year)).ReturnsAsync((List<MonthlyStatsDTO>?)null);

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                Svc.GetMonthlyStatsAsync(year));

            StringAssert.Contains(ex.Message, "Repository returned null");

            VerifyErrorLogged(Logger, "Error getting monthly stats", Times.Exactly(2));

            Repo.Verify(r => r.GetMonthlyStatsAsync(year), Times.Once());
        }
    }
}
