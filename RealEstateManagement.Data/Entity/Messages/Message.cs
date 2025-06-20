using RealEstateManagement.Data.Entity.User;

namespace RealEstateManagement.Data.Entity.Messages
{
    public class Message
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public bool NotificationSent { get; set; }
        public DateTime SentAt { get; set; }

        // Navigation properties
        public Conversation Conversation { get; set; }
        public ApplicationUser Sender { get; set; }
    }
}
