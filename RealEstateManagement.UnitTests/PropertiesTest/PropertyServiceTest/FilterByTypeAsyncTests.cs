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
    public class FilterByTypeAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Properties_When_Found()
        {
            var props = new List<Property>
            {
                new Property
                {
                    Id = 1,
                    Title = "Nice House",
                    Description = "Desc",
                    Area = 70,
                    Bedrooms = 2,
                    Bathrooms = 1,
                    Floors = 1,
                    Price = 500,
                    Status = "Available",
                    Location = "HN",
                    CreatedAt = DateTime.UtcNow,
                    ViewsCount = 100,
                    LandlordId = 101,
                    PropertyTypeId = 1,
                    PropertyType = new PropertyType { Id = 1, Name = "Apartment" },
                    AddressId = 11,
                    Address = new Address
                    {
                        Id = 11,
                        ProvinceId = 1, Province = new Province { Id = 1, Name = "HN" },
                        WardId = 2,     Ward     = new Ward { Id = 2, Name = "Ward1" },
                        StreetId = 3,   Street   = new Street { Id = 3, Name = "Street A" },
                        DetailedAddress = "No. 1"
                    },
                    Images = new List<PropertyImage> { new PropertyImage { Url = "img.jpg", IsPrimary = true } },
                    Landlord = new ApplicationUser { Id = 101, Name = "Mr A", PhoneNumber = "123", ProfilePictureUrl = "avatar.png" },
                    PropertyAmenities = new List<PropertyAmenity> { new PropertyAmenity { Amenity = new Amenity { Name = "Pool" } } },
                    PropertyPromotions = new List<PropertyPromotion>
                    {
                        new PropertyPromotion
                        {
                            PromotionPackage = new RealEstateManagement.Data.Entity.Payment.PromotionPackage
                            { Name = "Gold", Level = 2 }
                        }
                    }
                }
            };

            PropertyRepo.Setup(r => r.FilterByTypeAsync("Apartment")).ReturnsAsync(props);

            var result = (await Svc.FilterByTypeAsync("Apartment")).ToList();

            Assert.AreEqual(1, result.Count);
            var dto = result[0];
            Assert.AreEqual("Nice House", dto.Title);
            Assert.AreEqual("Gold", dto.PromotionPackageName);
            Assert.AreEqual("Pool", dto.Amenities.First());
            Assert.AreEqual("Mr A", dto.LandlordName);
            Assert.AreEqual("img.jpg", dto.PrimaryImageUrl);

            Assert.IsNull(dto.Street);
        }

        [TestMethod]
        public async Task Returns_Empty_When_No_Properties()
        {
            PropertyRepo.Setup(r => r.FilterByTypeAsync("Villa")).ReturnsAsync(new List<Property>());

            var result = await Svc.FilterByTypeAsync("Villa");

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
                    Title = "No Landlord",
                    Area = 50,
                    Bedrooms = 1,
                    Bathrooms = 1,
                    Floors = 1,
                    Price = 100,
                    Status = "Pending",
                    Location = "HCM",
                    CreatedAt = DateTime.UtcNow,
                    ViewsCount = 10,
                    LandlordId = 0,
                    PropertyTypeId = 1,
                    PropertyType = new PropertyType { Id = 1, Name = "Apartment" },
                    AddressId = 12,
                    Address = new Address
                    {
                        Id = 12,
                        ProvinceId = 1, Province = new Province { Id = 1, Name = "HCM" },
                        WardId = 2,     Ward     = new Ward { Id = 2, Name = "Ward2" },
                        StreetId = 3,   Street   = new Street { Id = 3, Name = "Street B" },
                        DetailedAddress = "No. 2"
                    },
                    Images = null,
                    Landlord = null,
                    PropertyAmenities = null,
                    PropertyPromotions = null
                }
            };

            PropertyRepo.Setup(r => r.FilterByTypeAsync("Apartment")).ReturnsAsync(props);

            var dto = (await Svc.FilterByTypeAsync("Apartment")).First();

            Assert.AreEqual(2, dto.Id);
            Assert.IsNull(dto.LandlordName);
            Assert.IsNull(dto.PrimaryImageUrl);
            Assert.AreEqual(0, dto.Amenities.Count);
            Assert.IsNull(dto.PromotionPackageName);


            Assert.IsNull(dto.Street);

        }
    }
}
