namespace RealEstateManagement.Data.Entity
{
    public class UserPreference
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Location { get; set; }
        public decimal? PriceRangeMin { get; set; }
        public decimal? PriceRangeMax { get; set; }
        public string Amenities { get; set; }
        public DateTime CreatedAt { get; set; }

        public ApplicationUser User { get; set; }
    }
}
