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
    public class AdminDashboardService_GetDailyStatsAsync_Tests : AdminDashboardServiceTestBase
    {
        [TestMethod]
        public async Task GetDailyStatsAsync_ReturnsData_WhenRepositorySucceeds()
        {
            // Arrange
            var startDate = new DateTime(2025, 1, 1);
            var endDate = new DateTime(2025, 1, 10);

            var expected = new List<DailyStatsDTO>
            {
                new DailyStatsDTO
                {
                    Date = startDate,
                    NewUsers = 5,
                    NewProperties = 3,
                    NewPosts = 2,
                    Revenue = 100,
                    Views = 50
                }
            };

            Repo.Setup(r => r.GetDailyStatsAsync(startDate, endDate))
                .ReturnsAsync(expected);

            // Act
            var result = await Svc.GetDailyStatsAsync(startDate, endDate);

            // Assert
            Assert.AreEqual(expected.Count, result.Count);
            Assert.AreEqual(expected[0].NewUsers, result[0].NewUsers);
            Assert.AreEqual(expected[0].Revenue, result[0].Revenue);

            // Không log lỗi
            VerifyErrorLogged(Logger, "Error getting daily stats", Times.Never());
            Repo.Verify(r => r.GetDailyStatsAsync(startDate, endDate), Times.Once());
        }

        [TestMethod]
        public async Task GetDailyStatsAsync_LogsErrorAndThrows_WhenRepositoryFails()
        {
            // Arrange
            var startDate = new DateTime(2025, 1, 1);
            var endDate = new DateTime(2025, 1, 10);

            var ex = new Exception("db fail");

            Repo.Setup(r => r.GetDailyStatsAsync(startDate, endDate))
                .ThrowsAsync(ex);

            // Act & Assert
            var thrown = await Assert.ThrowsExceptionAsync<Exception>(() =>
                Svc.GetDailyStatsAsync(startDate, endDate));

            Assert.AreEqual(ex, thrown);

            // Đúng là có log lỗi
            VerifyErrorLogged(Logger, "Error getting daily stats", Times.Once());
            Repo.Verify(r => r.GetDailyStatsAsync(startDate, endDate), Times.Once());
        }
    }
}
