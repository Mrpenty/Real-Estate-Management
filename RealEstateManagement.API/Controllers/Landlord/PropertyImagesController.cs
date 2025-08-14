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

                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                var uploadResult = await _uploadPicService.UploadImageAsync(file, "property-images", $"property_{propertyId}");

                if (!uploadResult.Succeeded)
                {
                    return BadRequest(uploadResult.ErrorMessage);
                }

                return Ok(new { imageUrl = uploadResult.ImageUrl });
            }
            catch (Exception ex)
            {
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

        //Xóa tất cả ảnh của một property
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearAllImages(int propertyId)
        {
            try
            {
                _logger.LogInformation("Clearing all images for property {PropertyId}", propertyId);
                
                var result = await _imageService.ClearAllImagesAsync(propertyId);
                
                if (result)
                {
                    _logger.LogInformation("Successfully cleared all images for property {PropertyId}", propertyId);
                    return Ok(new { message = "All images cleared successfully" });
                }
                else
                {
                    _logger.LogWarning("Failed to clear images for property {PropertyId}", propertyId);
                    return BadRequest("Failed to clear images");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing images for property {PropertyId}", propertyId);
                return StatusCode(500, "An error occurred while clearing images");
            }
        }

        //Xóa một ảnh cụ thể
        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(int propertyId, int imageId)
        {
            try
            {
                _logger.LogInformation("Deleting image {ImageId} for property {PropertyId}", imageId, propertyId);
                
                var result = await _imageService.DeleteImageAsync(propertyId, imageId);
                
                if (result)
                {
                    _logger.LogInformation("Successfully deleted image {ImageId} for property {PropertyId}", imageId, propertyId);
                    return Ok(new { message = "Image deleted successfully" });
                }
                else
                {
                    _logger.LogWarning("Failed to delete image {ImageId} for property {PropertyId}", imageId, propertyId);
                    return NotFound("Image not found or could not be deleted");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image {ImageId} for property {PropertyId}", imageId, propertyId);
                return StatusCode(500, "An error occurred while deleting image");
            }
        }

    }

}
