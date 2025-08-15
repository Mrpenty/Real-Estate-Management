using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Extensions.Logging;
using RealEstateManagement.Business.Services.Admin;
using RealEstateManagement.Business.Repositories.Admin;
using RealEstateManagement.Business.DTO.AdminDTO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

[TestClass]
public class AdminDashboardServiceTests
{
    private Mock<IAdminDashboardRepository> _mockAdminDashboardRepository;
    private Mock<ILogger<AdminDashboardService>> _mockLogger;
    private AdminDashboardService _adminDashboardService;

    [TestInitialize]
    public void Setup()
    {
        _mockAdminDashboardRepository = new Mock<IAdminDashboardRepository>();
        _mockLogger = new Mock<ILogger<AdminDashboardService>>();
        _adminDashboardService = new AdminDashboardService(_mockAdminDashboardRepository.Object, _mockLogger.Object);
    }

    [TestMethod]
    public async Task GenerateReportAsync_ExcelReportDaily_ReturnsByteArray()
    {
        var request = new ReportRequestDTO { ReportType = "daily", StartDate = DateTime.Now.AddDays(-7), EndDate = DateTime.Now };
        var dailyStats = new List<DailyStatsDTO>
        {
            new DailyStatsDTO { Date = DateTime.Now.AddDays(-1), NewUsers = 10, NewProperties = 5, NewPosts = 2, Revenue = 1000, Views = 200 }
        };
        _mockAdminDashboardRepository.Setup(r => r.GetDailyStatsAsync(request.StartDate, request.EndDate)).ReturnsAsync(dailyStats);

        var result = await _adminDashboardService.GenerateExcelReportAsync(request);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
        var csvContent = Encoding.UTF8.GetString(result);
        StringAssert.Contains(csvContent, "Date,NewUsers,NewProperties,NewPosts,Revenue,Views");
        StringAssert.Contains(csvContent, $"{dailyStats[0].Date:yyyy-MM-dd},10,5,2,1000,200");
    }

    [TestMethod]
    public async Task GenerateReportAsync_PdfReportMonthly_ReturnsByteArray()
    {
        var request = new ReportRequestDTO { ReportType = "monthly", StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 12, 31) };
        var monthlyStats = new List<MonthlyStatsDTO>
        {
            new MonthlyStatsDTO { Month = 1, Year = 2023, NewUsers = 50, NewProperties = 20, NewPosts = 10, Revenue = 5000, Views = 1000 }
        };
        _mockAdminDashboardRepository.Setup(r => r.GetMonthlyStatsAsync(request.StartDate.Year)).ReturnsAsync(monthlyStats);

        var result = await _adminDashboardService.GenerateExcelReportAsync(request);

        Assert.IsNotNull(result);
        Assert.IsTrue(result.Length > 0);
        var htmlContent = Encoding.UTF8.GetString(result);
        StringAssert.Contains(htmlContent, "<h1>Real Estate Management Report</h1>");
        StringAssert.Contains(htmlContent, "<th>Month</th><th>Year</th><th>New Users</th><th>New Properties</th><th>New Posts</th><th>Revenue</th><th>Views</th>");
        StringAssert.Contains(htmlContent, $"<td>1</td><td>2023</td><td>50</td><td>20</td><td>10</td><td>5.000,00 ₫</td><td>1000</td>");
    }

    [TestMethod]
    public async Task GenerateReportAsync_UnsupportedFormat_ThrowsArgumentException()
    {
        var request = new ReportRequestDTO { ReportType = "daily" };

        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _adminDashboardService.GenerateExcelReportAsync(request));
        _mockLogger.Verify(
            x => x.LogError(It.IsAny<Exception>(), "Error generating report"),
            Times.Once);
    }

    [TestMethod]
    public async Task GenerateReportAsync_UnsupportedReportType_ThrowsArgumentException()
    {
        var request = new ReportRequestDTO { ReportType = "unsupported" };

        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _adminDashboardService.GenerateExcelReportAsync(request));
    }
}