using Microsoft.Extensions.Logging;
using RealEstateManagement.Business.DTO.AdminDTO;
using RealEstateManagement.Business.Repositories.Admin;
using RealEstateManagement.Business.Services.Admin;
using System.Text;

namespace RealEstateManagement.Business.Services.Admin
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IAdminDashboardRepository _adminDashboardRepository;
        private readonly ILogger<AdminDashboardService> _logger;

        public AdminDashboardService(IAdminDashboardRepository adminDashboardRepository, ILogger<AdminDashboardService> logger)
        {
            _adminDashboardRepository = adminDashboardRepository;
            _logger = logger;
        }

        public async Task<DashboardStatsDTO> GetDashboardStatsAsync()
        {
            try
            {
                return await _adminDashboardRepository.GetDashboardStatsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard stats");
                throw;
            }
        }

        public async Task<List<DailyStatsDTO>> GetDailyStatsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _adminDashboardRepository.GetDailyStatsAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting daily stats");
                throw;
            }
        }

        public async Task<List<MonthlyStatsDTO>> GetMonthlyStatsAsync(int year)
        {
            try
            {
                return await _adminDashboardRepository.GetMonthlyStatsAsync(year);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly stats");
                throw;
            }
        }

        public async Task<List<PropertyStatsDTO>> GetPropertyStatsAsync()
        {
            try
            {
                return await _adminDashboardRepository.GetPropertyStatsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting property stats");
                throw;
            }
        }

        public async Task<List<UserStatsDTO>> GetUserStatsAsync()
        {
            try
            {
                return await _adminDashboardRepository.GetUserStatsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user stats");
                throw;
            }
        }

        public async Task<List<RevenueStatsDTO>> GetRevenueStatsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _adminDashboardRepository.GetRevenueStatsAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting revenue stats");
                throw;
            }
        }

        public async Task<byte[]> GenerateReportAsync(ReportRequestDTO request)
        {
            try
            {
                switch (request.Format.ToLower())
                {
                    case "excel":
                        return await GenerateExcelReportAsync(request);
                    case "pdf":
                        return await GeneratePdfReportAsync(request);
                    default:
                        throw new ArgumentException("Unsupported format. Use 'excel' or 'pdf'");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating report");
                throw;
            }
        }

        private async Task<byte[]> GenerateExcelReportAsync(ReportRequestDTO request)
        {
            var csvContent = new StringBuilder();
            
            switch (request.ReportType.ToLower())
            {
                case "daily":
                    var dailyStats = await GetDailyStatsAsync(request.StartDate, request.EndDate);
                    csvContent.AppendLine("Date,NewUsers,NewProperties,NewPosts,Revenue,Views");
                    foreach (var stat in dailyStats)
                    {
                        csvContent.AppendLine($"{stat.Date:yyyy-MM-dd},{stat.NewUsers},{stat.NewProperties},{stat.NewPosts},{stat.Revenue},{stat.Views}");
                    }
                    break;

                case "monthly":
                    var monthlyStats = await GetMonthlyStatsAsync(request.StartDate.Year);
                    csvContent.AppendLine("Month,Year,NewUsers,NewProperties,NewPosts,Revenue,Views");
                    foreach (var stat in monthlyStats)
                    {
                        csvContent.AppendLine($"{stat.Month},{stat.Year},{stat.NewUsers},{stat.NewProperties},{stat.NewPosts},{stat.Revenue},{stat.Views}");
                    }
                    break;

                case "property":
                    var propertyStats = await GetPropertyStatsAsync();
                    csvContent.AppendLine("Status,Count,TotalValue");
                    foreach (var stat in propertyStats)
                    {
                        csvContent.AppendLine($"{stat.Status},{stat.Count},{stat.TotalValue}");
                    }
                    break;

                case "user":
                    var userStats = await GetUserStatsAsync();
                    csvContent.AppendLine("Role,Count,ActiveCount,VerifiedCount");
                    foreach (var stat in userStats)
                    {
                        csvContent.AppendLine($"{stat.Role},{stat.Count},{stat.ActiveCount},{stat.VerifiedCount}");
                    }
                    break;

                case "revenue":
                    var revenueStats = await GetRevenueStatsAsync(request.StartDate, request.EndDate);
                    csvContent.AppendLine("Type,Amount,TransactionCount");
                    foreach (var stat in revenueStats)
                    {
                        csvContent.AppendLine($"{stat.Type},{stat.Amount},{stat.TransactionCount}");
                    }
                    break;

                default:
                    throw new ArgumentException("Unsupported report type");
            }

            return Encoding.UTF8.GetBytes(csvContent.ToString());
        }

        private async Task<byte[]> GeneratePdfReportAsync(ReportRequestDTO request)
        {
            // Simple HTML-based PDF generation
            var htmlContent = new StringBuilder();
            htmlContent.AppendLine("<!DOCTYPE html>");
            htmlContent.AppendLine("<html><head><title>Real Estate Management Report</title>");
            htmlContent.AppendLine("<style>");
            htmlContent.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            htmlContent.AppendLine("table { border-collapse: collapse; width: 100%; margin-top: 20px; }");
            htmlContent.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            htmlContent.AppendLine("th { background-color: #f2f2f2; }");
            htmlContent.AppendLine("h1 { color: #333; }");
            htmlContent.AppendLine("</style></head><body>");
            
            htmlContent.AppendLine($"<h1>Real Estate Management Report</h1>");
            htmlContent.AppendLine($"<p><strong>Report Type:</strong> {request.ReportType}</p>");
            htmlContent.AppendLine($"<p><strong>Period:</strong> {request.StartDate:yyyy-MM-dd} to {request.EndDate:yyyy-MM-dd}</p>");
            htmlContent.AppendLine($"<p><strong>Generated:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");

            switch (request.ReportType.ToLower())
            {
                case "daily":
                    var dailyStats = await GetDailyStatsAsync(request.StartDate, request.EndDate);
                    htmlContent.AppendLine("<table>");
                    htmlContent.AppendLine("<tr><th>Date</th><th>New Users</th><th>New Properties</th><th>New Posts</th><th>Revenue</th><th>Views</th></tr>");
                    foreach (var stat in dailyStats)
                    {
                        htmlContent.AppendLine($"<tr><td>{stat.Date:yyyy-MM-dd}</td><td>{stat.NewUsers}</td><td>{stat.NewProperties}</td><td>{stat.NewPosts}</td><td>{stat.Revenue:C}</td><td>{stat.Views}</td></tr>");
                    }
                    htmlContent.AppendLine("</table>");
                    break;

                case "monthly":
                    var monthlyStats = await GetMonthlyStatsAsync(request.StartDate.Year);
                    htmlContent.AppendLine("<table>");
                    htmlContent.AppendLine("<tr><th>Month</th><th>Year</th><th>New Users</th><th>New Properties</th><th>New Posts</th><th>Revenue</th><th>Views</th></tr>");
                    foreach (var stat in monthlyStats)
                    {
                        htmlContent.AppendLine($"<tr><td>{stat.Month}</td><td>{stat.Year}</td><td>{stat.NewUsers}</td><td>{stat.NewProperties}</td><td>{stat.NewPosts}</td><td>{stat.Revenue:C}</td><td>{stat.Views}</td></tr>");
                    }
                    htmlContent.AppendLine("</table>");
                    break;

                case "property":
                    var propertyStats = await GetPropertyStatsAsync();
                    htmlContent.AppendLine("<table>");
                    htmlContent.AppendLine("<tr><th>Status</th><th>Count</th><th>Total Value</th></tr>");
                    foreach (var stat in propertyStats)
                    {
                        htmlContent.AppendLine($"<tr><td>{stat.Status}</td><td>{stat.Count}</td><td>{stat.TotalValue:C}</td></tr>");
                    }
                    htmlContent.AppendLine("</table>");
                    break;

                case "user":
                    var userStats = await GetUserStatsAsync();
                    htmlContent.AppendLine("<table>");
                    htmlContent.AppendLine("<tr><th>Role</th><th>Count</th><th>Active Count</th><th>Verified Count</th></tr>");
                    foreach (var stat in userStats)
                    {
                        htmlContent.AppendLine($"<tr><td>{stat.Role}</td><td>{stat.Count}</td><td>{stat.ActiveCount}</td><td>{stat.VerifiedCount}</td></tr>");
                    }
                    htmlContent.AppendLine("</table>");
                    break;

                case "revenue":
                    var revenueStats = await GetRevenueStatsAsync(request.StartDate, request.EndDate);
                    htmlContent.AppendLine("<table>");
                    htmlContent.AppendLine("<tr><th>Type</th><th>Amount</th><th>Transaction Count</th></tr>");
                    foreach (var stat in revenueStats)
                    {
                        htmlContent.AppendLine($"<tr><td>{stat.Type}</td><td>{stat.Amount:C}</td><td>{stat.TransactionCount}</td></tr>");
                    }
                    htmlContent.AppendLine("</table>");
                    break;

                default:
                    throw new ArgumentException("Unsupported report type");
            }

            htmlContent.AppendLine("</body></html>");

            return Encoding.UTF8.GetBytes(htmlContent.ToString());
        }
    }
} 