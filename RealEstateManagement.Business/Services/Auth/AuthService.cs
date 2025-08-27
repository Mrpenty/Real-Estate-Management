using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.AuthDTO;
using RealEstateManagement.Business.Repositories.Token;
using RealEstateManagement.Business.Services.Mail;

using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Google.Apis.Util.Store;
using Google.Apis.Auth.OAuth2;
using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Business.Services.Wallet;

namespace RealEstateManagement.Business.Services.Auth
{
    public class AuthService: IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenRepository _tokenRepository;
        private readonly IMailService _mailService;
        private readonly ISmsService _smsService; 
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _configuration;
        private readonly WalletService _walletService;
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, ITokenRepository tokenRepository, IMailService mailService,ISmsService smsService, ILogger<AuthService> logger, IConfiguration configuration, WalletService walletService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _tokenRepository = tokenRepository;
            _mailService = mailService;
            _smsService = smsService; 
            _logger = logger;
            _configuration = configuration;
            _walletService = walletService;
        }

        public async Task<AuthMessDTO> LoginAsync(LoginDTO loginDTO)
        {
            //Comment 
            //loginDTO.LoginIdentifier = loginDTO.LoginIdentifier.Replace("+84", "0");
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == loginDTO.LoginIdentifier);

            if (user == null)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid Phone Number" };
            }

            if (!user.PhoneNumberConfirmed)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Phone number not verified. Please verify your phone number first." };
            }
            if (!user.IsActive)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "This account has been ban." };
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, false);
            if (result.Succeeded)
            {
                var tokenDto = await _tokenRepository.CreateJWTTokenAsync(user, true);

                _tokenRepository.SetTokenCookie(tokenDto, _httpContextAccessor.HttpContext);
                return new AuthMessDTO { IsAuthSuccessful = true, ErrorMessage = "Login successful", Token = tokenDto.AccessToken };
            }
            else
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid phone number or password." };
            }
        }

        public async Task<AuthMessDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            try
            {
                var existingUser = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == registerDTO.PhoneNumber);
                if (existingUser != null)
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Phone number already registered." };

                var existingEmail = await _userManager.Users
                    .FirstOrDefaultAsync(u => u.Email == registerDTO.Email);
                if (existingEmail != null)
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Email already registered." };

                // Tạo UserName duy nhất từ Name (loại bỏ khoảng trắng và ký tự đặc biệt)
                var userName = registerDTO.Name.Replace(" ", "").Replace("-", "").Replace("_", "");
                if (string.IsNullOrEmpty(userName))
                {
                    userName = registerDTO.PhoneNumber; // Fallback về PhoneNumber nếu Name rỗng
                }

                var user = new ApplicationUser
                {
                    Name = registerDTO.Name,
                    UserName = userName,
                    PhoneNumber = registerDTO.PhoneNumber,
                    NormalizedUserName = _userManager.NormalizeName(userName),
                    NormalizedEmail = null, // hoặc _userManager.NormalizeEmail(registerDTO.Email)
                    SecurityStamp = Guid.NewGuid().ToString(),
                    IsVerified = false
                };

                var createdUser = await _userManager.CreateAsync(user, registerDTO.Password);
                if (!createdUser.Succeeded)
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = string.Join(", ", createdUser.Errors.Select(e => e.Description)) };

                var roleResult = await _userManager.AddToRoleAsync(user, "Renter");
                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
                }

                // Tạo ví cho user mới
                await _walletService.CreateWalletAsync(user.Id);

                // Tạo và lưu OTP
                var confirmationCode = _tokenRepository.GenerateConfirmationCode();
                user.ConfirmationCode = confirmationCode;
                user.ConfirmationCodeExpiry = DateTime.UtcNow.AddMinutes(5);
                await _userManager.UpdateAsync(user);

                // Gửi SMS OTP (không làm fail đăng ký nếu SMS lỗi)
                try
                {
                    await _smsService.SendOtpAsync(user.PhoneNumber, confirmationCode);
                }
                catch (Exception smsEx)
                {
                    _logger.LogWarning(smsEx, "Failed to send OTP via SMS for user {UserId}, but registration continues", user.Id);
                }

                return new AuthMessDTO
                {
                    IsAuthSuccessful = true,
                    ErrorMessage = "Registration successful. An OTP has been sent to your phone for verification."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed for phone number: {PhoneNumber}", registerDTO.PhoneNumber);
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Registration failed due to a server error. Please try again." };
            }
        }


        public async Task LogoutAsync()
        {
            _tokenRepository.DeleteTokenCookie(_httpContextAccessor.HttpContext);
            await _signInManager.SignOutAsync();
        }


        public async Task<AuthMessDTO> VerifyOtpAsync(string phoneNumber, string otp)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
            if (user == null)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "User not found." };
            }

            try
            {
                // Use Twilio Verify API to verify OTP
                bool isOtpValid = await _smsService.VerifyOtpAsync(phoneNumber, otp);
                
                if (isOtpValid)
                {
                    user.PhoneNumberConfirmed = true;
                    user.ConfirmationCode = null; 
                    user.ConfirmationCodeExpiry = null;
                    await _userManager.UpdateAsync(user);

                    return new AuthMessDTO { IsAuthSuccessful = true, ErrorMessage = "Phone number verified successfully." };
                }
                else
                {
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid OTP." };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying OTP for phone number: {PhoneNumber}", phoneNumber);
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Error verifying OTP. Please try again." };
            }
        }

        public async Task<AuthMessDTO> GoogleLoginAsync(string idToken)
        {
            var payload = await _tokenRepository.ValidateGoogleIdTokenAsync(idToken);
            var email = payload.Email;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    Name = payload.Name ?? email.Split('@')[0],
                    EmailConfirmed = false,
                    CreatedAt = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Registration failed" };
                }
                await _userManager.AddToRoleAsync(user, "Renter");
            }

            // Always check if email is confirmed
            if (!user.EmailConfirmed)
            {
                // Removed confirmation code generation and storage as per user request

                var confirmationLink = $"https://localhost:7160/Auth/VerifyEmail?email={Uri.EscapeDataString(user.Email)}";
                await _mailService.SendEmailAsync(
                    user.Email,
                    "Email Confirmation",
                    $"Please confirm your email by clicking this link: {confirmationLink}"
                );
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Please confirm your email. A confirmation link has been sent." };
            }

            var tokenDTO = await _tokenRepository.CreateJWTTokenAsync(user, true);
            _tokenRepository.SetTokenCookie(tokenDTO, _httpContextAccessor.HttpContext);
            return new AuthMessDTO { IsAuthSuccessful = true, ErrorMessage = "Login successful", Token = tokenDTO.AccessToken };
        }

        public async Task<AuthMessDTO> HandleGoogleOAuthCallbackAsync(string code, string redirectUri)
        {
            try
            {
                var clientId = _configuration["Google:ClientId"];
                var clientSecret = _configuration["Google:ClientSecret"];

                _logger.LogInformation($"Google ClientId: {clientId}");
                _logger.LogInformation($"Google ClientSecret: {clientSecret}");

                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                {
                    _logger.LogError("Google ClientId or ClientSecret is null or empty.");
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Google client configuration missing." };
                }

                var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = clientId,
                        ClientSecret = clientSecret
                    },
                    Scopes = new[] { "email", "profile" },
                    DataStore = new FileDataStore("Google.Apis.Auth.MVC")
                });

                var token = await flow.ExchangeCodeForTokenAsync(
                    "google_oauth_user_key", // Provide a non-empty key for FileDataStore
                    code,
                    redirectUri,
                    CancellationToken.None);

                // Use the ID Token from the response
                if (token.IdToken == null)
                {
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Failed to retrieve Google ID Token." };
                }

                // Reuse the existing GoogleLoginAsync logic to process the ID token
                return await GoogleLoginAsync(token.IdToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling Google OAuth callback.");
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Google authentication failed: " + ex.Message };
            }
        }

        public async Task<AuthMessDTO> VerifyEmailConfirmationAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid email" };
            }

            // The code check is removed as per your request for link-only confirmation
            // if (user.ConfirmationCode != code || user.ConfirmationCodeExpiry <= DateTime.Now)
            // {
            //     return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid or expired confirmation code" };
            // }

            user.EmailConfirmed = true;
            
            await _userManager.UpdateAsync(user);

            var tokenDTO = await _tokenRepository.CreateJWTTokenAsync(user, true);
            _tokenRepository.SetTokenCookie(tokenDTO, _httpContextAccessor.HttpContext);
            return new AuthMessDTO { IsAuthSuccessful = true, ErrorMessage = "Email verified. Login successful", Token = tokenDTO.AccessToken };
        }

        public async Task<AuthMessDTO> ResendOtpAsync(string phoneNumber)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
                if (user == null)
                {
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "User not found." };
                }

                // Generate new OTP
                var otp = GenerateOTP();
                var expiryTime = DateTime.Now.AddMinutes(5); // OTP valid for 5 minutes

                // Update user with new OTP
                user.ConfirmationCode = otp;
                user.ConfirmationCodeExpiry = expiryTime;
                await _userManager.UpdateAsync(user);

                // Send OTP via TingTing SMS service
                await _smsService.SendOtpAsync(phoneNumber, otp);

                return new AuthMessDTO 
                { 
                    IsAuthSuccessful = true, 
                    ErrorMessage = "OTP has been resent successfully." 
                };
            }
            catch (Exception ex)
            {
                return new AuthMessDTO 
                { 
                    IsAuthSuccessful = false, 
                    ErrorMessage = $"Failed to resend OTP: {ex.Message}" 
                };
            }
        }

        private string GenerateOTP()
        {
            // Generate a 6-digit OTP
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        // Forgot Password methods
        public async Task<AuthMessDTO> ForgotPasswordAsync(string phoneNumber)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
                if (user == null)
                {
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Số điện thoại không tồn tại trong hệ thống." };
                }

                // Send OTP via Twilio Verify API (Twilio generates and sends OTP automatically)
                await _smsService.SendOtpAsync(phoneNumber, ""); // Empty string since Verify API generates OTP

                return new AuthMessDTO 
                { 
                    IsAuthSuccessful = true, 
                    ErrorMessage = "Mã OTP đã được gửi đến số điện thoại của bạn." 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ForgotPasswordAsync for phone: {PhoneNumber}", phoneNumber);
                return new AuthMessDTO 
                { 
                    IsAuthSuccessful = false, 
                    ErrorMessage = $"Không thể gửi mã OTP: {ex.Message}" 
                };
            }
        }

        public async Task<AuthMessDTO> VerifyOtpForPasswordResetAsync(string phoneNumber, string otp)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
                if (user == null)
                {
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Số điện thoại không tồn tại." };
                }

                // Use Twilio Verify API to verify OTP
                bool isOtpValid = await _smsService.VerifyOtpAsync(phoneNumber, otp);
                
                if (isOtpValid)
                {
                    return new AuthMessDTO 
                    { 
                        IsAuthSuccessful = true, 
                        ErrorMessage = "Xác thực OTP thành công. Vui lòng nhập mật khẩu mới." 
                    };
                }
                else
                {
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Mã OTP không đúng hoặc đã hết hạn." };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in VerifyOtpForPasswordResetAsync for phone: {PhoneNumber}", phoneNumber);
                return new AuthMessDTO 
                { 
                    IsAuthSuccessful = false, 
                    ErrorMessage = $"Lỗi xác thực OTP: {ex.Message}" 
                };
            }
        }

        public async Task<AuthMessDTO> ResetPasswordWithOTPAsync(ResetPasswordWithOTPDTO resetPasswordDTO)
        {
            try
            {
                // Validate passwords match
                if (resetPasswordDTO.NewPassword != resetPasswordDTO.ConfirmPassword)
                {
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Mật khẩu mới và xác nhận mật khẩu không khớp." };
                }

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == resetPasswordDTO.PhoneNumber);
                if (user == null)
                {
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Số điện thoại không tồn tại." };
                }

                // Reset password
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDTO.NewPassword);

                if (result.Succeeded)
                {
                    return new AuthMessDTO 
                    { 
                        IsAuthSuccessful = true, 
                        ErrorMessage = "Đặt lại mật khẩu thành công. Vui lòng đăng nhập với mật khẩu mới." 
                    };
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = $"Không thể đặt lại mật khẩu: {errors}" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ResetPasswordWithOTPAsync for phone: {PhoneNumber}", resetPasswordDTO.PhoneNumber);
                return new AuthMessDTO 
                { 
                    IsAuthSuccessful = false, 
                    ErrorMessage = $"Lỗi đặt lại mật khẩu: {ex.Message}" 
                };
            }
        }

    }
}