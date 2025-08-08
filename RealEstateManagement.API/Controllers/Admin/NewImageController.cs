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
    [Route("api/[controller]")]
    [ApiController]
    public class NewImageController : ControllerBase
    {
        private readonly INewImageService _imageService;
        private readonly IUploadPicService _uploadPicService;
        private readonly ILogger<NewImageController> _logger;

        public NewImageController(INewImageService imageService, IUploadPicService uploadPicService, ILogger<NewImageController> logger)
        {
            _imageService = imageService;
            _uploadPicService = uploadPicService;
            _logger = logger;
        }

        // Lấy danh sách ảnh của một bài tin tức
        [HttpGet]
        public async Task<IActionResult> GetImagesByNewsId(int newId)
        {
            try
            {
                var images = await _imageService.GetImagesByNewsIdAsync(newId);
                return Ok(images);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting images for new {newId}", newId);
                return StatusCode(500, "An error occurred while getting images");
            }
        }

        //Bước 2 của quá trình tạo bài đăng: Upload ảnh và đổi status từ Draft thành Pending
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImageFile(int newId, IFormFile file)
        {
            try
            {
                _logger.LogInformation("Starting upload for new {newId}, file: {FileName}", newId, file?.FileName);

                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                var uploadResult = await _uploadPicService.UploadImageAsync(file, "new-images", $"new_{newId}");

                if (!uploadResult.Succeeded)
                {
                    _logger.LogError("Upload failed for new {newId}: {Error}", newId, uploadResult.ErrorMessage);
                    return BadRequest(uploadResult.ErrorMessage);
                }

                _logger.LogInformation("Upload successful for new {newId}, URL: {ImageUrl}", newId, uploadResult.ImageUrl);
                return Ok(new { imageUrl = uploadResult.ImageUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading image for new {newId}", newId);
                return StatusCode(500, "An error occurred while uploading the image");
            }
        }

        //Thêm ảnh vào Database
        [HttpPost]
        public async Task<IActionResult> UploadImage(int newId, [FromBody] NewImageCreateDto dto)
        {
            try
            {
                _logger.LogInformation("Saving image info for newId {newId}, URL: {ImageUrl}, IsPrimary: {IsPrimary}, Order: {Order}",
                    newId, dto.ImageUrl, dto.IsPrimary, dto.Order);

                var image = await _imageService.AddImageAsync(newId, dto);

                _logger.LogInformation("Image saved successfully for newId {newId}, ImageId: {ImageId}", newId, image.Id);
                return Ok(image);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError("Argument exception while saving image for new {newId}: {Message}", newId, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while saving image for new {newId}", newId);
                return StatusCode(500, "An error occurred while saving the image");
            }
        }

        //Cập nhật ảnh mới
        [HttpPut("{imageId}")]
        public async Task<IActionResult> UpdateImage(int newId, int imageId, [FromBody] NewsImage dto)
        {
            if (dto.Id != imageId || dto.NewsId != newId)
                return BadRequest("ID mismatch");

            var updated = await _imageService.UpdateImageAsync(dto);
            return Ok(updated);
        }

        //Xóa ảnh
        [HttpDelete("{imageId}")]
        public async Task<IActionResult> DeleteImage(int newId, int imageId)
        {
            try
            {
                _logger.LogInformation("Deleting image {imageId} for new {newId}", imageId, newId);
                
                var result = await _imageService.DeleteImageAsync(newId, imageId);
                if (!result)
                {
                    _logger.LogWarning("Image {imageId} not found for new {newId}", imageId, newId);
                    return NotFound("Không tìm thấy ảnh");
                }

                _logger.LogInformation("Image {imageId} deleted successfully for new {newId}", imageId, newId);
                return Ok(new { message = "Xóa ảnh thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting image {imageId} for new {newId}", imageId, newId);
                return StatusCode(500, "An error occurred while deleting the image");
            }
        }

        //Xóa file vật lý
        [HttpPost("delete-file")]
        public async Task<IActionResult> DeleteImageFile([FromBody] DeleteImageFileRequest request)
        {
            try
            {
                _logger.LogInformation("Deleting image file: {ImageUrl}", request.ImageUrl);
                
                var result = await _uploadPicService.DeleteImageAsync(request.ImageUrl);
                if (result)
                {
                    _logger.LogInformation("Image file deleted successfully: {ImageUrl}", request.ImageUrl);
                    return Ok(new { message = "Xóa file thành công" });
                }
                else
                {
                    _logger.LogWarning("Image file not found: {ImageUrl}", request.ImageUrl);
                    return NotFound("Không tìm thấy file");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting image file: {ImageUrl}", request.ImageUrl);
                return StatusCode(500, "An error occurred while deleting the image file");
            }
        }
    }

    public class DeleteImageFileRequest
    {
        public string ImageUrl { get; set; } = string.Empty;
    }
    
}
