using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
namespace RealEstateManagement.Data.Entity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Role { get; set; } // renter, landlord, admin
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public ICollection<UserPreference> Preferences { get; set; }
        public ICollection<Property> Properties { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<MessageThread> MessagesAsRenter { get; set; } 
        public ICollection<MessageThread> MessagesAsLandlord { get; set; } 
    }
}
