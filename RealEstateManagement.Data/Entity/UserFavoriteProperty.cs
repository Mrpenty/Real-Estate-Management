namespace RealEstateManagement.Data.Entity
{
    public class UserFavoriteProperty
    {
        public int UserId { get; set; }           // Liên kết với ApplicationUser
        public int PropertyId { get; set; }
        public DateTime CreatedAt { get; set; }

        public ApplicationUser User { get; set; }
        public Property Property { get; set; }
    }
}
