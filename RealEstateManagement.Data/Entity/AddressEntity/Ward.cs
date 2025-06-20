namespace RealEstateManagement.Data.Entity.AddressEnity
{
    public class Ward
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public Province Province { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }

}
