using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Entity
{
    public class News
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(255)]
        public string? Slug { get; set; } // Tạo từ Title, dùng trong URL

        [Required]
        public string Content { get; set; }

        [MaxLength(500)]
        public string? Summary { get; set; } // Tóm tắt nội dung
         
        public DateTime? PublishedAt { get; set; } // Ngày xuất bản

        [MaxLength(100)]
        public string? AuthorName { get; set; } // Tên người đăng bài (Admin, AI Bot, ...)

        [MaxLength(255)]
        public string? Source { get; set; } // Nếu là bài viết từ nguồn ngoài

        public bool IsPublished { get; set; } = false; // Trạng thái công khai

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
        public ICollection<NewsImage>? Images { get; set; }
    }
}
