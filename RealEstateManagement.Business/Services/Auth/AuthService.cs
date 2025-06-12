using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.AuthDTO;
using RealEstateManagement.Business.Repositories.Token;
using RealEstateManagement.Business.Services.Mail;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Auth
{
    public class AuthService: IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenRepository _tokenRepository;
        private readonly IMailService _mailService;
        private readonly ISmsService _smsService; // Assuming you have an ISmsService for sending OTPs
        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, ITokenRepository tokenRepository, IMailService mailService,ISmsService smsService )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _tokenRepository = tokenRepository;
            _mailService = mailService;
            _smsService = smsService; // Injecting SMS service for OTP functionality
        }

        public async Task<AuthMessDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == loginDTO.LoginIdentifier);

            if (user == null)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid Phone Number" };
            }

            if (!user.PhoneNumberConfirmed)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Phone number not verified. Please verify your phone number first." };
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
            // Check if phone number already exists
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == registerDTO.PhoneNumber);
            if (existingUser != null)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Phone number already registered." };
            }

            var user = new ApplicationUser
            {
                Name = registerDTO.Name,
                UserName = registerDTO.UserName, // Use phone number as username
                PhoneNumber = registerDTO.PhoneNumber,
                NormalizedUserName = _userManager.NormalizeName(registerDTO.PhoneNumber),
                NormalizedEmail = null, // Email is optional if not used
                SecurityStamp = Guid.NewGuid().ToString(),
                IsVerified = false // Set to false until OTP is verified
            };

            var createdUser = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = string.Join(", ", createdUser.Errors.Select(e => e.Description)) };
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Renter");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
            }

            // Generate and send OTP
            var confirmationCode = _tokenRepository.GenerateConfirmationCode();
            user.ConfirmationCode = confirmationCode;
            user.ConfirmationCodeExpiry = DateTime.Now.AddMinutes(5); 
            await _userManager.UpdateAsync(user);

            // Assume ISmsService is injected to send OTP
            await _smsService.SendOtpAsync(user.PhoneNumber, confirmationCode);

            return new AuthMessDTO { IsAuthSuccessful = true, ErrorMessage = "Registration successful. An OTP has been sent to your phone for verification." };
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

            if (user.ConfirmationCode != otp || user.ConfirmationCodeExpiry < DateTime.Now)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid or expired OTP." };
            }

            user.PhoneNumberConfirmed = true;
            user.ConfirmationCode = null; // Clear OTP after verification
            user.ConfirmationCodeExpiry = null;
            await _userManager.UpdateAsync(user);

            return new AuthMessDTO { IsAuthSuccessful = true, ErrorMessage = "Phone number verified successfully." };
        }

        public async Task<AuthMessDTO> GoogleLoginAsync(string idToken)
        {
            var payload = await _tokenRepository.ValidateGoogleIdTokenAsync(idToken);
            var email = payload.Email;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // Register new user
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    Name = payload.Name ?? email.Split('@')[0],
                    IsVerified = true, // Google email is pre-verified
                    CreatedAt = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Registration failed" };
                }
                await _userManager.AddToRoleAsync(user, "User"); // Default role
            }
            else if (!user.IsVerified)
            {
                var confirmationCode = _tokenRepository.GenerateConfirmationCode();
                user.ConfirmationCode = confirmationCode;
                user.ConfirmationCodeExpiry = DateTime.Now.AddMinutes(1);
                await _userManager.UpdateAsync(user);
                await _mailService.SendEmailAsync(user.Email, "Email Confirmation", $"Your confirmation code is: {confirmationCode}. It expires in 1 minute.");
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Please verify your email with the code sent." };
            }

            var tokenDTO = await _tokenRepository.CreateJWTTokenAsync(user, true);
            _tokenRepository.SetTokenCookie(tokenDTO, _httpContextAccessor.HttpContext);
            return new AuthMessDTO { IsAuthSuccessful = true, ErrorMessage = "Login successful" };
        }

        public async Task<AuthMessDTO> VerifyEmailConfirmationAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.ConfirmationCode != code || user.ConfirmationCodeExpiry <= DateTime.Now)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid or expired confirmation code" };
            }

            user.EmailConfirmed = true;
            user.ConfirmationCode = null;
            user.ConfirmationCodeExpiry = null;
            await _userManager.UpdateAsync(user);

            var tokenDTO = await _tokenRepository.CreateJWTTokenAsync(user, true);
            _tokenRepository.SetTokenCookie(tokenDTO, _httpContextAccessor.HttpContext);
            return new AuthMessDTO { IsAuthSuccessful = true, ErrorMessage = "Email verified. Login successful" };
        }


    }
}
