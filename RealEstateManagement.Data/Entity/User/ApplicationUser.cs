﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using RealEstateManagement.Data.Entity.Messages;
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

    }
}
