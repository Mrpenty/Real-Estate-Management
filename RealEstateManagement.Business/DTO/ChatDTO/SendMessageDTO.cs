using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.ChatDTO
{
    public class SendMessageDTO
    {
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
    }
}
