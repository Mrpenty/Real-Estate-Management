using RealEstateManagement.Data.Entity.PropertyEntity;


namespace RealEstateManagement.Data.Entity.AddressEnity
{
    public class Address
    {
        public int Id { get; set; }
        public int? ProvinceId { get; set; } // Tỉnh/Thành
        public int? WardId { get; set; } // Phường/Xã
        public int? StreetId { get; set; } // Đường/Phố
        public string DetailedAddress { get; set; } // Thông tin chi tiết (số nhà, v.v.)

        // Quan hệ với các bảng tham chiếu
        public Province Province { get; set; }
        public Ward Ward { get; set; }
        public Street Street { get; set; }

        // Quan hệ với Property
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }

}
