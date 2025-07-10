using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Chat
{
    public class NotifiDTO
    {
        public int ConversationId { get; set; } // Cuộc trò chuyện chứa tin nhắn chưa đọc
        public int SenderId { get; set; }       // Ai là người gửi
        public int ReceiverId { get; set; }     // Ai nhận thông báo
        public string SenderName { get; set; }  // Tên người gửi
        public string Message { get; set; }     // Nội dung tin nhắn mới
        public DateTime SentAt { get; set; }    // Thời gian gửi
        public bool IsRead { get; set; }        // Đã đọc hay chưa
    }
}
