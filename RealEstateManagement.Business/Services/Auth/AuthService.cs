using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateManagement.Business.DTO.AuthDTO;
using RealEstateManagement.Business.Repositories.Token;
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

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _tokenRepository = tokenRepository;
        }

        public async Task<AuthMessDTO> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.LoginIdentifier) ??
                       await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == loginDTO.LoginIdentifier);

            if (user == null)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid User Identifier " };
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
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = "Invalid login attempt." };
            }
        }

        public async Task<AuthMessDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email,
                NormalizedUserName = _userManager.NormalizeName(registerDTO.UserName),
                NormalizedEmail = _userManager.NormalizeEmail(registerDTO.Email),
                SecurityStamp = Guid.NewGuid().ToString(),
              
            };

            var createdUser = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = string.Join(", ", createdUser.Errors.Select(e => e.Description)) };
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "HR");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return new AuthMessDTO { IsAuthSuccessful = false, ErrorMessage = string.Join(", ", roleResult.Errors.Select(e => e.Description)) };
            }

            return new AuthMessDTO { IsAuthSuccessful = true };
        }

        public async Task LogoutAsync()
        {
            _tokenRepository.DeleteTokenCookie(_httpContextAccessor.HttpContext);
            await _signInManager.SignOutAsync();
        }


    }
}
