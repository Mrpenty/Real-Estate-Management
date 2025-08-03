using RealEstateManagement.Business.DTO.AdminDTO;

namespace RealEstateManagement.Business.Repositories.Admin
{
    public interface IAdminDashboardRepository
    {
        Task<DashboardStatsDTO> GetDashboardStatsAsync();
        Task<List<DailyStatsDTO>> GetDailyStatsAsync(DateTime startDate, DateTime endDate);
        Task<List<MonthlyStatsDTO>> GetMonthlyStatsAsync(int year);
        Task<List<PropertyStatsDTO>> GetPropertyStatsAsync();
        Task<List<UserStatsDTO>> GetUserStatsAsync();
        Task<List<RevenueStatsDTO>> GetRevenueStatsAsync(DateTime startDate, DateTime endDate);
        Task<int> GetTotalUsersAsync();
        Task<int> GetNewUsersTodayAsync();
        Task<int> GetTotalPropertiesAsync();
        Task<int> GetNewPropertiesTodayAsync();
        Task<int> GetTotalPostsAsync();
        Task<int> GetNewPostsTodayAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetRevenueTodayAsync();
        Task<int> GetPendingPostsAsync();
        Task<int> GetApprovedPostsAsync();
        Task<int> GetRejectedPostsAsync();
        Task<int> GetRentedPropertiesAsync();
        Task<int> GetAvailablePropertiesAsync();
    }
} 