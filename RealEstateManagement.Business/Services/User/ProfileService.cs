using Microsoft.AspNetCore.Identity;
using RealEstateManagement.Business.DTO.UserDTO;
using RealEstateManagement.Data.Entity.User;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.Services.UploadPicService;

namespace RealEstateManagement.Business.Services.User
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ProfileService> _logger;
        private readonly IUploadPicService _uploadPicService;

        public ProfileService(UserManager<ApplicationUser> userManager, ILogger<ProfileService> logger, IUploadPicService uploadPicService)
        {
            _userManager = userManager;
            _logger = logger;
            _uploadPicService = uploadPicService;
        }

        public async Task<ViewProfileDto> GetProfileAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "User";

            var profile = new ViewProfileDto
            {
                Id = user.Id,
                Name = user.Name,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Email = user.Email ?? string.Empty,
                Role = role,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                IsVerified = user.IsVerified,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                CreatedAt = user.CreatedAt,
                
            };

            return profile;
        }

        public async Task<IdentityResult> UpdateProfileAsync(int userId, UpdateProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (!string.IsNullOrEmpty(model.Email))
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null && existingUser.Id != userId)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Email is already taken by another user" });
                }
            }

            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber && u.Id != userId);
                if (existingUser != null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Phone number is already taken by another user" });
                }
            }

            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = model.Email; 
            user.PhoneNumber = model.PhoneNumber;

            if (!string.IsNullOrEmpty(model.ProfilePictureUrl))
            {
                user.ProfilePictureUrl = model.ProfilePictureUrl;
            }

            var emailChanged = !string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase);
            if (emailChanged)
            {
                user.EmailConfirmed = false;
            }

            var phoneChanged = !string.Equals(user.PhoneNumber, model.PhoneNumber, StringComparison.OrdinalIgnoreCase);
            if (phoneChanged)
            {
                user.PhoneNumberConfirmed = false;
            }

            return await _userManager.UpdateAsync(user);
        }

        public async Task<ProfilePictureUploadResult> UploadProfilePictureAsync(int userId, IFormFile file)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return ProfilePictureUploadResult.Failure("User not found");
                }

                var uploadResult = await _uploadPicService.UploadImageAsync(file, "profile-pictures", $"profile_{userId}");

                if (!uploadResult.Succeeded)
                {
                    return ProfilePictureUploadResult.Failure(uploadResult.ErrorMessage ?? "Upload failed");
                }

                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    await _uploadPicService.DeleteImageAsync(user.ProfilePictureUrl);
                }

                user.ProfilePictureUrl = uploadResult.ImageUrl;
                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    return ProfilePictureUploadResult.Success(uploadResult.ImageUrl!);
                }
                else
                {
                    return ProfilePictureUploadResult.Failure(updateResult.Errors.Select(e => e.Description));
                }
            }
            catch (Exception ex)
            {
                return ProfilePictureUploadResult.Failure("An error occurred while uploading the profile picture");
               
            }
        }
    }
}
