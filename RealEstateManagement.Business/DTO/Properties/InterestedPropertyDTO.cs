using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Business.DTO.Properties
{
    public class InterestedPropertyDTO
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int RenterId { get; set; }
        public string? RenterName { get; set; }
        public string? RenterPhone { get; set; }
        public string? RenterUserName { get; set; }
        public string? RenterEmail { get; set; }
        public DateTime InterestedAt { get; set; }
        public int Status { get; set; }
        public DateTime? RenterReplyAt { get; set; }
        public DateTime? LandlordReplyAt { get; set; }
        public bool RenterConfirmed { get; set; }
        public bool LandlordConfirmed { get; set; }
    }
}
