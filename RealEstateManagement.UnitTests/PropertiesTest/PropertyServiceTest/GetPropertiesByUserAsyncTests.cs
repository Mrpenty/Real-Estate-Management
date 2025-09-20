using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class GetPropertiesByUserAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Properties_When_UserId_Matches()
        {
            // Arrange: seed đủ nav để mapping không NRE
            var props = new List<Property>
            {
                new Property
                {
                    Id = 1,
                    Title = "House 1",
                    Description = "Desc 1",
                    Area = 80,
                    Bedrooms = 3,
                    Bathrooms = 2,
                    Floors = 2,
                    Price = 1000,
                    Status = "Available",
                    Location = "HN",
                    CreatedAt = DateTime.UtcNow,
                    ViewsCount = 123,

                    LandlordId = 99,
                    Landlord = new ApplicationUser { Id = 99, Name = "Mr A", PhoneNumber = "123", ProfilePictureUrl = "ava.png" },

                    PropertyTypeId = 1,
                    PropertyType = new PropertyType { Id = 1, Name = "House" },

                    AddressId = 11,
                    Address = new Address
                    {
                        Id = 11,
                        ProvinceId = 1, Province = new Province { Id = 1, Name = "HN" },
                        WardId = 2,     Ward     = new Ward { Id = 2, Name = "Ward 1" },
                        StreetId = 3,   Street   = new Street { Id = 3, Name = "Street A" },
                        DetailedAddress = "123 Street A"
                    },

                    Images = new List<PropertyImage> { new PropertyImage { Url = "img.jpg", IsPrimary = true } },
                    PropertyAmenities = new List<PropertyAmenity> { new PropertyAmenity { Amenity = new Amenity { Name = "Pool" } } },
                    PropertyPromotions = new List<PropertyPromotion>
                    {
                        new PropertyPromotion
                        {
                            PromotionPackage =
                                new RealEstateManagement.Data.Entity.Payment.PromotionPackage { Name = "Gold", Level = 2 }
                        }
                    }
                }
            };

            PropertyRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(props);
            // nếu service có gọi favorites -> trả về rỗng cho an toàn
            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());

            // Act
            var result = (await Svc.GetPropertiesByUserAsync(99)).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            var dto = result[0];
            Assert.AreEqual("House 1", dto.Title);
            Assert.AreEqual("HN", dto.Province);
            Assert.AreEqual("Ward 1", dto.Ward);
            Assert.AreEqual("Street A", dto.Street);
            Assert.AreEqual("Gold", dto.PromotionPackageName);
            Assert.AreEqual("Pool", dto.Amenities.First());
            Assert.AreEqual("Mr A", dto.LandlordName);
            Assert.AreEqual("img.jpg", dto.PrimaryImageUrl);
        }

        [TestMethod]
        public async Task Returns_Empty_When_No_Properties_For_User()
        {
            var props = new List<Property>
            {
                new Property { Id = 1, LandlordId = 100, AddressId = 1, Address = new Address(), PropertyTypeId = 1, PropertyType = new PropertyType(), Images = new List<PropertyImage>(), PropertyAmenities = new List<PropertyAmenity>(), PropertyPromotions = new List<PropertyPromotion>() },
                new Property { Id = 2, LandlordId = 200, AddressId = 2, Address = new Address(), PropertyTypeId = 1, PropertyType = new PropertyType(), Images = new List<PropertyImage>(), PropertyAmenities = new List<PropertyAmenity>(), PropertyPromotions = new List<PropertyPromotion>() }
            };

            PropertyRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(props);
            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());

            var result = await Svc.GetPropertiesByUserAsync(999);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Handles_Null_Optional_Fields()
        {
            var props = new List<Property>
            {
                new Property
                {
                    Id = 2,
                    Title = "House 2",
                    Area = 55,
                    Bedrooms = 2,
                    Bathrooms = 1,
                    Floors = 1,
                    Price = 200,
                    Status = "Pending",
                    Location = "HCM",
                    CreatedAt = DateTime.UtcNow,
                    ViewsCount = 10,

                    LandlordId = 77,
                    Landlord = null, // optional null

                    PropertyTypeId = 1,
                    PropertyType = new PropertyType { Id = 1, Name = "Apartment" },

                    AddressId = 12,
                    Address = new Address
                    {
                        Id = 12,
                        ProvinceId = 1, Province = new Province { Id = 1, Name = "HCM" },
                        WardId = 2,     Ward     = new Ward { Id = 2, Name = "Ward 2" },
                        StreetId = 3,   Street   = new Street { Id = 3, Name = "Street B" },
                        DetailedAddress = "456 Street B"
                    },

                    Images = null,                    // optional null
                    PropertyAmenities = null,         // optional null
                    PropertyPromotions = null         // optional null
                }
            };

            PropertyRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(props);
            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());

            var dto = (await Svc.GetPropertiesByUserAsync(77)).First();

            Assert.AreEqual(2, dto.Id);
            Assert.IsNull(dto.LandlordName);
            Assert.IsNull(dto.PrimaryImageUrl);
            Assert.AreEqual(0, dto.Amenities.Count);
            Assert.IsNull(dto.PromotionPackageName);
            // địa chỉ vẫn có vì đã seed Address
            Assert.AreEqual("HCM", dto.Province);
            Assert.AreEqual("Ward 2", dto.Ward);
            Assert.AreEqual("Street B", dto.Street);
        }
    }
}
