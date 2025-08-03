using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.AdminDTO;
using RealEstateManagement.Business.Services.Admin;

namespace RealEstateManagement.API.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/Admin/Dashboard")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly ILogger<AdminDashboardController> _logger;

        public AdminDashboardController(IAdminDashboardService adminDashboardService, ILogger<AdminDashboardController> logger)
        {
            _adminDashboardService = adminDashboardService;
            _logger = logger;
        }

        /// <summary>
        /// Get overall dashboard statistics
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var stats = await _adminDashboardService.GetDashboardStatsAsync();
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting dashboard stats");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get daily statistics for a date range
        /// </summary>
        [HttpGet("daily-stats")]
        public async Task<IActionResult> GetDailyStats([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BadRequest(new { success = false, message = "Start date must be before end date" });
                }

                var stats = await _adminDashboardService.GetDailyStatsAsync(startDate, endDate);
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting daily stats");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get monthly statistics for a specific year
        /// </summary>
        [HttpGet("monthly-stats/{year}")]
        public async Task<IActionResult> GetMonthlyStats(int year)
        {
            try
            {
                if (year < 2020 || year > DateTime.Now.Year + 1)
                {
                    return BadRequest(new { success = false, message = "Invalid year" });
                }

                var stats = await _adminDashboardService.GetMonthlyStatsAsync(year);
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting monthly stats");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get property statistics by status
        /// </summary>
        [HttpGet("property-stats")]
        public async Task<IActionResult> GetPropertyStats()
        {
            try
            {
                var stats = await _adminDashboardService.GetPropertyStatsAsync();
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting property stats");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get user statistics by role
        /// </summary>
        [HttpGet("user-stats")]
        public async Task<IActionResult> GetUserStats()
        {
            try
            {
                var stats = await _adminDashboardService.GetUserStatsAsync();
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user stats");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get revenue statistics for a date range
        /// </summary>
        [HttpGet("revenue-stats")]
        public async Task<IActionResult> GetRevenueStats([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BadRequest(new { success = false, message = "Start date must be before end date" });
                }

                var stats = await _adminDashboardService.GetRevenueStatsAsync(startDate, endDate);
                return Ok(new { success = true, data = stats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting revenue stats");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        /// <summary>
        /// Generate and download report (Excel/PDF)
        /// </summary>
        [HttpPost("download-report")]
        public async Task<IActionResult> DownloadReport([FromBody] ReportRequestDTO request)
        {
            try
            {
                if (request.StartDate > request.EndDate)
                {
                    return BadRequest(new { success = false, message = "Start date must be before end date" });
                }

                if (string.IsNullOrEmpty(request.ReportType) || string.IsNullOrEmpty(request.Format))
                {
                    return BadRequest(new { success = false, message = "Report type and format are required" });
                }

                var validReportTypes = new[] { "daily", "monthly", "property", "user", "revenue" };
                var validFormats = new[] { "excel", "pdf" };

                if (!validReportTypes.Contains(request.ReportType.ToLower()))
                {
                    return BadRequest(new { success = false, message = "Invalid report type" });
                }

                if (!validFormats.Contains(request.Format.ToLower()))
                {
                    return BadRequest(new { success = false, message = "Invalid format. Use 'excel' or 'pdf'" });
                }

                var reportData = await _adminDashboardService.GenerateReportAsync(request);
                
                string fileName = $"report_{request.ReportType}_{request.StartDate:yyyyMMdd}_{request.EndDate:yyyyMMdd}.{request.Format}";
                string contentType = request.Format.ToLower() == "excel" ? "text/csv" : "text/html";

                return File(reportData, contentType, fileName);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating report");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }

        /// <summary>
        /// Get system overview statistics
        /// </summary>
        [HttpGet("system-overview")]
        public async Task<IActionResult> GetSystemOverview()
        {
            try
            {
                var dashboardStats = await _adminDashboardService.GetDashboardStatsAsync();
                var propertyStats = await _adminDashboardService.GetPropertyStatsAsync();
                var userStats = await _adminDashboardService.GetUserStatsAsync();

                var overview = new
                {
                    Dashboard = dashboardStats,
                    Properties = propertyStats,
                    Users = userStats,
                    GeneratedAt = DateTime.Now
                };

                return Ok(new { success = true, data = overview });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting system overview");
                return StatusCode(500, new { success = false, message = "Internal server error" });
            }
        }
    }
} 