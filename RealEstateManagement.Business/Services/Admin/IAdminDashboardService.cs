using RealEstateManagement.Business.DTO.AdminDTO;

namespace RealEstateManagement.Business.Services.Admin
{
    public interface IAdminDashboardService
    {
        Task<DashboardStatsDTO> GetDashboardStatsAsync();
        Task<List<DailyStatsDTO>> GetDailyStatsAsync(DateTime startDate, DateTime endDate);
        Task<List<MonthlyStatsDTO>> GetMonthlyStatsAsync(int year);
        Task<List<PropertyStatsDTO>> GetPropertyStatsAsync();
        Task<List<UserStatsDTO>> GetUserStatsAsync();
        Task<List<RevenueStatsDTO>> GetRevenueStatsAsync(DateTime startDate, DateTime endDate);
        Task<byte[]> GenerateReportAsync(ReportRequestDTO request);
    }
} 