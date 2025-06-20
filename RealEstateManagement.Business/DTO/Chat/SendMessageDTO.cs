namespace RealEstateManagement.Business.DTO.Chat
{
    public class SendMessageDTO
    {
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
    }
}
