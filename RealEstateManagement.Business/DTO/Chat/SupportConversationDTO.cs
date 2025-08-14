namespace RealEstateManagement.Business.DTO.Chat
{
    public class CreateSupportConversationDTO
    {
        public string InitialMessage { get; set; }
    }

    public class SupportConversationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? AdminId { get; set; }
        public string Subject { get; set; } = "Yêu cầu hỗ trợ";
        public string Status { get; set; } = "Open";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string UserName { get; set; }
        public string? AdminName { get; set; }
        public string LastMessage { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public int UnreadCount { get; set; }
    }

    public class SupportMessageDTO
    {
        public int Id { get; set; }
        public int SupportConversationId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsFromAdmin { get; set; }
        public string SenderName { get; set; }
        public string SenderRole { get; set; }
    }

    public class UpdateSupportStatusDTO
    {
        public string Status { get; set; }
        public int? AdminId { get; set; }
        public string? AdminNote { get; set; }
    }
} 