using RealEstateManagement.Business.DTO.ChatDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Chat
{
    public interface IChatService
    {
        Task<MessageDTO> SendMessageAsync(SendMessageDTO dto);
        Task<List<MessageDTO>> GetMessagesAsync(int conversationId);
    }
}
