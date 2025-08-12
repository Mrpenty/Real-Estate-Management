using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Security.Claims;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Authorization;

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
        [EnableQuery]
        public IQueryable<OwnerPropertyDto> GetMyProperties()
        {
            var landlordId = GetCurrentLandlordId();
            return _ownerPropertyService.GetPropertiesByLandlordQueryable(landlordId);
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
        public async Task<IActionResult> UpdateProperty(int id, [FromBody] PropertyCreateRequestDto dto)
        {
            var landlordId = GetCurrentLandlordId();
            await _ownerPropertyService.UpdatePropertyAsync(dto, landlordId, id);
            return Ok(new { id });
        }

        // DELETE: Xoá BĐS
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var landlordId = GetCurrentLandlordId();
            await _ownerPropertyService.DeletePropertyAsync(id, landlordId);
            return NoContent();
        }

        //Gia hạn 1 bài đăng đã hết hạn
        [HttpPut("extend/{postId}/{landlordId}")]
        //[Authorize(Roles = "Landlord")]
        public async Task<IActionResult> ExtendPost(int postId, [FromQuery] int days, int landlordId)
        {
            var result = await _ownerPropertyService.ExtendPostAsync(postId, days, landlordId);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

    }
}
