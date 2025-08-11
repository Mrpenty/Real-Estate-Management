using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Review
{
    public class ReviewReplyDTO
    {
        public int Id { get; set; }
        public int LandlordId { get; set; }
        public string ReplyContent { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
