using Microsoft.AspNetCore.Http;

namespace RealEstateManagement.Business.Services.UploadPicService
{
    public interface IUploadPicService
    {
        Task<UploadResult> UploadImageAsync(IFormFile file, string folderName, string prefix = "");
        Task<bool> DeleteImageAsync(string imageUrl);
        bool IsValidImageFile(IFormFile file, long maxSizeInBytes = 5 * 1024 * 1024);
    }

    public class UploadResult
    {
        public bool Succeeded { get; set; }
        public string? ImageUrl { get; set; }
        public string? ErrorMessage { get; set; }

        public static UploadResult Success(string imageUrl)
        {
            return new UploadResult
            {
                Succeeded = true,
                ImageUrl = imageUrl,
                ErrorMessage = null
            };
        }

        public static UploadResult Failure(string errorMessage)
        {
            return new UploadResult
            {
                Succeeded = false,
                ImageUrl = null,
                ErrorMessage = errorMessage
            };
        }
    }
} 