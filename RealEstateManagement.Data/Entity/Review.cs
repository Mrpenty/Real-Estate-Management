﻿using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.Data.Entity
{
    public class Review
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int RenterId { get; set; }
        public int Rating { get; set; } //1-5
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }

        public Property? Property { get; set; }
        public ApplicationUser Renter { get; set; }
    }
}
