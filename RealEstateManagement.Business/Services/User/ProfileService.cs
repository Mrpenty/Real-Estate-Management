using Microsoft.AspNetCore.Identity;
using RealEstateManagement.Business.DTO.UserDTO;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.Business.Services.User
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ViewProfileDto> GetProfileAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new Exception("User not found");

            return new ViewProfileDto
            {
                Name = user.Name,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Email = user.Email,
                Role = user.Role,
                PhoneNumber = user.PhoneNumber,
                IsVerified = user.IsVerified,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<IdentityResult> UpdateProfileAsync(int userId, UpdateProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new Exception("User not found");

            user.Name = model.Name;
            user.ProfilePictureUrl = model.ProfilePictureUrl;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
           

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
    }
}
