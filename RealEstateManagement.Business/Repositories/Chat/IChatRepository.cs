using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.Repositories.Chat
{
    public interface IChatRepository
    {
        Task<Message> SaveMessageAsync(Message message);
        Task<List<Message>> GetMessagesByConversationIdAsync(int conversationId);
    }
}
