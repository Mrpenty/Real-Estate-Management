namespace RealEstateManagement.Data.Entity
{
    public class Property
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public int LandlordId { get; set; }
        public string Status { get; set; }
        public bool IsPromoted { get; set; }
        public decimal Price { get; set; }
        public bool IsVerified { get; set; }
        public int ViewsCount { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; }

        public ApplicationUser Landlord { get; set; }
        public ICollection<PropertyImage> Images { get; set; }
        public ICollection<PropertyPost> Posts { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Conversation> Conversations { get; set; }
        public ICollection<PropertyAmenity> PropertyAmenities { get; set; }
        public ICollection<UserPreferenceFavoriteProperties> UserPreferences { get; set; } // Corrected type
    }
}
