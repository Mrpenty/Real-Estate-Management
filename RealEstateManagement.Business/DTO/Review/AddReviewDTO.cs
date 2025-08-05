using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Review
{
    public class AddReviewDTO
    {
        public int ContractId { get; set; }
        public int PropertyId { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
    }
}
