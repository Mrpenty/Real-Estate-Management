using RealEstateManagement.Business.DTO.AuthDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthMessDTO> LoginAsync(LoginDTO loginDTO);
        Task<AuthMessDTO> RegisterAsync(RegisterDTO registerDTO);
        Task LogoutAsync();
        Task<AuthMessDTO> VerifyOtpAsync(string phoneNumber, string otp);

        Task<AuthMessDTO> GoogleLoginAsync(string idToken);
        Task<AuthMessDTO> HandleGoogleOAuthCallbackAsync(string code, string redirectUri);
        Task<AuthMessDTO> VerifyEmailConfirmationAsync(string email);
        Task<AuthMessDTO> ResendOtpAsync(string phoneNumber);
        
        // Forgot Password methods
        Task<AuthMessDTO> ForgotPasswordAsync(string phoneNumber);
        Task<AuthMessDTO> VerifyOtpForPasswordResetAsync(string phoneNumber, string otp);
        Task<AuthMessDTO> ResetPasswordWithOTPAsync(ResetPasswordWithOTPDTO resetPasswordDTO);
    }
}
