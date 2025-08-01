using System.ComponentModel.DataAnnotations;

namespace RealEstateManagement.Data.Entity
{
    public class NewsImage
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
        public int Order { get; set; }

        public News? News { get; set; }
    }

}