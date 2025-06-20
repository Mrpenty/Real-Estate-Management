﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDTO dto)
        {
            var result = await _messageService.SendMessageAsync(dto);
            return Ok(result);
        }

        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetMessages(int conversationId)
        {
            var messages = await _messageService.GetMessagesAsync(conversationId);
            return Ok(messages);
        }
    }
}
