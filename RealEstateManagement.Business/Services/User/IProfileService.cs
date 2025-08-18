using Microsoft.AspNetCore.Identity;
using RealEstateManagement.Business.DTO.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RealEstateManagement.Business.Services.User
{
    public interface IProfileService
    {
        Task<ViewProfileDto> GetProfileAsync(int userId);
        Task<IdentityResult> UpdateProfileAsync(int userId, UpdateProfileDto model);
        Task<IdentityResult> RequestVerificationAsync(int userId);
        Task<ProfilePictureUploadResult> UploadProfilePictureAsync(int userId, IFormFile file);
        Task<ProfilePictureUploadResult> UploadCccdImageAsync(int userId, IFormFile file, bool isFront);
        Task<IdentityResult> ResetPasswordAsync(int userId, ResetPasswordDto model);

        Task<UserBasicInfoDto> GetUserBasicInfoAsync(int userId);
    }
}
