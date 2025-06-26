namespace RealEstateManagement.Data.Entity.PropertyEntity
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string Url { get; set; }
        public bool IsPrimary { get; set; }
        public int Order { get; set; }

        public Property? Property { get; set; }
    }
}
