using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services.OwnerService;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/properties/{propertyId}/images")]
    [ApiController]
    public class PropertyImagesController : ControllerBase
    {
        private readonly IPropertyImageService _imageService;

        public PropertyImagesController(IPropertyImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(int propertyId, [FromBody] PropertyImageCreateDto dto)
        {
            try
            {
                var image = await _imageService.AddImageAsync(propertyId, dto);
                return Ok(image);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

}
