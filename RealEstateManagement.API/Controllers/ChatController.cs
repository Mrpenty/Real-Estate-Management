using Microsoft.AspNetCore.Mvc;
using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Business.Services.Chat;

namespace RealEstateManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _messageService;

        public ChatController(IChatService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("create-conversation")]
        public async Task<IActionResult> CreateConversation([FromBody] ConversationDTO dto)
        {
            if (dto == null || dto.RenterId <= 0 || dto.LandlordId <= 0 || dto.PropertyId <= 0)
            {
                return BadRequest("Invalid conversation data.");
            }

            try
            {
                var conversation = await _messageService.CreateConversationAsync(dto);
                if (conversation == null)
                {
                    return NotFound("Conversation already exists or could not be created.");
                }
                return Ok(conversation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _messageService.GetDetailsAsync(id);
            return Ok(result);
        }
    }
}
