using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.UserDTO;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RealEstateManagement.Data.Entity.User;
namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfileService _profileService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager, IProfileService profileService, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _profileService = profileService;
            _logger = logger;
        }

        
        [HttpGet("Get-Profile")]
        public async Task<ActionResult<ViewProfileDto>> GetProfile()
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == 0)
                {
                    return Unauthorized("Invalid user token");
                }

                var profile = await _profileService.GetProfileAsync(userId);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting profile");
                return StatusCode(500, "An error occurred while retrieving the profile");
            }
        }

       
        [HttpPut("Update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetUserIdFromToken();
                if (userId == 0)
                {
                    return Unauthorized("Invalid user token");
                }

                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return NotFound("User not found");
                }

                var result = await _profileService.UpdateProfileAsync(userId, model);

                if (result.Succeeded)
                {
                    return Ok(new { 
                        message = "Cập nhật thông tin thành công!",
                        role = user.Role
                    });
                }

                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating profile");
                return StatusCode(500, "An error occurred while updating the profile");
            }
        }

        [HttpPost("request-verification")]
        public async Task<IActionResult> RequestVerification()
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId == 0)
                {
                    return Unauthorized("Invalid user token");
                }

                var result = await _profileService.RequestVerificationAsync(userId);

                if (result.Succeeded)
                {
                    return Ok(new { 
                        message = "Yêu cầu duyệt đã được gửi thành công! Hồ sơ của bạn đã được gửi để xác thực." 
                    });
                }

                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while requesting verification");
                return StatusCode(500, "An error occurred while requesting verification");
            }
        }

        /// <summary>
        /// Get user profile by ID (Admin only)
        /// </summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ViewProfileDto>> GetUserProfile(int id)
        {
            try
            {
                var profile = await _profileService.GetProfileAsync(id);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting profile for user {UserId}", id);
                return StatusCode(500, "An error occurred while retrieving the profile");
            }
        }

        
        [HttpPost("profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded");
                }

                var userId = GetUserIdFromToken();
                if (userId == 0)
                {
                    return Unauthorized("Invalid user token");
                }

                var result = await _profileService.UploadProfilePictureAsync(userId, file);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Profile picture uploaded successfully", imageUrl = result.ImageUrl });
                }

                return BadRequest(new { errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading profile picture");
                return StatusCode(500, "An error occurred while uploading the profile picture");
            }
        }

       
        [HttpPut("admin-update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminUpdateUser(int id, [FromBody] UpdateProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound("User not found");

            bool roleChanged = false;
            if (!string.IsNullOrEmpty(dto.Role) && dto.Role != user.Role)
            {
                user.Role = dto.Role;
                roleChanged = true;
            }

            if (dto.IsVerified.HasValue) user.IsVerified = dto.IsVerified.Value;
            if (dto.IsActive.HasValue) user.IsActive = dto.IsActive.Value;
            if (!string.IsNullOrEmpty(dto.Name)) user.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Email)) user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.PhoneNumber)) user.PhoneNumber = dto.PhoneNumber;
            if (!string.IsNullOrEmpty(dto.CitizenIdNumber)) user.CitizenIdNumber = dto.CitizenIdNumber;
            if (!string.IsNullOrEmpty(dto.CitizenIdFrontImageUrl)) user.CitizenIdFrontImageUrl = dto.CitizenIdFrontImageUrl;
            if (!string.IsNullOrEmpty(dto.CitizenIdBackImageUrl)) user.CitizenIdBackImageUrl = dto.CitizenIdBackImageUrl;
            if (dto.CitizenIdIssuedDate.HasValue) user.CitizenIdIssuedDate = dto.CitizenIdIssuedDate;
            if (dto.CitizenIdExpiryDate.HasValue) user.CitizenIdExpiryDate = dto.CitizenIdExpiryDate;
            if (!string.IsNullOrEmpty(dto.VerificationRejectReason)) user.VerificationRejectReason = dto.VerificationRejectReason;
            if (!string.IsNullOrEmpty(dto.VerificationStatus)) user.VerificationStatus = dto.VerificationStatus;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // If role changed, update ASP.NET Core Identity roles and invalidate tokens
                if (roleChanged)
                {
                    // Update ASP.NET Core Identity roles
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    if (currentRoles.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    }
                    await _userManager.AddToRoleAsync(user, dto.Role);

                    // Invalidate existing refresh tokens for this user
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = null;
                    await _userManager.UpdateAsync(user);

                    return Ok(new { 
                        message = "User updated successfully",
                        note = "Role changed - user will need to login again to get new token with updated role"
                    });
                }

                return Ok(new { message = "User updated successfully" });
            }
            return BadRequest(result.Errors);
        }

     
        [HttpGet("admin-list")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdminUserList(string search = "", string role = "", bool? isActive = null, int page = 1, int pageSize = 10)
        {
            var users = _userManager.Users
                .Where(u => u.Role == "renter" || u.Role == "landlord");

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.Name.Contains(search) || u.PhoneNumber.Contains(search));
            }
            if (!string.IsNullOrEmpty(role))
            {
                users = users.Where(u => u.Role == role);
            }
            if (isActive.HasValue)
            {
                users = users.Where(u => u.IsActive == isActive.Value);
            }

            int totalUsers = users.Count();
            var userList = users
                .OrderByDescending(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new {
                    u.Id,
                    u.Name,
                    u.PhoneNumber,
                    u.Email,
                    u.Role,
                    u.IsActive,
                    u.CreatedAt,
                    u.VerificationStatus
                })
                .ToList();

            return Ok(new {
                totalUsers,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalUsers / pageSize),
                users = userList
            });
        }

        [HttpPost("upload-cccd-front")]
        public async Task<IActionResult> UploadCccdFront(IFormFile file)
        {
            var userId = GetUserIdFromToken();
            if (userId == 0) return Unauthorized("Invalid user token");
            if (file == null || file.Length == 0) return BadRequest("No file uploaded");

            var result = await _profileService.UploadCccdImageAsync(userId, file, true);
            if (result.Succeeded)
                return Ok(new { message = "Uploaded successfully", imageUrl = result.ImageUrl });
            return BadRequest(new { errors = result.Errors });
        }

        [HttpPost("upload-cccd-back")]
        public async Task<IActionResult> UploadCccdBack(IFormFile file)
        {
            var userId = GetUserIdFromToken();
            if (userId == 0) return Unauthorized("Invalid user token");
            if (file == null || file.Length == 0) return BadRequest("No file uploaded");

            var result = await _profileService.UploadCccdImageAsync(userId, file, false);
            if (result.Succeeded)
                return Ok(new { message = "Uploaded successfully", imageUrl = result.ImageUrl });
            return BadRequest(new { errors = result.Errors });
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = GetUserIdFromToken();
                if (userId == 0)
                {
                    return Unauthorized("Invalid user token");
                }
                var result = await _profileService.ResetPasswordAsync(userId, model);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Password reset successfully" });
                }
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while resetting password");
                return StatusCode(500, "An error occurred while resetting the password");
            }
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var nameIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var subClaim = User.FindFirst("sub")?.Value;

            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            else if (!string.IsNullOrEmpty(nameIdClaim) && int.TryParse(nameIdClaim, out userId))
            {
                return userId;
            }
            else if (!string.IsNullOrEmpty(subClaim) && int.TryParse(subClaim, out userId))
            {
                return userId;
            }

            return 0;
        }

    }
}
