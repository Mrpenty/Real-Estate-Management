using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.UserDTO;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Data.Entity;
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

                var result = await _profileService.UpdateProfileAsync(userId, model);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Profile updated successfully" });
                }

                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating profile");
                return StatusCode(500, "An error occurred while updating the profile");
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
