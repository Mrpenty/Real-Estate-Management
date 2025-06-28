using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Chat
{
    public class ConversationDTO
    {
        public int RenterId { get; set; }
        public int LandlordId { get; set; }
        public int? PropertyId { get; set; }
    }
}
