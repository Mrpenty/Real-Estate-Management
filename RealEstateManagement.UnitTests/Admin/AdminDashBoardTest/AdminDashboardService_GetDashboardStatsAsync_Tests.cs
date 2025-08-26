using Microsoft.Extensions.Logging;
using Moq;
using RealEstateManagement.Business.DTO.AdminDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RealEstateManagement.UnitTests.Admin.AdminDashBoardTest
{
    [TestClass]
    public class AdminDashboardService_GetDashboardStatsAsync_Tests : AdminDashboardServiceTestBase
    {
        [TestMethod]
        public async Task GetDashboardStatsAsync_Success_ReturnsDto_And_NoErrorLog()
        {
            // Arrange
            var expected = new DashboardStatsDTO
            {
                TotalUsers = 10,
                TotalProperties = 20,
                TotalPosts = 5,
                TotalRevenue = 123.45m,
                NewUsersToday = 2
            };
            Repo.Setup(r => r.GetDashboardStatsAsync()).ReturnsAsync(expected);

            // Act
            var result = await Svc.GetDashboardStatsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.TotalUsers);
            Assert.AreEqual(123.45m, result.TotalRevenue);
            Repo.Verify(r => r.GetDashboardStatsAsync(), Times.Once);

            // Happy path: không log lỗi
            Logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Never);
        }

        [TestMethod]
        public async Task GetDashboardStatsAsync_RepositoryThrows_LogsError_And_Rethrows()
        {
            // Arrange
            Repo.Setup(r => r.GetDashboardStatsAsync())
                .ThrowsAsync(new InvalidOperationException("DB error"));

            // Act + Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => Svc.GetDashboardStatsAsync());

            // Verify có log lỗi với thông điệp đúng
            VerifyErrorLogged(Logger, "Error getting dashboard stats", Times.Once());
            Repo.Verify(r => r.GetDashboardStatsAsync(), Times.Once);
        }
    }
}
