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
using RealEstateManagement.Business.Repositories.Token;
using RealEstateManagement.Business.Services.Auth;
using RealEstateManagement.Data.Entity.User;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
     
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ITokenRepository _tokenRepository;

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
                //loginDTO.LoginIdentifier = loginDTO.LoginIdentifier.Replace("+84", "0");
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


        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpRequest request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest(new { success = false, message = "Phone number is required" });
            }

            var result = await _authService.ResendOtpAsync(request.PhoneNumber);

            if (result.IsAuthSuccessful)
            {
                return Ok(new { success = true, message = result.ErrorMessage });
            }

            return BadRequest(new { success = false, message = result.ErrorMessage });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO request)
        {
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return BadRequest(new { success = false, message = "Phone number is required" });
            }

            try
            {
                var result = await _authService.ForgotPasswordAsync(request.PhoneNumber);
                
                if (result.IsAuthSuccessful)
                {
                    return Ok(new { success = true, message = result.ErrorMessage });
                }
                
                return BadRequest(new { success = false, message = result.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing forgot password request.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("verify-otp-for-password-reset")]
        public async Task<IActionResult> VerifyOtpForPasswordReset([FromBody] VerifyOtpDTO verifyOtpDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.VerifyOtpForPasswordResetAsync(verifyOtpDTO.PhoneNumber, verifyOtpDTO.Otp);
                
                if (response.IsAuthSuccessful)
                {
                    return Ok(new { success = true, message = response.ErrorMessage });
                }
                
                return BadRequest(new { success = false, message = response.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while verifying OTP for password reset.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("reset-password-with-otp")]
        public async Task<IActionResult> ResetPasswordWithOTP([FromBody] ResetPasswordWithOTPDTO resetPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.ResetPasswordWithOTPAsync(resetPasswordDTO);
                
                if (response.IsAuthSuccessful)
                {
                    return Ok(new { success = true, message = response.ErrorMessage });
                }
                
                return BadRequest(new { success = false, message = response.ErrorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while resetting password with OTP.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [HttpPost("google-login")]
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
        public async Task<IActionResult> VerifyEmailConfirmationAsync([FromBody] VerifyEmailDTO verifyEmailDTO)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(verifyEmailDTO.Email))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = await _authService.VerifyEmailConfirmationAsync(verifyEmailDTO.Email);
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

        [HttpGet("google-oauth-callback")]
        public async Task<IActionResult> GoogleOAuthCallback([FromQuery] string code)
        {
            
            var redirectUriForGoogle = Url.ActionLink("GoogleOAuthCallback", "Auth", null, Request.Scheme);

            try
            {
                var response = await _authService.HandleGoogleOAuthCallbackAsync(code, redirectUriForGoogle);
                if (response.IsAuthSuccessful)
                {
                    return Redirect("http://194.233.81.64:5001/");
                }
                else
                {
                    return Redirect($"http://194.233.81.64:5001/Auth/Login?error={Uri.EscapeDataString(response.ErrorMessage)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Google OAuth callback.");
                return Redirect($"http://194.233.81.64:5001/Auth/Login?error={Uri.EscapeDataString("Google login failed: " + ex.Message)}");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BadRequest("Refresh token is required");
                }

                var user = await _tokenRepository.GetUserFromRefreshTokenAsync(refreshToken);
                if (user == null)
                {
                    return BadRequest("Invalid refresh token");
                }

                var tokenDto = await _tokenRepository.CreateJWTTokenAsync(user, true);
                _tokenRepository.SetTokenCookie(tokenDto, HttpContext);

                return Ok(new { message = "Token refreshed successfully", token = tokenDto.AccessToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while refreshing token");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while refreshing token");
            }
        }
    }
}