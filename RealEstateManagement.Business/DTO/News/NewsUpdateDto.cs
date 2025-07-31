using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.News
{
    public class NewsUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Summary { get; set; }
        public string? AuthorName { get; set; }
        public string? Source { get; set; }
        public bool IsPublished { get; set; }
    }
}
