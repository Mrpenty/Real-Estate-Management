using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.AdminDTO;
using RealEstateManagement.Business.Services.Admin;
using RealEstateManagement.Data.Entity.User;

namespace RealEstateManagement.API.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/Admin/Dashboard")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _adminDashboardService;
        private readonly ILogger<AdminDashboardController> _logger;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

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
        /// Generate and download Excel report
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

                if (string.IsNullOrEmpty(request.ReportType))
                {
                    return BadRequest(new { success = false, message = "Report type is required" });
                }

                var validReportTypes = new[] { "daily", "monthly", "property", "user", "revenue" };

                if (!validReportTypes.Contains(request.ReportType.ToLower()))
                {
                    return BadRequest(new { success = false, message = "Invalid report type" });
                }

                var reportData = await _adminDashboardService.GenerateExcelReportAsync(request);
                
                string fileName = $"report_{request.ReportType}_{request.StartDate:yyyyMMdd}_{request.EndDate:yyyyMMdd}.xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(reportData, contentType, fileName);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Excel report");
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

        [HttpPut("update-user-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Update custom Role field
                user.Role = request.NewRole;
                await _userManager.UpdateAsync(user);

                // Update ASP.NET Core Identity roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                // Add new role
                var roleResult = await _userManager.AddToRoleAsync(user, request.NewRole);
                if (!roleResult.Succeeded)
                {
                    return BadRequest($"Failed to update role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }

                // Invalidate existing refresh tokens for this user to force re-authentication
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;
                await _userManager.UpdateAsync(user);

                return Ok(new { 
                    message = $"User role updated to {request.NewRole} successfully",
                    note = "User will need to login again to get new token with updated role"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user role");
                return StatusCode(500, "An error occurred while updating user role");
            }
        }

        /// <summary>
        /// Force refresh user token after role change (Admin only)
        /// </summary>
        [HttpPost("force-refresh-user-token/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ForceRefreshUserToken(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Generate new refresh token
                user.RefreshToken = Guid.NewGuid().ToString();
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                await _userManager.UpdateAsync(user);

                return Ok(new { 
                    message = "User token refreshed successfully",
                    note = "User should use new refresh token to get access token with updated role"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing user token");
                return StatusCode(500, "An error occurred while refreshing user token");
            }
        }
    }
} 