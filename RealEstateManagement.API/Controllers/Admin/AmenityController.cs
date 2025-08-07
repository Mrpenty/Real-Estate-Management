using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.Repositories.AmenityRepo;
using RealEstateManagement.Data.Entity;
using System.Linq;
using RealEstateManagement.Business.DTO.AmenityDTO;

namespace RealEstateManagement.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityRepository _amenityRepository;
        public AmenityController(IAmenityRepository amenityRepository)
        {
            _amenityRepository = amenityRepository;
        }

        /// <summary>
        /// Get all amenities
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAmenities()
        {
            var amenities = await _amenityRepository.GetAsync();
            return Ok(new
            {
                message = "Lấy danh sách tiện ích thành công.",
                data = amenities
            });
        }

        /// <summary>
        /// Create a new amenity
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAmenity([FromBody] AmenityDto amenity)
        {
            if (amenity == null)
                return BadRequest(new { message = "Dữ liệu tiện ích không được để trống." });

            var entity = new Amenity
            {
                Name = amenity.Name,
                Description = amenity.Description
            };

            await _amenityRepository.AddAsync(entity);
            amenity.Id = entity.Id;

            return CreatedAtAction(nameof(GetAllAmenities), new { id = entity.Id }, new
            {
                message = "Thêm tiện ích thành công.",
                data = amenity
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAmenity(int id, [FromBody] AmenityDto amenity)
        {
            if (amenity == null || id != amenity.Id)
                return BadRequest(new { message = "Dữ liệu tiện ích không hợp lệ." });

            var existingAmenity = await _amenityRepository.GetByIdAsync(id);
            if (existingAmenity == null)
                return NotFound(new { message = "Không tìm thấy tiện ích." });

            existingAmenity.Name = amenity.Name;
            existingAmenity.Description = amenity.Description;
            await _amenityRepository.UpdateAsync(existingAmenity);

            return Ok(new { message = "Cập nhật tiện ích thành công." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmenity(int id)
        {
            var amenity = await _amenityRepository.GetByIdAsync(id);
            if (amenity == null)
                return NotFound(new { message = "Không tìm thấy tiện ích." });

            await _amenityRepository.DeleteAsync(id);
            return Ok(new { message = "Xóa tiện ích thành công." });
        }

        /// <summary>
        /// Get paged amenities
        /// </summary>
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResultDto<AmenityDto>>> GetPagedAmenities(int page = 1, int pageSize = 10)
        {
            var skip = (page - 1) * pageSize;
            var amenities = await _amenityRepository.GetPagedAsync(skip, pageSize);
            var totalCount = await _amenityRepository.GetTotalCountAsync();

            var pagedAmenities = amenities.Select(a => new AmenityDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description
            }).ToList();

            var result = new PagedResultDto<AmenityDto>
            {
                Items = pagedAmenities,
                TotalCount = totalCount
            };

            return Ok(new
            {
                message = "Lấy danh sách tiện ích phân trang thành công.",
                data = result
            });
        }
    }
}
