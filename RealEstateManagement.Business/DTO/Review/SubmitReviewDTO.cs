using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Review
{
    public class SubmitReviewDTO
    {
        public int PropertyId { get; set; }
        public int Rating { get; set; }      // 1-5
        public string Comment { get; set; }
    }
}
