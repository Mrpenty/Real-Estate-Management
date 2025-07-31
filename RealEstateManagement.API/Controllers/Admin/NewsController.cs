using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.API.Controllers.Landlord;
using RealEstateManagement.Business.DTO.News;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services.NewsService;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Business.Services.UploadPicService;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.API.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _service;

        public NewsController(INewsService service)
        {
            _service = service;
        }

        // Tạo mới (Draft)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NewsCreateDto dto)
        {
            var id = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        // Lấy tất cả bài viết (cả nháp lẫn đã xuất bản)
        [HttpGet("All-News")]
        public async Task<IActionResult> GetAll()
        {
            var news = await _service.GetAllAsync();
            return Ok(news);
        }

        // Lấy danh sách đã xuất bản
        [HttpGet("published")]
        public async Task<IActionResult> GetPublished()
        {
            var news = await _service.GetPublishedAsync();
            return Ok(news);
        }

        // Lấy bài viết theo id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var news = await _service.GetByIdAsync(id);
            if (news == null) return NotFound();
            return Ok(news);
        }

        // Lấy bài viết theo slug
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var news = await _service.GetBySlugAsync(slug);
            if (news == null) return NotFound();
            return Ok(news);
        }
        // Cập nhật bài viết
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NewsUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("Id mismatch");
            var result = await _service.UpdateAsync(dto);
            if (!result) return NotFound();
            return NoContent();
        }

        // Xuất bản bài viết
        [HttpPut("{id}/publish")]
        public async Task<IActionResult> Publish(int id)
        {
            var result = await _service.PublishAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        // Xóa bài viết
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }

}
