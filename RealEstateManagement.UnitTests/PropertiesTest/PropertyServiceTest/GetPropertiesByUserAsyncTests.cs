using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class GetPropertiesByUserAsyncTests
    {
        private Mock<IPropertyRepository> _mockRepo;
        private PropertyService _svc;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IPropertyRepository>(MockBehavior.Strict);
            _svc = new PropertyService(_mockRepo.Object, null, null);
        }

        [TestMethod]
        public async Task Returns_Properties_When_UserId_Matches()
        {
            // Arrange
            var props = new List<Property>
            {
                new Property
                {
                    Id = 1,
                    Title = "House 1",
                    Description = "Desc 1",
                    Type = "Apartment",
                    AddressId = 11,
                    Area = 80,
                    Bedrooms = 3,
                    Price = 1000,
                    Status = "Available",
                    Location = "HN",
                    CreatedAt = DateTime.UtcNow,
                    ViewsCount = 123,
                    LandlordId = 99,
                    Landlord = new ApplicationUser { Name = "Mr A", PhoneNumber = "123", ProfilePictureUrl = "ava.png" },
                    Address = new Address
                    {
                        ProvinceId = 1, Province = new Province { Name = "HN" },
                        WardId = 2, Ward = new Ward { Name = "Ward 1" },
                        StreetId = 3, Street = new Street { Name = "Street A" },
                        DetailedAddress = "123 Street A"
                    },
                    Images = new List<PropertyImage> { new PropertyImage { Url = "img.jpg", IsPrimary = true } },
                    PropertyAmenities = new List<PropertyAmenity> { new PropertyAmenity { Amenity = new Amenity { Name = "Pool" } } },
                    PropertyPromotions = new List<PropertyPromotion>
                    {
                        new PropertyPromotion { PromotionPackage = new Data.Entity.Payment.PromotionPackage { Name = "Gold", Level = 2 } }
                    }
                }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(props);

            // Act
            var result = (await _svc.GetPropertiesByUserAsync(99)).ToList();

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
            // Arrange
            var props = new List<Property>
            {
                new Property { Id = 1, LandlordId = 100 },
                new Property { Id = 2, LandlordId = 200 }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(props);

            // Act
            var result = await _svc.GetPropertiesByUserAsync(999);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Handles_Null_Optional_Fields()
        {
            // Arrange
            var props = new List<Property>
            {
                new Property
                {
                    Id = 2,
                    Title = "House 2",
                    LandlordId = 77,
                    AddressId = 12,
                    Area = 55,
                    Bedrooms = 2,
                    Price = 200,
                    Status = "Pending",
                    Location = "HCM",
                    CreatedAt = DateTime.UtcNow,
                    ViewsCount = 10,
                    Landlord = null,
                    Address = new Address
                    {
                        ProvinceId = 1, Province = new Province { Name = "HCM" },
                        WardId = 2, Ward = new Ward { Name = "Ward 2" },
                        StreetId = 3, Street = new Street { Name = "Street B" },
                        DetailedAddress = "456 Street B"
                    },
                    Images = null,
                    PropertyAmenities = null,
                    PropertyPromotions = null
                }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(props);

            // Act
            var result = (await _svc.GetPropertiesByUserAsync(77)).First();

            // Assert
            Assert.IsNull(result.LandlordName);
            Assert.IsNull(result.PrimaryImageUrl);
            Assert.AreEqual(0, result.Amenities.Count);
            Assert.IsNull(result.PromotionPackageName);
        }
    }
}
