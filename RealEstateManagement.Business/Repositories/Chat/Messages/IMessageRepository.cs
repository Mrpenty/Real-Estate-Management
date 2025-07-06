using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Chat.Messages
{
    public interface IMessageRepository
    {
        Task<Message> CreateAsync(Message message);
        Task<IEnumerable<Message>> GetByConversationIdAsync(int conversationId, int skip = 0, int take = 20);
    }
}
