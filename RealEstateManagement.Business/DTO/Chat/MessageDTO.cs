namespace RealEstateManagement.Business.DTO.Chat
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string? SenderName { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
