using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.SliderDTO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

[Route("api/admin/slider")]
[ApiController]
public class SliderController : ControllerBase
{
    private readonly ISliderService _sliderService;

    public SliderController(ISliderService sliderService)
    {
        _sliderService = sliderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var sliders = await _sliderService.GetAllAsync();
            var response = sliders.Select(s => new SliderResponseDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                ImageUrl = s.ImageUrl,
                CreatedAt = s.CreatedAt
            });
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var slider = await _sliderService.GetByIdAsync(id);
            if (slider == null) return NotFound(new { error = $"Slider with ID {id} not found." });

            var response = new SliderResponseDto
            {
                Id = slider.Id,
                Title = slider.Title,
                Description = slider.Description,
                ImageUrl = slider.ImageUrl,
                CreatedAt = slider.CreatedAt
            };
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateSliderDto dto,  IFormFile imageFile)
    {
        try
        {
            if (dto == null) return BadRequest(new { error = "DTO cannot be null." });
            if (imageFile == null) return BadRequest(new { error = "Image file is required." });

            var result = await _sliderService.AddAsync(dto, imageFile);
            var response = new SliderResponseDto
            {
                Id = result.Id,
                Title = result.Title,
                Description = result.Description,
                ImageUrl = result.ImageUrl,
                CreatedAt = result.CreatedAt
            };
            return CreatedAtAction(nameof(Get), new { id = result.Id }, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateSliderDto dto, IFormFile imageFile)
    {
        try
        {
            if (dto == null) return BadRequest(new { error = "DTO cannot be null." });
            await _sliderService.UpdateAsync(dto, imageFile);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _sliderService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}