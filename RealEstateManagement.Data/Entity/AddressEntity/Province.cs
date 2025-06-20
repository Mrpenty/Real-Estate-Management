namespace RealEstateManagement.Data.Entity.AddressEnity
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }

}
