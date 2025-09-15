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
        public async Task GetDailyStatsAsync_LogsErrorAndThrows_WhenUnauthorized()
        {
            var startDate = new DateTime(2025, 1, 1);
            var endDate = new DateTime(2025, 1, 10);

            var ex = new UnauthorizedAccessException("no permission");

            Repo.Setup(r => r.GetDailyStatsAsync(startDate, endDate))
                .ThrowsAsync(ex);

            var thrown = await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(() =>
                Svc.GetDailyStatsAsync(startDate, endDate));

            Assert.AreEqual(ex, thrown);

            VerifyErrorLogged(Logger, "Error getting daily stats", Times.Once());
            Repo.Verify(r => r.GetDailyStatsAsync(startDate, endDate), Times.Once());
        }
        [TestMethod]
        public async Task GetDailyStatsAsync_LogsErrorAndThrows_WhenEndDateBeforeStartDate()
        {
            // Arrange
            var startDate = new DateTime(2025, 1, 10);
            var endDate = new DateTime(2025, 1, 1);

            // Act & Assert
            var ex = await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                Svc.GetDailyStatsAsync(startDate, endDate));

            StringAssert.Contains(ex.Message, "endDate must not be earlier than startDate");

            // Có log lỗi
            VerifyErrorLogged(Logger, "Error getting daily stats", Times.Once());

            // Repo KHÔNG được gọi
            Repo.Verify(r => r.GetDailyStatsAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Never());
        }
    }
}
