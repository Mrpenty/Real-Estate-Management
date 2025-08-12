using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/Property/[controller]")]
    [ApiController]
    public class InterestedPropertyController : ODataController
    {
        private readonly IInterestedPropertyService _service;

        public InterestedPropertyController(IInterestedPropertyService service)
        {
            _service = service;
        }

        /// <summary>
        /// OData: phân trang/filter/sort tự động (IQueryable)
        /// </summary>
        [EnableQuery(PageSize = 10, MaxTop = 50)]
        [HttpGet]
        public IQueryable<InterestedPropertyDTO> GetOdata()
        {
            return _service.QueryDTO();
        }

        // API thường phân trang thủ công
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? renterId = null)
        {
            var result = await _service.GetPaginatedAsync(page, pageSize, renterId);
            return Ok(result);
        }

        // Get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
                return NotFound();
            return Ok(entity);
        }

        /// <summary>
        /// Lấy danh sách quan tâm của 1 renter
        /// </summary>
        [HttpGet("ByRenter/{renterId}")]
        public async Task<ActionResult<IEnumerable<InterestedPropertyDTO>>> GetByRenter(int renterId)
        {
            var list = await _service.GetByRenterAsync(renterId);
            return Ok(list);
        }

        /// <summary>
        /// Tạo mới quan tâm (Add Interest)
        /// </summary>
        [HttpPost("AddInterest")]
        public async Task<ActionResult<InterestedPropertyDTO>> AddInterest([FromQuery] int renterId, [FromQuery] int propertyId)
        {
            try
            {
                var result = await _service.AddInterestAsync(renterId, propertyId);
                return Ok(result);
            }
            catch (InvalidOperationException ex) // LandlordRejected -> không cho quan tâm lại
            {
                return Conflict(new { message = ex.Message }); // 409
            }
        }

        /// <summary>
        /// Bỏ quan tâm (Remove Interest)
        /// </summary>
        [HttpDelete("RemoveInterest")]
        public async Task<IActionResult> RemoveInterest([FromQuery] int renterId, [FromQuery] int propertyId)
        {
            var result = await _service.RemoveInterestAsync(renterId, propertyId);
            if (!result)
                return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Cập nhật trạng thái quan tâm (Update Status)
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] InterestedStatus status)
        {
            try
            {
                await _service.UpdateStatusAsync(id, status);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> ConfirmInterest(int id, [FromQuery] bool isRenter, [FromQuery] bool confirmed)
        {
            try
            {
                await _service.ConfirmInterestAsync(id, isRenter, confirmed);
                return Ok(new { message = "Xác nhận thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
