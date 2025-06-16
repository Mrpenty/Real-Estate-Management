namespace RealEstateManagement.Data.Entity
{
    public class PropertyPost
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int LandlordId { get; set; }
        public PropertyPostStatus Status { get; set; } = PropertyPostStatus.Draft;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? ArchiveDate { get; set; }

        public Property Property { get; set; }
        public ApplicationUser Landlord { get; set; }
        public ApplicationUser VerifiedByUser { get; set; }

        public enum PropertyPostStatus
        {
            Draft = 0,
            Pending = 1,
            Approved = 2,
            Rejected = 3,
            Rented = 4,
            Sold = 5
        }
    }
}
