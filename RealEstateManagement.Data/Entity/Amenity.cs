using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.Data.Entity
{
    public class Amenity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<PropertyAmenity> PropertyAmenities { get; set; }
    }
}
