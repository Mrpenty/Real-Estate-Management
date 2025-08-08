using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.Data.Entity.Reviews
{
    public class Review
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int RenterId { get; set; }
        public int ContractId { get; set; } // Đảm bảo mỗi contract chỉ 1 review
        public int Rating { get; set; } // 1-5
        public string ReviewText { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsFlagged { get; set; } // Nếu bị flag bởi admin
        public bool IsVisible { get; set; } // Hiển thị ra ngoài
        public ReviewReply Reply { get; set; }
        public Property Property { get; set; }
        public ApplicationUser Renter { get; set; }
        public RentalContract Contract { get; set; }
    }
}
