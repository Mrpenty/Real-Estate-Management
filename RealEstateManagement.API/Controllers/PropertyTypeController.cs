using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.PropertyTypeDTO;
using RealEstateManagement.Business.Repositories.PropertyTypeRepository;
using RealEstateManagement.Business.Services.PropertyTypeService;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyTypeController : ControllerBase
    {
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly ILogger<PropertyTypeController> _logger;
        public PropertyTypeController(IPropertyTypeService propertyTypeService, ILogger<PropertyTypeController> logger)
        {
            _propertyTypeService = propertyTypeService;
            _logger = logger;
        }
        [HttpGet("GetAllPropertyTypes")]
        public async Task<IActionResult> GetAllPropertyTypes()
        {
            try
            {
                var propertyTypes = await _propertyTypeService.GetAllAsync();
                if (propertyTypes == null || !propertyTypes.Any())
                {
                    return NotFound("No property types found.");
                }
                return Ok(propertyTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving property types.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpGet("GetPropertyTypeById/{id}")]
        public async Task<IActionResult> GetPropertyTypeById(int id)
        {
            try
            {
                var propertyType = await _propertyTypeService.GetByIdAsync(id);
                if (propertyType == null)
                {
                    return NotFound($"No property type found with ID {id}.");
                }
                return Ok(propertyType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving property type by ID.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpPost("CreatePropertyType")]
        public async Task<IActionResult> CreatePropertyType([FromBody] PropertyTypeGet propertyTypeDto)
        {
            if (propertyTypeDto == null)
            {
                return BadRequest("Property type data is null.");
            }
            try
            {
                var createdPropertyType = await _propertyTypeService.CreateAsync(propertyTypeDto);
                return CreatedAtAction(nameof(GetPropertyTypeById), new { id = createdPropertyType.Id }, createdPropertyType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating property type.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpPut("UpdatePropertyType/{id}")]
        public async Task<IActionResult> UpdatePropertyType(int id, [FromBody] PropertyTypeGet propertyTypeDto)
        {
            if (propertyTypeDto == null || id != propertyTypeDto.Id)
            {
                return BadRequest("Invalid property type data.");
            }
            try
            {
                var updatedPropertyType = await _propertyTypeService.UpdateAsync(id, propertyTypeDto);
                if (updatedPropertyType == null)
                {
                    return NotFound($"No property type found with ID {id} to update.");
                }
                return Ok(updatedPropertyType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating property type.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }

        [HttpDelete("DeletePropertyType/{id}")]
        public async Task<IActionResult> DeletePropertyType(int id)
        {
            try
            {
                var existingPropertyType = await _propertyTypeService.GetByIdAsync(id);
                if (existingPropertyType == null)
                {
                    return NotFound($"No property type found with ID {id} to delete.");
                }
                await _propertyTypeService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting property type.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
            }
        }
    }
}
