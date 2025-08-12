using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Business.DTO.AdminDTO
{
    public class DashboardStatsDTO
    {
        public int TotalUsers { get; set; }
        public int TotalProperties { get; set; }
        public int TotalPosts { get; set; }
        public decimal TotalRevenue { get; set; }
        public int NewUsersToday { get; set; }
        public int NewPropertiesToday { get; set; }
        public int NewPostsToday { get; set; }
        public decimal RevenueToday { get; set; }
        public int PendingPosts { get; set; }
        public int ApprovedPosts { get; set; }
        public int RejectedPosts { get; set; }
        public int RentedProperties { get; set; }
        public int AvailableProperties { get; set; }
    }

    public class DailyStatsDTO
    {
        public DateTime Date { get; set; }
        public int NewUsers { get; set; }
        public int NewProperties { get; set; }
        public int NewPosts { get; set; }
        public decimal Revenue { get; set; }
        public int Views { get; set; }
    }

    public class MonthlyStatsDTO
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int NewUsers { get; set; }
        public int NewProperties { get; set; }
        public int NewPosts { get; set; }
        public decimal Revenue { get; set; }
        public int Views { get; set; }
    }

    public class PropertyStatsDTO
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalValue { get; set; }
    }

    public class UserStatsDTO
    {
        public string Role { get; set; } = string.Empty;
        public int Count { get; set; }
        public int ActiveCount { get; set; }
        public int VerifiedCount { get; set; }
    }

    public class RevenueStatsDTO
    {
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int TransactionCount { get; set; }
    }

    public class ReportRequestDTO
    {
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string ReportType { get; set; } = string.Empty; // "daily", "monthly", "property", "user", "revenue"
    }
} 