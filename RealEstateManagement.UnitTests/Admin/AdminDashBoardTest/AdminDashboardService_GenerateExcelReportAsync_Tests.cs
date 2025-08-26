using Moq;
using OfficeOpenXml;
using RealEstateManagement.Business.DTO.AdminDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Admin.AdminDashBoardTest
{
    [TestClass]
    public class AdminDashboardService_GenerateExcelReportAsync_Tests : AdminDashboardServiceTestBase
    {
        private static ExcelWorksheet OpenReportSheet(byte[] bytes)
        {
            Assert.IsNotNull(bytes);
            Assert.IsTrue(bytes.Length > 0, "Excel bytes should not be empty.");

            using var ms = new MemoryStream(bytes);
            using var pkg = new ExcelPackage(ms);
            var ws = pkg.Workbook.Worksheets["Report"];
            Assert.IsNotNull(ws, "Worksheet 'Report' should exist.");
            return ws; // <== CAUTION: only read simple values inside test before disposal
        }

        [TestMethod]
        public async Task GenerateExcelReportAsync_Daily_WritesHeadersAndRows()
        {
            // Arrange
            var req = new ReportRequestDTO
            {
                ReportType = "daily",
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 1, 2)
            };
            var daily = new List<DailyStatsDTO>
            {
                new DailyStatsDTO { Date = req.StartDate, NewUsers = 5, NewProperties = 2, NewPosts = 3, Revenue = 100m, Views = 50 },
                new DailyStatsDTO { Date = req.EndDate,   NewUsers = 1, NewProperties = 0, NewPosts = 1, Revenue =  20m, Views = 10 }
            };
            Repo.Setup(r => r.GetDailyStatsAsync(req.StartDate, req.EndDate)).ReturnsAsync(daily);

            // Act
            var bytes = await Svc.GenerateExcelReportAsync(req);

            // Assert (open once and copy values locally; sheet disposed with package)
            using var ms = new MemoryStream(bytes);
            using var pkg = new ExcelPackage(ms);
            var ws = pkg.Workbook.Worksheets["Report"];
            Assert.IsNotNull(ws);

            // headers
            Assert.AreEqual("Date", ws.Cells[1, 1].GetValue<string>());
            Assert.AreEqual("New Users", ws.Cells[1, 2].GetValue<string>());
            Assert.AreEqual("New Properties", ws.Cells[1, 3].GetValue<string>());
            Assert.AreEqual("New Posts", ws.Cells[1, 4].GetValue<string>());
            Assert.AreEqual("Revenue", ws.Cells[1, 5].GetValue<string>());
            Assert.AreEqual("Views", ws.Cells[1, 6].GetValue<string>());

            // first row
            Assert.AreEqual(req.StartDate.ToString("yyyy-MM-dd"), ws.Cells[2, 1].GetValue<string>());
            Assert.AreEqual(5, ws.Cells[2, 2].GetValue<int>());
            Assert.AreEqual(2, ws.Cells[2, 3].GetValue<int>());
            Assert.AreEqual(3, ws.Cells[2, 4].GetValue<int>());
            Assert.AreEqual(100, ws.Cells[2, 5].GetValue<decimal>());
            Assert.AreEqual(50, ws.Cells[2, 6].GetValue<int>());

            // second row
            Assert.AreEqual(req.EndDate.ToString("yyyy-MM-dd"), ws.Cells[3, 1].GetValue<string>());
            Assert.AreEqual(1, ws.Cells[3, 2].GetValue<int>());
            Assert.AreEqual(0, ws.Cells[3, 3].GetValue<int>());
            Assert.AreEqual(1, ws.Cells[3, 4].GetValue<int>());
            Assert.AreEqual(20, ws.Cells[3, 5].GetValue<decimal>());
            Assert.AreEqual(10, ws.Cells[3, 6].GetValue<int>());

            VerifyErrorLogged(Logger, "Error generating Excel report", Times.Never());
            Repo.Verify(r => r.GetDailyStatsAsync(req.StartDate, req.EndDate), Times.Once());
        }

        [TestMethod]
        public async Task GenerateExcelReportAsync_Monthly_WritesHeadersAndRows()
        {
            // Arrange
            var req = new ReportRequestDTO
            {
                ReportType = "monthly",
                StartDate = new DateTime(2025, 6, 1),
                EndDate = new DateTime(2025, 6, 30)
            };
            var monthly = new List<MonthlyStatsDTO>
            {
                new MonthlyStatsDTO { Year = 2025, Month = 1, NewUsers = 10, NewProperties = 3, NewPosts = 5, Revenue = 200m, Views = 1000 },
                new MonthlyStatsDTO { Year = 2025, Month = 2, NewUsers =  8, NewProperties = 2, NewPosts = 4, Revenue = 150m, Views =  800 }
            };
            Repo.Setup(r => r.GetMonthlyStatsAsync(2025)).ReturnsAsync(monthly);

            // Act
            var bytes = await Svc.GenerateExcelReportAsync(req);

            using var ms = new MemoryStream(bytes);
            using var pkg = new ExcelPackage(ms);
            var ws = pkg.Workbook.Worksheets["Report"];
            Assert.IsNotNull(ws);

            // headers
            Assert.AreEqual("Month", ws.Cells[1, 1].GetValue<string>());
            Assert.AreEqual("Year", ws.Cells[1, 2].GetValue<string>());
            Assert.AreEqual("New Users", ws.Cells[1, 3].GetValue<string>());
            Assert.AreEqual("New Properties", ws.Cells[1, 4].GetValue<string>());
            Assert.AreEqual("New Posts", ws.Cells[1, 5].GetValue<string>());
            Assert.AreEqual("Revenue", ws.Cells[1, 6].GetValue<string>());
            Assert.AreEqual("Views", ws.Cells[1, 7].GetValue<string>());

            // row 2
            Assert.AreEqual(1, ws.Cells[2, 1].GetValue<int>());
            Assert.AreEqual(2025, ws.Cells[2, 2].GetValue<int>());
            Assert.AreEqual(10, ws.Cells[2, 3].GetValue<int>());
            Assert.AreEqual(3, ws.Cells[2, 4].GetValue<int>());
            Assert.AreEqual(5, ws.Cells[2, 5].GetValue<int>());
            Assert.AreEqual(200, ws.Cells[2, 6].GetValue<decimal>());
            Assert.AreEqual(1000, ws.Cells[2, 7].GetValue<int>());

            // row 3
            Assert.AreEqual(2, ws.Cells[3, 1].GetValue<int>());
            Assert.AreEqual(150, ws.Cells[3, 6].GetValue<decimal>());
            Assert.AreEqual(800, ws.Cells[3, 7].GetValue<int>());

            VerifyErrorLogged(Logger, "Error generating Excel report", Times.Never());
            Repo.Verify(r => r.GetMonthlyStatsAsync(2025), Times.Once());
        }
        [TestMethod]
        public async Task GenerateExcelReportAsync_Property_WritesHeadersAndRows()
        {
            // Arrange
            var req = new ReportRequestDTO { ReportType = "property", StartDate = DateTime.Today, EndDate = DateTime.Today };
            var property = new List<PropertyStatsDTO>
            {
                new PropertyStatsDTO { Status = "Approved", Count = 5, TotalValue = 5000m }
            };
            Repo.Setup(r => r.GetPropertyStatsAsync()).ReturnsAsync(property);

            // Act
            var bytes = await Svc.GenerateExcelReportAsync(req);

            using var ms = new MemoryStream(bytes);
            using var pkg = new ExcelPackage(ms);
            var ws = pkg.Workbook.Worksheets["Report"];
            Assert.IsNotNull(ws);

            Assert.AreEqual("Status", ws.Cells[1, 1].GetValue<string>());
            Assert.AreEqual("Count", ws.Cells[1, 2].GetValue<string>());
            Assert.AreEqual("Total Value", ws.Cells[1, 3].GetValue<string>());

            Assert.AreEqual("Approved", ws.Cells[2, 1].GetValue<string>());
            Assert.AreEqual(5, ws.Cells[2, 2].GetValue<int>());
            Assert.AreEqual(5000m, ws.Cells[2, 3].GetValue<decimal>());

            VerifyErrorLogged(Logger, "Error generating Excel report", Times.Never());
            Repo.Verify(r => r.GetPropertyStatsAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GenerateExcelReportAsync_User_WritesHeadersAndRows()
        {
            // Arrange
            var req = new ReportRequestDTO { ReportType = "user", StartDate = DateTime.Today, EndDate = DateTime.Today };
            var users = new List<UserStatsDTO>
            {
                new UserStatsDTO { Role = "Admin", Count = 2, ActiveCount = 2, VerifiedCount = 1 }
            };
            Repo.Setup(r => r.GetUserStatsAsync()).ReturnsAsync(users);

            // Act
            var bytes = await Svc.GenerateExcelReportAsync(req);

            using var ms = new MemoryStream(bytes);
            using var pkg = new ExcelPackage(ms);
            var ws = pkg.Workbook.Worksheets["Report"];
            Assert.IsNotNull(ws);

            Assert.AreEqual("Role", ws.Cells[1, 1].GetValue<string>());
            Assert.AreEqual("Count", ws.Cells[1, 2].GetValue<string>());
            Assert.AreEqual("Active Count", ws.Cells[1, 3].GetValue<string>());
            Assert.AreEqual("Verified Count", ws.Cells[1, 4].GetValue<string>());

            Assert.AreEqual("Admin", ws.Cells[2, 1].GetValue<string>());
            Assert.AreEqual(2, ws.Cells[2, 2].GetValue<int>());
            Assert.AreEqual(2, ws.Cells[2, 3].GetValue<int>());
            Assert.AreEqual(1, ws.Cells[2, 4].GetValue<int>());

            VerifyErrorLogged(Logger, "Error generating Excel report", Times.Never());
            Repo.Verify(r => r.GetUserStatsAsync(), Times.Once());
        }

        [TestMethod]
        public async Task GenerateExcelReportAsync_Revenue_WritesHeadersAndRows()
        {
            // Arrange
            var req = new ReportRequestDTO
            {
                ReportType = "revenue",
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 1, 31)
            };
            var revenue = new List<RevenueStatsDTO>
            {
                new RevenueStatsDTO { Type = "Ads", Amount = 200m, TransactionCount = 3 }
            };
            Repo.Setup(r => r.GetRevenueStatsAsync(req.StartDate, req.EndDate)).ReturnsAsync(revenue);

            // Act
            var bytes = await Svc.GenerateExcelReportAsync(req);

            using var ms = new MemoryStream(bytes);
            using var pkg = new ExcelPackage(ms);
            var ws = pkg.Workbook.Worksheets["Report"];
            Assert.IsNotNull(ws);

            Assert.AreEqual("Type", ws.Cells[1, 1].GetValue<string>());
            Assert.AreEqual("Amount", ws.Cells[1, 2].GetValue<string>());
            Assert.AreEqual("Transaction Count", ws.Cells[1, 3].GetValue<string>());

            Assert.AreEqual("Ads", ws.Cells[2, 1].GetValue<string>());
            Assert.AreEqual(200m, ws.Cells[2, 2].GetValue<decimal>());
            Assert.AreEqual(3, ws.Cells[2, 3].GetValue<int>());

            VerifyErrorLogged(Logger, "Error generating Excel report", Times.Never());
            Repo.Verify(r => r.GetRevenueStatsAsync(req.StartDate, req.EndDate), Times.Once());
        }

        [TestMethod]
        public async Task GenerateExcelReportAsync_UnsupportedType_Throws_And_Logs()
        {
            // Arrange
            var req = new ReportRequestDTO
            {
                ReportType = "whatever",
                StartDate = DateTime.Today.AddDays(-7),
                EndDate = DateTime.Today
            };

            // Act + Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => Svc.GenerateExcelReportAsync(req));

            // Outer catch in GenerateExcelReportAsync logs this message:
            VerifyErrorLogged(Logger, "Error generating Excel report", Times.Once());
            // No repo calls expected
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task GenerateExcelReportAsync_Daily_RepoThrows_LogsBothAndRethrows()
        {
            // Arrange
            var req = new ReportRequestDTO
            {
                ReportType = "daily",
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 1, 2)
            };
            // Inner method GetDailyStatsAsync will log "Error getting daily stats",
            // then outer GenerateExcelReportAsync will log "Error generating Excel report".
            Repo.Setup(r => r.GetDailyStatsAsync(req.StartDate, req.EndDate))
                .ThrowsAsync(new InvalidOperationException("DB fail"));

            // Act + Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => Svc.GenerateExcelReportAsync(req));

            VerifyErrorLogged(Logger, "Error getting daily stats", Times.Once());
            VerifyErrorLogged(Logger, "Error generating Excel report", Times.Once());
            Repo.Verify(r => r.GetDailyStatsAsync(req.StartDate, req.EndDate), Times.Once());
        }
    }
}
