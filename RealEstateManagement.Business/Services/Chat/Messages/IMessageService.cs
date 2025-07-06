using RealEstateManagement.Business.DTO.Chat;
using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Services.Chat.Messages
{
    public interface IMessageService
    {
        Task<Message> SendMessageAsync(MessageDTO dto, int senderId);
        Task<IEnumerable<MessageViewDTO>> GetMessagesByConversationAsync(int conversationId, int skip = 0, int take = 20);
    }
}
