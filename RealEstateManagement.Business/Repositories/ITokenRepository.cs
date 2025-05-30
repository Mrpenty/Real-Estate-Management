using RealEstateManagement.Business.DTO;
using RealEstateManagement.Data.Entity;
using Microsoft.AspNetCore.Http;

namespace RealEstateManagement.Business.Repositories
{
    public interface ITokenRepository
    {
        Task<TokenDTO> CreateJWTTokenAsync(ApplicationUser user, bool populateExp);
        Task<TokenDTO> RefreshJWTTokenAsync(TokenDTO tokenDTO);
        void SetTokenCookie(TokenDTO tokenDTO, HttpContext context);
        void DeleteTokenCookie(HttpContext context);
    }
}
