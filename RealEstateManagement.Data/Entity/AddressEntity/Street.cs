namespace RealEstateManagement.Data.Entity.AddressEnity
{
    public class Street
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WardId { get; set; }
        public Ward Ward { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }

}
