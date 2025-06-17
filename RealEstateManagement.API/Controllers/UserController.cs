using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.UserDTO;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProfileService _profileService;
        public UserController(UserManager<ApplicationUser> userManager, IProfileService profileService)
        {
            _userManager = userManager;
            _profileService = profileService;
        }
        [HttpGet]
         
        public async Task<ActionResult<ViewProfileDto>> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
            var profile = await _profileService.GetProfileAsync(userId);
            return Ok(profile);
        }
        [HttpPut]
        
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _profileService.UpdateProfileAsync(userId, model);

            if (result.Succeeded)
            {
                return Ok(new { message = "Profile updated successfully" });
            }

            return BadRequest(result.Errors);
        }
    }
}
