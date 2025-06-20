using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.Data.Entity.Messages
{
    public class Conversation
    {
        public int Id { get; set; }
        public int RenterId { get; set; }
        public int LandlordId { get; set; }
        public int? PropertyId { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ApplicationUser Renter { get; set; }
        public ApplicationUser Landlord { get; set; }
        public Property Property { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
