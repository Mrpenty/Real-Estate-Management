using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Data.Entity.AddressEnity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Business.Repositories.FavoriteRepository;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class GetUserProfileWithPropertiesAsyncTests
    {
        private Mock<IPropertyRepository> _mockRepo;
        private Mock<IFavoriteRepository> _mockFavRepo;
        private PropertyService _svc;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IPropertyRepository>(MockBehavior.Strict);
            _mockFavRepo = new Mock<IFavoriteRepository>(MockBehavior.Strict);

            _svc = new PropertyService(_mockRepo.Object, null, null);
        }

        [TestMethod]
        public async Task Returns_Null_When_User_Not_Found()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetUserByIdAsync(10)).ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await _svc.GetUserProfileWithPropertiesAsync(10);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Returns_UserProfile_With_Properties()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = 1,
                Name = "Alice",
                PhoneNumber = "123",
                Email = "alice@test.com",
                ProfilePictureUrl = "ava.png"
            };

            var properties = new List<Property>
            {
                new Property
                {
                    Id = 101,
                    Title = "Villa",
                    Description = "Nice Villa",
                    Type = "House",
                    AddressId = 201,
                    Area = 150,
                    Bedrooms = 4,
                    Price = 3000,
                    Status = "Available",
                    CreatedAt = DateTime.UtcNow,
                    ViewsCount = 50,
                    LandlordId = 1,
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

            _mockRepo.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);
            _mockRepo.Setup(r => r.GetPropertiesByLandlordIdAsync(1)).ReturnsAsync(properties);
            _mockFavRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>())).ReturnsAsync(new List<Property>());

            // Act
            var result = await _svc.GetUserProfileWithPropertiesAsync(1, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Alice", result.Name);
            Assert.AreEqual("Villa", result.Properties.First().Title);
            Assert.AreEqual("HN", result.Properties.First().Province);
            Assert.AreEqual("Pool", result.Properties.First().Amenities.First());
            Assert.AreEqual("Gold", result.Properties.First().PromotionPackageName);
        }

        [TestMethod]
        public async Task Handles_Null_Optional_Fields()
        {
            // Arrange
            var user = new ApplicationUser { Id = 3, Name = "Charlie" };
            var props = new List<Property>
            {
                new Property
                {
                    Id = 301,
                    Title = "Simple House",
                    LandlordId = 3,
                    AddressId = 10,
                    Address = null, // null Address
                    Images = null, // null Images
                    PropertyAmenities = null, // null Amenities
                    PropertyPromotions = null // null Promotions
                }
            };

            _mockRepo.Setup(r => r.GetUserByIdAsync(3)).ReturnsAsync(user);
            _mockRepo.Setup(r => r.GetPropertiesByLandlordIdAsync(3)).ReturnsAsync(props);
            _mockFavRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>())).ReturnsAsync(new List<Property>());

            // Act
            var result = await _svc.GetUserProfileWithPropertiesAsync(3);

            // Assert
            Assert.IsNotNull(result);
            var dto = result.Properties.First();
            Assert.IsNull(dto.Province);
            Assert.AreEqual(0, dto.Amenities.Count);
            Assert.IsNull(dto.PromotionPackageName);
            Assert.IsNull(dto.PrimaryImageUrl);
        }
    }
}
