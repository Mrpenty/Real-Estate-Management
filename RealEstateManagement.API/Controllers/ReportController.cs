using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.ReportDTO;
using RealEstateManagement.Business.DTO.NotificationDTO;
using RealEstateManagement.Business.Services.NotificationService;
using RealEstateManagement.Data.Entity.ReportEntity;
using System;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly RentalDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationService _notificationService;

        public ReportController(RentalDbContext context, IHttpContextAccessor httpContextAccessor, INotificationService notificationService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
        }
        //[HttpPost("review/{userReporterId}")]
        //public async Task<IActionResult> ReportReview([FromBody] ReportCreateDto dto, int userReporterId)
        //{
        //    var existing = await _context.Reports.FirstOrDefaultAsync(r =>
        //        r.TargetId == dto.TargetId &&
        //        r.TargetType == "Review" &&
        //        r.ReportedByUserId == userReporterId);

        //    if (existing != null)
        //        return BadRequest("Bạn đã report review này rồi.");

        //    var report = new Report
        //    {
        //        TargetId = dto.TargetId,
        //        TargetType = "Review",
        //        ReportedByUserId = userReporterId,
        //        ReportedAt = DateTime.UtcNow,
        //        Reason = dto.Reason,
        //        Description = dto.Description,
        //        Status = "Pending"
        //    };

        //    _context.Reports.Add(report);
        //    await _context.SaveChangesAsync();
        //    return Ok("Đã gửi report review.");
        //}
        //[HttpPost("review-reply/{userReporterId}")]
        //public async Task<IActionResult> ReportReviewReply([FromBody] ReportCreateDto dto, int userReporterId)
        //{
        //    var existing = await _context.Reports.FirstOrDefaultAsync(r =>
        //        r.TargetId == dto.TargetId &&
        //        r.TargetType == "ReviewReply" &&
        //        r.ReportedByUserId == userReporterId);

        //    if (existing != null)
        //        return BadRequest("Bạn đã report phản hồi này rồi.");

        //    var report = new Report
        //    {
        //        TargetId = dto.TargetId,
        //        TargetType = "ReviewReply",
        //        ReportedByUserId = userReporterId,
        //        ReportedAt = DateTime.UtcNow,
        //        Reason = dto.Reason,
        //        Description = dto.Description,
        //        Status = "Pending"
        //    };

        //    _context.Reports.Add(report);
        //    await _context.SaveChangesAsync();
        //    return Ok("Đã gửi report phản hồi review.");
        //}
        [HttpPost("post/{userRepoertId}")]
        public async Task<IActionResult> ReportPost([FromBody] ReportCreateDto dto, int userRepoertId)
        {

            // Kiểm tra đã report chưa
            var existing = await _context.Reports.FirstOrDefaultAsync(r =>
                r.TargetId == dto.TargetId &&
                r.TargetType == "PropertyPost" &&
                r.ReportedByUserId == userRepoertId);

            if (existing != null)
                return BadRequest(new { message = "Bạn đã report bài đăng này rồi." });

            var report = new Report
            {
                TargetId = dto.TargetId,
                TargetType = "PropertyPost",
                ReportedByUserId = userRepoertId,
                ReportedAt = DateTime.UtcNow,
                Reason = dto.Reason,
                Description = dto.Description,
                Status = "Pending"
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            // Send notification to admins note  {dto.TargetType}
            try
            {
                await _notificationService.SendNotificationToAdminsAsync(new CreateNotificationDTO
                {
                    Title = "Báo cáo mới",
                    Content = $"Có báo cáo mới từ người dùng ID: {userRepoertId}",
                    Type = "Report",
                    Audience = "Admin"
                });
            }
            catch (Exception ex)
            {
                // Log error but don't fail the report creation
                Console.WriteLine($"Failed to send notification: {ex.Message}");
            }

            return Ok(new { message = "Đã gửi report." });
        }

        [HttpDelete("post/{targetId}/{userRepoertId}")]
        public async Task<IActionResult> CancelPostReport(int targetId, int userRepoertId)
        {
            var report = await _context.Reports.FirstOrDefaultAsync(r =>
                r.TargetId == targetId &&
                r.TargetType == "PropertyPost" &&
                r.ReportedByUserId == userRepoertId);

            if (report == null)
                return NotFound(new { message = "Bạn chưa report bài đăng này." });

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã hủy report." });
        }

        [HttpPost("user/{userRepoertId}")]
        public async Task<IActionResult> ReportUser([FromBody] ReportCreateDto dto, int userRepoertId)
        {
            var existing = await _context.Reports.FirstOrDefaultAsync(r =>
                r.TargetId == dto.TargetId &&
                r.TargetType == "User" &&
                r.ReportedByUserId == userRepoertId);

            if (existing != null)
                return BadRequest(new { message = "Bạn đã report người dùng này rồi." });

            var report = new Report
            {
                TargetId = dto.TargetId,
                TargetType = "User",
                ReportedByUserId = userRepoertId,
                ReportedAt = DateTime.UtcNow,
                Reason = dto.Reason,
                Description = dto.Description,
                Status = "Pending"
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            // Send notification to admins
            try
            {
                await _notificationService.SendNotificationToAdminsAsync(new CreateNotificationDTO
                {
                    Title = "Báo cáo người dùng mới",
                    Content = $"Có báo cáo mới về người dùng ID: {dto.TargetId} từ người dùng ID: {userRepoertId}",
                    Type = "Report",
                    Audience = "Admin"
                });
            }
            catch (Exception ex)
            {
                // Log error but don't fail the report creation
                Console.WriteLine($"Failed to send notification: {ex.Message}");
            }

            return Ok(new { message = "Đã gửi report người dùng." });
        }

        [HttpDelete("user/{targetId}/{userRepoertId}")]
        public async Task<IActionResult> CancelUserReport(int targetId, int userRepoertId)
        {

            var report = await _context.Reports.FirstOrDefaultAsync(r =>
                r.TargetId == targetId &&
                r.TargetType == "User" &&
                r.ReportedByUserId == userRepoertId);

            if (report == null)
                return NotFound(new { message = "Bạn chưa report người dùng này." });

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã hủy report người dùng." });
        }

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminViewAllReport(
            [FromQuery] string? targetType,
            [FromQuery] string? status,
            [FromQuery] string? keyword,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Reports
                .Include(r => r.ReportedByUser)
                .Include(r => r.ResolvedByUser)
                .AsQueryable();

            if (!string.IsNullOrEmpty(targetType))
                query = query.Where(r => r.TargetType == targetType);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Status == status);

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(r => r.Reason.Contains(keyword) || r.Description.Contains(keyword));

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var results = await query
                .OrderByDescending(r => r.ReportedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new
                {
                    r.Id,
                    r.TargetId,
                    r.TargetType,
                    r.Reason,
                    r.Description,
                    r.Status,
                    r.ReportedAt,
                    ReportedBy = new { r.ReportedByUser.Id, r.ReportedByUser.Name },
                    ResolvedBy = r.ResolvedByUser == null ? null : new { r.ResolvedByUser.Id, r.ResolvedByUser.Name },
                    r.ResolvedAt,
                    r.AdminNote
                })
                .ToListAsync();

            return Ok(new
            {
                reports = results,
                total = totalCount,
                totalPages = totalPages,
                currentPage = page,
                pageSize = pageSize
            });
        }

        [HttpGet("myReport/{userId}")]
        [Authorize]
        public async Task<IActionResult> UserViewMyReport(int userId)
        {
            var myReports = await _context.Reports
                .Where(r => r.ReportedByUserId == userId)
                .OrderByDescending(r => r.ReportedAt)
                .Select(r => new
                {
                    r.Id,
                    r.TargetId,
                    r.TargetType,
                    r.Reason,
                    r.Description,
                    r.Status,
                    r.ReportedAt,
                    r.ResolvedAt,
                    r.AdminNote
                })
                .ToListAsync();

            return Ok(myReports);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReportById(int id)
        {
            var report = await _context.Reports
                .Include(r => r.ReportedByUser)
                .Include(r => r.ResolvedByUser)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null)
                return NotFound(new { message = "Không tìm thấy báo cáo" });

            var result = new
            {
                report.Id,
                report.TargetId,
                report.TargetType,
                report.Reason,
                report.Description,
                report.Status,
                report.ReportedAt,
                report.ResolvedAt,
                report.AdminNote,
                ReportedBy = report.ReportedByUser == null ? null : new { report.ReportedByUser.Id, report.ReportedByUser.Name },
                ResolvedBy = report.ResolvedByUser == null ? null : new { report.ResolvedByUser.Id, report.ResolvedByUser.Name }
            };

            return Ok(result);
        }

        [HttpPut("{id}/resolve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResolveReport(int id, [FromBody] ResolveReportDto dto)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
                return NotFound(new { message = "Không tìm thấy báo cáo" });

            // Get current admin user ID
            var adminIdClaim = User.FindFirst("id") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (adminIdClaim == null || !int.TryParse(adminIdClaim.Value, out var adminId))
                return Unauthorized("Không thể xác định người xử lý");

            report.Status = "Resolved";
            report.ResolvedAt = DateTime.UtcNow;
            report.ResolvedByUserId = adminId;
            report.AdminNote = dto.AdminNote;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã xử lý báo cáo thành công" });
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
                return NotFound(new { message = "Không tìm thấy báo cáo" });

            // Get current admin user ID
            var adminIdClaim = User.FindFirst("id") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (adminIdClaim == null || !int.TryParse(adminIdClaim.Value, out var adminId))
                return Unauthorized("Không thể xác định người xử lý");

            report.Status = "Rejected";
            report.ResolvedAt = DateTime.UtcNow;
            report.ResolvedByUserId = adminId;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã từ chối báo cáo" });
        }

    }
}
