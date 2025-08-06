using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.Data.Entity.User
{
    public class InterestedProperty
    {
        //Xác nhận xem property post đã thuê hay chưa 
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int RenterId { get; set; }
        public DateTime InterestedAt { get; set; }
        public InterestedStatus Status { get; set; }
        public DateTime? RenterReplyAt { get; set; }
        public DateTime? LandlordReplyAt { get; set; }
        public Property Property { get; set; }
        public ApplicationUser Renter { get; set; }
        public bool RenterConfirmed { get; set; } // true nếu renter đã xác nhận
        public bool LandlordConfirmed { get; set; } // true nếu landlord đã xác nhận
    }
    public enum InterestedStatus
    {
        None = 0, 
        WaitingForRenterReply = 1,
        RenterWantToRent = 2,
        RenterNotRent = 3,
        LandlordAccepted = 4,
        LandlordRejected = 5,
        DealSuccess = 6 
    }
}
