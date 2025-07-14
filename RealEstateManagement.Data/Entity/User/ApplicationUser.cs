using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using RealEstateManagement.Data.Entity.Messages;
using RealEstateManagement.Data.Entity.Payment;
using RealEstateManagement.Data.Entity.PropertyEntity;
using Property = RealEstateManagement.Data.Entity.PropertyEntity.Property;


namespace RealEstateManagement.Data.Entity.User
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Role { get; set; } // renter, landlord, admin

        public string? ProfilePictureUrl { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public string? ConfirmationCode { get; set; }
        public DateTime? ConfirmationCodeExpiry { get; set; }
        public ICollection<UserPreference> Preferences { get; set; }
        public ICollection<Property>? Properties { get; set; }
        public ICollection<Review> Reviews { get; set; }

        public ICollection<Conversation> ConversationsAsRenter { get; set; }
        public ICollection<Conversation> ConversationsAsLandlord { get; set; }
        public ICollection<UserFavoriteProperty> FavoriteProperties { get; set; }
        public ICollection<Message> MessagesSent { get; set; }

        public Wallet Wallet { get; set; }

        public string? CitizenIdNumber { get; set; }
        public DateTime? CitizenIdIssuedDate { get; set; }
        public DateTime? CitizenIdExpiryDate { get; set; }
        public bool IsActive { get; set; } = true; // true: active, false: banned
        public string? CitizenIdImageUrl { get; set; } // URL to the citizen ID image
        public string? CitizenIdFrontImageUrl { get; set; }
        public string? CitizenIdBackImageUrl { get; set; }
        public string? VerificationRejectReason { get; set; }
    }
}
