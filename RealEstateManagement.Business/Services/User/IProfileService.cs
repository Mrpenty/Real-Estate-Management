using Microsoft.AspNetCore.Identity;
using RealEstateManagement.Business.DTO.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.User
{
    public interface IProfileService
    {
        Task<ViewProfileDto> GetProfileAsync(int userId);
        Task<IdentityResult> UpdateProfileAsync(int userId, UpdateProfileDto model);
    }
}
