using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Review
{
    public class AddReviewDTO
    {
        public int PropertyPostId { get; set; }    // PropertyPost mà renter muốn review
        public string ReviewText { get; set; }
        [Range(1, 5, ErrorMessage = "Số sao phải từ 1 đến 5.")]
        public int Rating { get; set; }
    }
}
