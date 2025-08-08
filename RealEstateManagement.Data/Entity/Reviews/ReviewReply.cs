using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Entity.Reviews
{
    public class ReviewReply
    {
        public int Id { get; set; }
        public int ReviewId { get; set; }
        public int LandlordId { get; set; }
        public string ReplyContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsFlagged { get; set; } // Nếu bị flag bởi admin
        public bool IsVisible { get; set; } // Hiển thị ra ngoài
        public Review Review { get; set; }
        public ApplicationUser Landlord { get; set; }
    }
}
