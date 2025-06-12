using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.AuthDTO;
using RealEstateManagement.Business.Repositories;
using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
     
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAuthService authService, ILogger<AuthController> logger, IConfiguration configuration)
        {

            _authService = authService;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.LoginAsync(loginDTO);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.RegisterAsync(registerDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                await _authService.LogoutAsync();
                return Ok(new { Message = "Logged out successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging out.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtpAsync([FromBody] VerifyOtpDTO verifyOtpDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.VerifyOtpAsync(verifyOtpDTO.PhoneNumber, verifyOtpDTO.Otp);
                if (!response.IsAuthSuccessful)
                {
                    return BadRequest(response.ErrorMessage);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while verifying OTP.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("google-login")]
        [Authorize(Roles = "Renter")]
        public async Task<IActionResult> GoogleLoginAsync([FromBody] GoogleLoginDTO googleLoginDTO)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(googleLoginDTO.IdToken))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.GoogleLoginAsync(googleLoginDTO.IdToken);
                if (response.IsAuthSuccessful)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during Google login.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("verify-email")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> VerifyEmailConfirmationAsync([FromBody] VerifyEmailDTO verifyEmailDTO)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(verifyEmailDTO.Email) || string.IsNullOrEmpty(verifyEmailDTO.Code))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.VerifyEmailConfirmationAsync(verifyEmailDTO.Email, verifyEmailDTO.Code);
                if (response.IsAuthSuccessful)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while verifying email.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

       

    }
}