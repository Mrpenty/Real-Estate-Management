using Moq;
using RealEstateManagement.Business.DTO.AdminDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Admin.AdminDashBoardTest
{
    public class AdminDashboardService_GetPropertyStatsAsync_Tests : AdminDashboardServiceTestBase
    {
        [TestMethod]
        public async Task GetPropertyStatsAsync_ReturnsData_WhenRepositorySucceeds()
        {
            // Arrange
            var expected = new List<PropertyStatsDTO>
            {
                new PropertyStatsDTO { Status = "Approved", Count = 5, TotalValue = 5000 }
            };
            Repo.Setup(r => r.GetPropertyStatsAsync()).ReturnsAsync(expected);

            // Act
            var result = await Svc.GetPropertyStatsAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Approved", result[0].Status);
            VerifyErrorLogged(Logger, "Error getting property stats", Times.Never());
            Repo.Verify(r => r.GetPropertyStatsAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GetPropertyStatsAsync_LogsErrorAndThrows_WhenRepositoryFails()
        {
            // Arrange
            var ex = new Exception("repo down");
            Repo.Setup(r => r.GetPropertyStatsAsync()).ThrowsAsync(ex);

            // Act & Assert
            var thrown = await Assert.ThrowsExceptionAsync<Exception>(() => Svc.GetPropertyStatsAsync());
            Assert.AreSame(ex, thrown);
            VerifyErrorLogged(Logger, "Error getting property stats", Times.Once());
            Repo.Verify(r => r.GetPropertyStatsAsync(), Times.Once());
        }
    }
}

