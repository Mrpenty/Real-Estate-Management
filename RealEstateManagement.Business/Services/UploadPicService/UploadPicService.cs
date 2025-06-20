using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace RealEstateManagement.Business.Services.UploadPicService
{
    public class UploadPicService : IUploadPicService
    {
        private readonly ILogger<UploadPicService> _logger;

        public UploadPicService(ILogger<UploadPicService> logger)
        {
            _logger = logger;
        }

        public async Task<UploadResult> UploadImageAsync(IFormFile file, string folderName, string prefix = "")
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return UploadResult.Failure("No file uploaded");
                }

                if (!IsValidImageFile(file))
                {
                    return UploadResult.Failure("Invalid file type or size. Only JPG, JPEG, PNG, and GIF files up to 5MB are allowed.");
                }

                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                var fileExtension = Path.GetExtension(file.FileName);
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var fileName = string.IsNullOrEmpty(prefix) 
                    ? $"image_{timestamp}{fileExtension}"
                    : $"{prefix}_{timestamp}{fileExtension}";
                
                var filePath = Path.Combine(uploadsDir, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var imageUrl = $"/uploads/{folderName}/{fileName}";

                _logger.LogInformation("Image uploaded successfully: {ImageUrl}", imageUrl);
                return UploadResult.Success(imageUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading image");
                return UploadResult.Failure("An error occurred while uploading the image");
            }
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                {
                    return false;
                }

                var relativePath = imageUrl.TrimStart('/');
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    _logger.LogInformation("Image deleted successfully: {ImageUrl}", imageUrl);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting image: {ImageUrl}", imageUrl);
                return false;
            }
        }

        public bool IsValidImageFile(IFormFile file, long maxSizeInBytes = 5 * 1024 * 1024)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            if (file.Length > maxSizeInBytes)
            {
                return false;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            return allowedExtensions.Contains(fileExtension);
        }
    }
} 