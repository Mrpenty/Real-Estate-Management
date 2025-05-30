namespace RealEstateManagement.Data.Entity
{
    public class PropertyAmenity
    {
        public int PropertyId { get; set; }
        public int AmenityId { get; set; }

        public Property Property { get; set; }
        public Amenity Amenity { get; set; }
    }
}
