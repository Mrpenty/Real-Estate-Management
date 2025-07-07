using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers.Landlord
{
    [Authorize(Roles = "Landlord")]
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerPropertyController : ControllerBase
    {
        private readonly IOwnerPropertyService _ownerPropertyService;

        public OwnerPropertyController(IOwnerPropertyService ownerPropertyService)
        {
            _ownerPropertyService = ownerPropertyService;
        }

        // Lấy ID landlord từ JWT
        private int GetCurrentLandlordId()
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Không tìm thấy thông tin người dùng");

            if (!int.TryParse(userIdClaim.Value, out var landlordId))
                throw new UnauthorizedAccessException("ID người dùng không hợp lệ");

            return landlordId;
        }

        // READ: Lấy danh sách BĐS của landlord
        [HttpGet]
        public async Task<IActionResult> GetMyProperties()
        {
            var landlordId = GetCurrentLandlordId();
            var properties = await _ownerPropertyService.GetPropertiesByLandlordAsync(landlordId);
            return Ok(properties);
        }

        // READ: Lấy chi tiết 1 BĐS của landlord
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProperty(int id)
        {
            var landlordId = GetCurrentLandlordId();
            var property = await _ownerPropertyService.GetPropertyByIdAsync(id, landlordId);
            return Ok(property);
        }

        // UPDATE: Cập nhật BĐS
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProperty([FromQuery] int id, [FromBody] OwnerUpdatePropertyDto dto)
        {
            var landlordId = GetCurrentLandlordId();
            dto.Id = id; // Bắt buộc gán lại để fix ID từ route
            await _ownerPropertyService.UpdatePropertyAsync(dto, landlordId);
            return Ok(new { id });
        }


        // DELETE: Xoá BĐS
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty([FromQuery] int id)
        {
            var landlordId = GetCurrentLandlordId();
            await _ownerPropertyService.DeletePropertyAsync(id, landlordId);
            return NoContent();
        }

    }
}
