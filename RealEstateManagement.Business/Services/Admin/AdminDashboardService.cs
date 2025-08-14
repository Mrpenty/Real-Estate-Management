using Microsoft.Extensions.Logging;
using RealEstateManagement.Business.DTO.AdminDTO;
using RealEstateManagement.Business.Repositories.Admin;
using OfficeOpenXml;

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

        public async Task<byte[]> GenerateExcelReportAsync(ReportRequestDTO request)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Report");
                    
                    // Set up headers based on report type
                    switch (request.ReportType.ToLower())
                    {
                        case "daily":
                            var dailyStats = await GetDailyStatsAsync(request.StartDate, request.EndDate);
                            worksheet.Cells[1, 1].Value = "Date";
                            worksheet.Cells[1, 2].Value = "New Users";
                            worksheet.Cells[1, 3].Value = "New Properties";
                            worksheet.Cells[1, 4].Value = "New Posts";
                            worksheet.Cells[1, 5].Value = "Revenue";
                            worksheet.Cells[1, 6].Value = "Views";
                            
                            for (int i = 0; i < dailyStats.Count; i++)
                            {
                                var stat = dailyStats[i];
                                worksheet.Cells[i + 2, 1].Value = stat.Date.ToString("yyyy-MM-dd");
                                worksheet.Cells[i + 2, 2].Value = stat.NewUsers;
                                worksheet.Cells[i + 2, 3].Value = stat.NewProperties;
                                worksheet.Cells[i + 2, 4].Value = stat.NewPosts;
                                worksheet.Cells[i + 2, 5].Value = stat.Revenue;
                                worksheet.Cells[i + 2, 6].Value = stat.Views;
                            }
                            break;

                        case "monthly":
                            var monthlyStats = await GetMonthlyStatsAsync(request.StartDate.Year);
                            worksheet.Cells[1, 1].Value = "Month";
                            worksheet.Cells[1, 2].Value = "Year";
                            worksheet.Cells[1, 3].Value = "New Users";
                            worksheet.Cells[1, 4].Value = "New Properties";
                            worksheet.Cells[1, 5].Value = "New Posts";
                            worksheet.Cells[1, 6].Value = "Revenue";
                            worksheet.Cells[1, 7].Value = "Views";
                            
                            for (int i = 0; i < monthlyStats.Count; i++)
                            {
                                var stat = monthlyStats[i];
                                worksheet.Cells[i + 2, 1].Value = stat.Month;
                                worksheet.Cells[i + 2, 2].Value = stat.Year;
                                worksheet.Cells[i + 2, 3].Value = stat.NewUsers;
                                worksheet.Cells[i + 2, 4].Value = stat.NewProperties;
                                worksheet.Cells[i + 2, 5].Value = stat.NewPosts;
                                worksheet.Cells[i + 2, 6].Value = stat.Revenue;
                                worksheet.Cells[i + 2, 7].Value = stat.Views;
                            }
                            break;

                        case "property":
                            var propertyStats = await GetPropertyStatsAsync();
                            worksheet.Cells[1, 1].Value = "Status";
                            worksheet.Cells[1, 2].Value = "Count";
                            worksheet.Cells[1, 3].Value = "Total Value";
                            
                            for (int i = 0; i < propertyStats.Count; i++)
                            {
                                var stat = propertyStats[i];
                                worksheet.Cells[i + 2, 1].Value = stat.Status;
                                worksheet.Cells[i + 2, 2].Value = stat.Count;
                                worksheet.Cells[i + 2, 3].Value = stat.TotalValue;
                            }
                            break;

                        case "user":
                            var userStats = await GetUserStatsAsync();
                            worksheet.Cells[1, 1].Value = "Role";
                            worksheet.Cells[1, 2].Value = "Count";
                            worksheet.Cells[1, 3].Value = "Active Count";
                            worksheet.Cells[1, 4].Value = "Verified Count";
                            
                            for (int i = 0; i < userStats.Count; i++)
                            {
                                var stat = userStats[i];
                                worksheet.Cells[i + 2, 1].Value = stat.Role;
                                worksheet.Cells[i + 2, 2].Value = stat.Count;
                                worksheet.Cells[i + 2, 3].Value = stat.ActiveCount;
                                worksheet.Cells[i + 2, 4].Value = stat.VerifiedCount;
                            }
                            break;

                        case "revenue":
                            var revenueStats = await GetRevenueStatsAsync(request.StartDate, request.EndDate);
                            worksheet.Cells[1, 1].Value = "Type";
                            worksheet.Cells[1, 2].Value = "Amount";
                            worksheet.Cells[1, 3].Value = "Transaction Count";
                            
                            for (int i = 0; i < revenueStats.Count; i++)
                            {
                                var stat = revenueStats[i];
                                worksheet.Cells[i + 2, 1].Value = stat.Type;
                                worksheet.Cells[i + 2, 2].Value = stat.Amount;
                                worksheet.Cells[i + 2, 3].Value = stat.TransactionCount;
                            }
                            break;

                        default:
                            throw new ArgumentException("Unsupported report type");
                    }

                    // Auto-fit columns
                    worksheet.Cells.AutoFitColumns();
                    
                    // Style the header row
                    using (var range = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    return await package.GetAsByteArrayAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Excel report");
                throw;
            }
        }
    }
} 