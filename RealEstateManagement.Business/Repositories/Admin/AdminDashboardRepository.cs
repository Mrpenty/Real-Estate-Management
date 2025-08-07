using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.AdminDTO;
using RealEstateManagement.Data.Data;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Data.Entity.Payment;
using RealEstateManagement.Business.Repositories.Repository;

namespace RealEstateManagement.Business.Repositories.Admin
{
    public class AdminDashboardRepository : RepositoryAsync<PropertyPost>, IAdminDashboardRepository
    {
        private readonly RentalDbContext _context;

        public AdminDashboardRepository(RentalDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDTO> GetDashboardStatsAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            var totalUsers = await GetTotalUsersAsync();
            var newUsersToday = await GetNewUsersTodayAsync();
            var totalProperties = await GetTotalPropertiesAsync();
            var newPropertiesToday = await GetNewPropertiesTodayAsync();
            var totalPosts = await GetTotalPostsAsync();
            var newPostsToday = await GetNewPostsTodayAsync();
            var totalRevenue = await GetTotalRevenueAsync();
            var revenueToday = await GetRevenueTodayAsync();
            var pendingPosts = await GetPendingPostsAsync();
            var approvedPosts = await GetApprovedPostsAsync();
            var rejectedPosts = await GetRejectedPostsAsync();
            var rentedProperties = await GetRentedPropertiesAsync();
            var availableProperties = await GetAvailablePropertiesAsync();

            return new DashboardStatsDTO
            {
                TotalUsers = totalUsers,
                TotalProperties = totalProperties,
                TotalPosts = totalPosts,
                TotalRevenue = totalRevenue,
                NewUsersToday = newUsersToday,
                NewPropertiesToday = newPropertiesToday,
                NewPostsToday = newPostsToday,
                RevenueToday = revenueToday,
                PendingPosts = pendingPosts,
                ApprovedPosts = approvedPosts,
                RejectedPosts = rejectedPosts,
                RentedProperties = rentedProperties,
                AvailableProperties = availableProperties
            };
        }

        public async Task<List<DailyStatsDTO>> GetDailyStatsAsync(DateTime startDate, DateTime endDate)
        {
            var stats = await _context.Properties
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt < endDate.AddDays(1))
                .GroupBy(p => p.CreatedAt.Date)
                .Select(g => new DailyStatsDTO
                {
                    Date = g.Key,
                    NewProperties = g.Count(),
                    Views = g.Sum(p => p.ViewsCount)
                })
                .ToListAsync();

            var userStats = await _context.Users
                .Where(u => u.CreatedAt >= startDate && u.CreatedAt < endDate.AddDays(1))
                .GroupBy(u => u.CreatedAt.Date)
                .Select(g => new { Date = g.Key, NewUsers = g.Count() })
                .ToListAsync();

            var postStats = await _context.PropertyPosts
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt < endDate.AddDays(1))
                .GroupBy(p => p.CreatedAt.Date)
                .Select(g => new { Date = g.Key, NewPosts = g.Count() })
                .ToListAsync();

            var revenueStats = await _context.WalletTransactions
                .Where(wt => wt.CreatedAt >= startDate && wt.CreatedAt < endDate.AddDays(1) && wt.Status == "Success")
                .GroupBy(wt => wt.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Revenue = g.Sum(wt => wt.Amount) })
                .ToListAsync();

            // Merge all stats
            var result = stats.Select(s => new DailyStatsDTO
            {
                Date = s.Date,
                NewProperties = s.NewProperties,
                Views = s.Views,
                NewUsers = userStats.FirstOrDefault(us => us.Date == s.Date)?.NewUsers ?? 0,
                NewPosts = postStats.FirstOrDefault(ps => ps.Date == s.Date)?.NewPosts ?? 0,
                Revenue = revenueStats.FirstOrDefault(rs => rs.Date == s.Date)?.Revenue ?? 0
            }).ToList();

            return result;
        }

        public async Task<List<MonthlyStatsDTO>> GetMonthlyStatsAsync(int year)
        {
            var stats = await _context.Properties
                .Where(p => p.CreatedAt.Year == year)
                .GroupBy(p => p.CreatedAt.Month)
                .Select(g => new MonthlyStatsDTO
                {
                    Month = g.Key,
                    Year = year,
                    NewProperties = g.Count(),
                    Views = g.Sum(p => p.ViewsCount)
                })
                .ToListAsync();

            var userStats = await _context.Users
                .Where(u => u.CreatedAt.Year == year)
                .GroupBy(u => u.CreatedAt.Month)
                .Select(g => new { Month = g.Key, NewUsers = g.Count() })
                .ToListAsync();

            var postStats = await _context.PropertyPosts
                .Where(p => p.CreatedAt.Year == year)
                .GroupBy(p => p.CreatedAt.Month)
                .Select(g => new { Month = g.Key, NewPosts = g.Count() })
                .ToListAsync();

            var revenueStats = await _context.WalletTransactions
                .Where(wt => wt.CreatedAt.Year == year && wt.Status == "Success")
                .GroupBy(wt => wt.CreatedAt.Month)
                .Select(g => new { Month = g.Key, Revenue = g.Sum(wt => wt.Amount) })
                .ToListAsync();

            // Merge all stats
            var result = stats.Select(s => new MonthlyStatsDTO
            {
                Month = s.Month,
                Year = s.Year,
                NewProperties = s.NewProperties,
                Views = s.Views,
                NewUsers = userStats.FirstOrDefault(us => us.Month == s.Month)?.NewUsers ?? 0,
                NewPosts = postStats.FirstOrDefault(ps => ps.Month == s.Month)?.NewPosts ?? 0,
                Revenue = revenueStats.FirstOrDefault(rs => rs.Month == s.Month)?.Revenue ?? 0
            }).ToList();

            return result;
        }

        public async Task<List<PropertyStatsDTO>> GetPropertyStatsAsync()
        {
            var stats = await _context.Properties
                .GroupBy(p => p.Status)
                .Select(g => new PropertyStatsDTO
                {
                    Status = g.Key ?? "Unknown",
                    Count = g.Count(),
                    TotalValue = g.Sum(p => p.Price)
                })
                .ToListAsync();

            return stats;
        }

        public async Task<List<UserStatsDTO>> GetUserStatsAsync()
        {
            var stats = await _context.Users
                .GroupBy(u => u.Role)
                .Select(g => new UserStatsDTO
                {
                    Role = g.Key,
                    Count = g.Count(),
                    ActiveCount = g.Count(u => u.IsActive),
                    VerifiedCount = g.Count(u => u.IsVerified)
                })
                .ToListAsync();

            return stats;
        }

        public async Task<List<RevenueStatsDTO>> GetRevenueStatsAsync(DateTime startDate, DateTime endDate)
        {
            var stats = await _context.WalletTransactions
                .Where(wt => wt.CreatedAt >= startDate && wt.CreatedAt < endDate.AddDays(1) && wt.Status == "Success")
                .GroupBy(wt => wt.Type)
                .Select(g => new RevenueStatsDTO
                {
                    Type = g.Key,
                    Amount = g.Sum(wt => wt.Amount),
                    TransactionCount = g.Count()
                })
                .ToListAsync();

            return stats;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetNewUsersTodayAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            return await _context.Users.CountAsync(u => u.CreatedAt >= today && u.CreatedAt < tomorrow);
        }

        public async Task<int> GetTotalPropertiesAsync()
        {
            return await _context.Properties.CountAsync();
        }

        public async Task<int> GetNewPropertiesTodayAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            return await _context.Properties.CountAsync(p => p.CreatedAt >= today && p.CreatedAt < tomorrow);
        }

        public async Task<int> GetTotalPostsAsync()
        {
            return await _context.PropertyPosts.CountAsync();
        }

        public async Task<int> GetNewPostsTodayAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            return await _context.PropertyPosts.CountAsync(p => p.CreatedAt >= today && p.CreatedAt < tomorrow);
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.WalletTransactions
                .Where(wt => wt.Status == "Success")
                .SumAsync(wt => wt.Amount);
        }

        public async Task<decimal> GetRevenueTodayAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            return await _context.WalletTransactions
                .Where(wt => wt.CreatedAt >= today && wt.CreatedAt < tomorrow && wt.Status == "Success")
                .SumAsync(wt => wt.Amount);
        }

        public async Task<int> GetPendingPostsAsync()
        {
            return await _context.PropertyPosts.CountAsync(p => p.Status == PropertyPost.PropertyPostStatus.Pending);
        }

        public async Task<int> GetApprovedPostsAsync()
        {
            return await _context.PropertyPosts.CountAsync(p => p.Status == PropertyPost.PropertyPostStatus.Approved);
        }

        public async Task<int> GetRejectedPostsAsync()
        {
            return await _context.PropertyPosts.CountAsync(p => p.Status == PropertyPost.PropertyPostStatus.Rejected);
        }

        public async Task<int> GetRentedPropertiesAsync()
        {
            return await _context.PropertyPosts.CountAsync(p => p.Status == PropertyPost.PropertyPostStatus.Rented);
        }

        public async Task<int> GetAvailablePropertiesAsync()
        {
            return await _context.PropertyPosts.CountAsync(p => p.Status == PropertyPost.PropertyPostStatus.Approved);
        }
    }
} 