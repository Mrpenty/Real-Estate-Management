using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.ReportDTO;
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

        public ReportController(RentalDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("post/{userRepoertId}")]
        public async Task<IActionResult> ReportPost([FromBody] ReportCreateDto dto, int userRepoertId)
        {

            // Kiểm tra đã report chưa
            var existing = await _context.Reports.FirstOrDefaultAsync(r =>
                r.TargetId == dto.TargetId &&
                r.TargetType == "PropertyPost" &&
                r.ReportedByUserId == userRepoertId);

            if (existing != null)
                return BadRequest("Bạn đã report bài đăng này rồi.");

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
            return Ok("Đã gửi report.");
        }

        [HttpDelete("post/{targetId}/{userRepoertId}")]
        public async Task<IActionResult> CancelPostReport(int targetId, int userRepoertId)
        {
            var report = await _context.Reports.FirstOrDefaultAsync(r =>
                r.TargetId == targetId &&
                r.TargetType == "PropertyPost" &&
                r.ReportedByUserId == userRepoertId);

            if (report == null)
                return NotFound("Bạn chưa report bài đăng này.");

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return Ok("Đã hủy report.");
        }

        [HttpPost("user/{userRepoertId}")]
        public async Task<IActionResult> ReportUser([FromBody] ReportCreateDto dto, int userRepoertId)
        {
            var existing = await _context.Reports.FirstOrDefaultAsync(r =>
                r.TargetId == dto.TargetId &&
                r.TargetType == "User" &&
                r.ReportedByUserId == userRepoertId);

            if (existing != null)
                return BadRequest("Bạn đã report người dùng này rồi.");

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
            return Ok("Đã gửi report người dùng.");
        }

        [HttpDelete("user/{targetId}/{userRepoertId}")]
        public async Task<IActionResult> CancelUserReport(int targetId, int userRepoertId)
        {

            var report = await _context.Reports.FirstOrDefaultAsync(r =>
                r.TargetId == targetId &&
                r.TargetType == "User" &&
                r.ReportedByUserId == userRepoertId);

            if (report == null)
                return NotFound("Bạn chưa report người dùng này.");

            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return Ok("Đã hủy report người dùng.");
        }

        [HttpGet("admin/all")]
       // [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminViewAllReport(
    [FromQuery] string? targetType,
    [FromQuery] string? status,
    [FromQuery] string? keyword)
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

            var results = await query
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
                    ReportedBy = new { r.ReportedByUser.Id, r.ReportedByUser.Name },
                    ResolvedBy = r.ResolvedByUser == null ? null : new { r.ResolvedByUser.Id, r.ResolvedByUser.Name },
                    r.ResolvedAt,
                    r.AdminNote
                })
                .ToListAsync();

            return Ok(results);
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

    }
}
