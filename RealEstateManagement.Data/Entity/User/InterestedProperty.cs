using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Entity.User
{
    public class InterestedProperty
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int RenterId { get; set; }
        public DateTime InterestedAt { get; set; }
        public InterestedStatus Status { get; set; } // None, WaitingForRenterReply, RenterWantToRent, RenterNotRent, LandlordAccepted, LandlordRejected
        public DateTime? RenterReplyAt { get; set; }
        public DateTime? LandlordReplyAt { get; set; }
        public PropertyEntity.Property Property { get; set; }
        public ApplicationUser Renter { get; set; }
    }
    public enum InterestedStatus
    {
        None = 0, 
        WaitingForRenterReply = 1,
        RenterWantToRent = 2,
        RenterNotRent = 3,
        LandlordAccepted = 4,
        LandlordRejected = 5
    }
}
