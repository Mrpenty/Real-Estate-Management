using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Chat
{
    public class ConversationDTO
    {
        public int Id { get; set; } 
        public int RenterId { get; set; }
        public int LandlordId { get; set; }
        public int? PropertyId { get; set; }
        public string RenterName { get; set; }
        public string LandlordName { get; set; }

        public string? LastMessage { get; set; }          
        public DateTime? LastSentAt { get; set; }          
        //Danh sách tin nhắn
        public List<MessageDTO> Messages { get; set; }
    }
}
