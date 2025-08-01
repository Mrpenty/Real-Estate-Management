using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.News
{
     public class NewImageCreateDto
    {
        //public int Id { get; set; }
        public int NewId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
        public int Order { get; set; }
    }
}
