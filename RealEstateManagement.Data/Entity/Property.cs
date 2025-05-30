namespace RealEstateManagement.Data.Entity
{
    public class Property
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Type { get; set; } // room, house, apartment
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
      //  public string RenterType { get; set; } // male, female, both
        public int LandlordId { get; set; }
        public string Status { get; set; } // available, rented, unavailable
        public bool IsPromoted { get; set; }
        public decimal Price { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool IsVerified { get; set; }
        public int ViewsCount { get; set; }
        public string Location { get; set; } // Store as string (lat,long) for simplicity
        public DateTime CreatedAt { get; set; }

        public ApplicationUser Landlord { get; set; }
        public ICollection<PropertyImage> Images { get; set; }
        public ICollection<PropertyPost> Posts { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<PropertyAmenity> PropertyAmenities { get; set; }
        public ICollection<UserPreference> UserPreferences { get; set; } // New: Users who favorited this property
    }
}
