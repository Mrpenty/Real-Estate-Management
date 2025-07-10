using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Business.Services.UploadPicService;
using Microsoft.Extensions.Logging;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.API.Controllers.Landlord
{
    [Route("api/properties/{propertyId}/images")]
    [ApiController]
    public class PropertyImagesController : ControllerBase
    {
        private readonly IPropertyImageService _imageService;
        private readonly IUploadPicService _uploadPicService;
        private readonly ILogger<PropertyImagesController> _logger;

        public PropertyImagesController(IPropertyImageService imageService, IUploadPicService uploadPicService, ILogger<PropertyImagesController> logger)
        {
            _imageService = imageService;
            _uploadPicService = uploadPicService;
            _logger = logger;
        }

        //Bước 2 của quá trình tạo bài đăng: Upload ảnh và đổi status từ Draft thành Pending
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImageFile(int propertyId, IFormFile file)
        {
            try
            {
                _logger.LogInformation("Starting upload for property {PropertyId}, file: {FileName}", propertyId, file?.FileName);

                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                var uploadResult = await _uploadPicService.UploadImageAsync(file, "property-images", $"property_{propertyId}");

                if (!uploadResult.Succeeded)
                {
                    _logger.LogError("Upload failed for property {PropertyId}: {Error}", propertyId, uploadResult.ErrorMessage);
                    return BadRequest(uploadResult.ErrorMessage);
                }

                _logger.LogInformation("Upload successful for property {PropertyId}, URL: {ImageUrl}", propertyId, uploadResult.ImageUrl);
                return Ok(new { imageUrl = uploadResult.ImageUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading image for property {PropertyId}", propertyId);
                return StatusCode(500, "An error occurred while uploading the image");
            }
        }

        //Thêm ảnh vào Database
        [HttpPost]
        public async Task<IActionResult> UploadImage(int propertyId, [FromBody] PropertyImageCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Saving image info for property {PropertyId}, URL: {ImageUrl}, IsPrimary: {IsPrimary}, Order: {Order}",
                    propertyId, dto.Url, dto.IsPrimary, dto.Order);

                var image = await _imageService.AddImageAsync(propertyId, dto);

                _logger.LogInformation("Image saved successfully for property {PropertyId}, ImageId: {ImageId}", propertyId, image.Id);
                return Ok(image);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Argument exception while saving image for property {PropertyId}: {Message}", propertyId, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while saving image for property {PropertyId}", propertyId);
                return StatusCode(500, "An error occurred while saving the image");
            }
        }

        //Cập nhật ảnh mới
        [HttpPut("{imageId}")]
        public async Task<IActionResult> UpdateImage(int propertyId, int imageId, [FromBody] PropertyImage dto)
        {
            if (dto.Id != imageId || dto.PropertyId != propertyId)
                return BadRequest("ID mismatch");

            var updated = await _imageService.UpdateImageAsync(dto);
            return Ok(updated);
        }

    }

}
