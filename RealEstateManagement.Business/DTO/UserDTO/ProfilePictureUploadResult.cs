using Microsoft.AspNetCore.Identity;

namespace RealEstateManagement.Business.DTO.UserDTO
{
    public class ProfilePictureUploadResult
    {
        public bool Succeeded { get; set; }
        public string? ImageUrl { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public static ProfilePictureUploadResult Success(string imageUrl)
        {
            return new ProfilePictureUploadResult
            {
                Succeeded = true,
                ImageUrl = imageUrl,
                Errors = new List<string>()
            };
        }

        public static ProfilePictureUploadResult Failure(IEnumerable<string> errors)
        {
            return new ProfilePictureUploadResult
            {
                Succeeded = false,
                ImageUrl = null,
                Errors = errors
            };
        }

        public static ProfilePictureUploadResult Failure(string error)
        {
            return new ProfilePictureUploadResult
            {
                Succeeded = false,
                ImageUrl = null,
                Errors = new List<string> { error }
            };
        }
    }
} 