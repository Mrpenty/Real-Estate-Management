using RealEstateManagement.Business.DTO;
using RealEstateManagement.Data.Entity;
using Microsoft.AspNetCore.Http;
using RealEstateManagement.Business.DTO.AuthDTO;
using Google.Apis.Auth;


namespace RealEstateManagement.Business.Repositories.Token
{
    public interface ITokenRepository
    {
        Task<TokenDTO> CreateJWTTokenAsync(ApplicationUser user, bool populateExp);
        Task<TokenDTO> RefreshJWTTokenAsync(TokenDTO tokenDTO);
        void SetTokenCookie(TokenDTO tokenDTO, HttpContext context);
        void DeleteTokenCookie(HttpContext context);

        Task<GoogleJsonWebSignature.Payload> ValidateGoogleIdTokenAsync(string idToken);
        string GenerateConfirmationCode();

    }
}
