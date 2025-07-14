using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Interaction;
using RealEstateManagement.Business.Services.TenantInteraction;

namespace RealEstateManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantInteractionController : ControllerBase
    {
        private readonly IInteractionService _tenantInteractionService;
        public TenantInteractionController(IInteractionService tenantInteractionService)
        {
            _tenantInteractionService = tenantInteractionService;
        }
        [HttpGet("landlord-profile/{landlordId}")]
        public async Task<ActionResult<ProfileLandlordDTO>> GetLandlordProfile(int landlordId)
        {
            var profile = await _tenantInteractionService.GetProfileLandlordByIdAsync(landlordId);

            if (profile == null)
                return NotFound("Không tìm thấy landlord");

            return Ok(profile);
        }
    }
}
