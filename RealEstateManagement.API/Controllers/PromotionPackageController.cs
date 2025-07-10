using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.Services.PromotionPackages;
using RealEstateManagement.Business.DTO.PromotionPackageDTO;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionPackageController : ControllerBase
    {
        private readonly ILogger<PromotionPackageController> _logger;
        private readonly IPromotionPackageService _promotionPackageService;
        public PromotionPackageController(IPromotionPackageService promotionPackageService, ILogger<PromotionPackageController> logger)
        {
            _promotionPackageService = promotionPackageService;
            _logger = logger;
        }

        [HttpGet("GetAllPackage")]
        public async Task<IActionResult> GetAllPromotionPackages()
        {
            try
            {
                var packages = await _promotionPackageService.GetAllAsync();
                if (packages == null || !packages.Any())
                {
                    return NotFound("Không tìm thấy gói khuyến mãi nào.");
                }
                return Ok(packages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách gói khuyến mãi.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ.");
            }
        }
        [HttpGet("GetPackageById/{id}")]
        public async Task<IActionResult> GetPromotionPackageById(int id)
        {
            try
            {
                var package = await _promotionPackageService.GetByIdAsync(id);
                if (package == null)
                {
                    return NotFound($"Không tìm thấy gói khuyến mãi với ID {id}.");
                }
                return Ok(package);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy gói khuyến mãi theo ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ.");
            }
        }
        [HttpPost("CreatePackage")]
        public async Task<IActionResult> CreatePromotionPackage([FromBody] CreatePromotionPackageDTO packageDto)
        {
            if (packageDto == null)
            {
                return BadRequest("Dữ liệu gói khuyến mãi không hợp lệ.");
            }

            try
            {
                var created = await _promotionPackageService.CreateAsync(packageDto);
                return CreatedAtAction(nameof(GetPromotionPackageById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo gói khuyến mãi mới.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ.");
            }
        }
        [HttpPut("UpdatePackage/{id}")]
        public async Task<IActionResult> UpdatePromotionPackage(int id, [FromBody] UpdatePromotionPackageDTO packageDto)
        {
            if (packageDto == null)
            {
                return BadRequest("Dữ liệu gói khuyến mãi không hợp lệ.");
            }

            try
            {
                var updated = await _promotionPackageService.UpdateAsync(id, packageDto);
                if (updated == null)
                {
                    return NotFound($"Không tìm thấy gói khuyến mãi với ID {id} để cập nhật.");
                }
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật gói khuyến mãi.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ.");
            }
        }
        [HttpDelete("DeletePackage/{id}")]
        public async Task<IActionResult> DeletePromotionPackage(int id)
        {
            try
            {
                var deleted = await _promotionPackageService.DeleteAsync(id);
                if (!deleted)
                {
                    return NotFound($"Không tìm thấy gói khuyến mãi với ID {id} để xóa.");
                }
                return NoContent(); // Trả về 204 No Content nếu xóa thành công
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa gói khuyến mãi.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Lỗi máy chủ nội bộ.");
            }
        }
    }
}
