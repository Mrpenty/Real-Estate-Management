using Microsoft.AspNetCore.Identity;
using RealEstateManagement.Business.DTO.UserDTO;
using RealEstateManagement.Data.Entity.User;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.Services.UploadPicService;
using RealEstateManagement.Data.Entity.User;

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
                Role = user.Role,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                IsVerified = user.IsVerified,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
                CitizenIdFrontImageUrl = user.CitizenIdFrontImageUrl,
                CitizenIdBackImageUrl = user.CitizenIdBackImageUrl,
                VerificationRejectReason = user.VerificationRejectReason,
                CitizenIdNumber = user.CitizenIdNumber,
                CitizenIdIssuedDate = user.CitizenIdIssuedDate,
                CitizenIdExpiryDate = user.CitizenIdExpiryDate,
                VerificationStatus = user.VerificationStatus
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

            if (!string.IsNullOrEmpty(model.CitizenIdNumber))
            {
                user.CitizenIdNumber = model.CitizenIdNumber;
            }
            if (model.CitizenIdIssuedDate.HasValue)
            {
                user.CitizenIdIssuedDate = model.CitizenIdIssuedDate;
            }
            if (model.CitizenIdExpiryDate.HasValue)
            {
                user.CitizenIdExpiryDate = model.CitizenIdExpiryDate;
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

        public async Task<IdentityResult> RequestVerificationAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (user.Role != "renter")
            {
                return IdentityResult.Failed(new IdentityError { Description = "Chỉ người dùng renter mới có thể yêu cầu duyệt" });
            }

            // Kiểm tra xem có đủ thông tin không
            var hasAllRequiredFields = !string.IsNullOrEmpty(user.CitizenIdNumber) &&
                                     user.CitizenIdIssuedDate.HasValue &&
                                     user.CitizenIdExpiryDate.HasValue &&
                                     !string.IsNullOrEmpty(user.CitizenIdFrontImageUrl) &&
                                     !string.IsNullOrEmpty(user.CitizenIdBackImageUrl);

            if (!hasAllRequiredFields)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Vui lòng điền đầy đủ thông tin CCCD trước khi yêu cầu duyệt" });
            }

            // Chỉ cho phép yêu cầu duyệt nếu trạng thái là none hoặc rejected
            if (user.VerificationStatus != "none" && user.VerificationStatus != "rejected")
            {
                return IdentityResult.Failed(new IdentityError { Description = "Không thể yêu cầu duyệt với trạng thái hiện tại" });
            }

            user.VerificationStatus = "pending";
            user.VerificationRejectReason = null; // Xóa lý do từ chối cũ

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

        public async Task<ProfilePictureUploadResult> UploadCccdImageAsync(int userId, IFormFile file, bool isFront)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return ProfilePictureUploadResult.Failure("User not found");

            var folder = isFront ? "cccd-front" : "cccd-back";
            var prefix = isFront ? $"cccd_front_{userId}" : $"cccd_back_{userId}";
            var uploadResult = await _uploadPicService.UploadImageAsync(file, folder, prefix);

            if (!uploadResult.Succeeded)
                return ProfilePictureUploadResult.Failure(uploadResult.ErrorMessage ?? "Upload failed");

            // Xóa ảnh cũ nếu có
            if (isFront && !string.IsNullOrEmpty(user.CitizenIdFrontImageUrl))
                await _uploadPicService.DeleteImageAsync(user.CitizenIdFrontImageUrl);
            if (!isFront && !string.IsNullOrEmpty(user.CitizenIdBackImageUrl))
                await _uploadPicService.DeleteImageAsync(user.CitizenIdBackImageUrl);

            if (isFront)
                user.CitizenIdFrontImageUrl = uploadResult.ImageUrl;
            else
                user.CitizenIdBackImageUrl = uploadResult.ImageUrl;

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
                return ProfilePictureUploadResult.Success(uploadResult.ImageUrl!);
            else
                return ProfilePictureUploadResult.Failure(updateResult.Errors.Select(e => e.Description));
        }

        public async Task<IdentityResult> ResetPasswordAsync(int userId, ResetPasswordDto model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return result;
        }

        //Lấy tên, số điện thoại , email của user
        public async Task<UserBasicInfoDto> GetUserBasicInfoAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return new UserBasicInfoDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber ?? string.Empty
            };
        }
    }
}
