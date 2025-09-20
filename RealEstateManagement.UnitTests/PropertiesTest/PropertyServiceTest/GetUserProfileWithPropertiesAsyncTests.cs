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
    public class GetUserProfileWithPropertiesAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Null_When_User_Not_Found()
        {
            // Arrange
            PropertyRepo.Setup(r => r.GetUserByIdAsync(10))
                        .ReturnsAsync((ApplicationUser)null);

            // Act
            var result = await Svc.GetUserProfileWithPropertiesAsync(10, null);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Returns_UserProfile_With_Properties()
        {
            // Arrange: user + 1 bất động sản có đủ nav để tránh NRE
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
                    Area = 150,
                    Bedrooms = 4,
                    Bathrooms = 3,
                    Floors = 2,
                    Price = 3000,
                    Status = "Available",
                    Location = "HN",
                    CreatedAt = DateTime.UtcNow,
                    ViewsCount = 50,

                    LandlordId = 1,
                    Landlord = user,

                    PropertyTypeId = 7,
                    PropertyType = new PropertyType { Id = 7, Name = "Villa" },

                    AddressId = 201,
                    Address = new Address
                    {
                        Id = 201,
                        ProvinceId = 1, Province = new Province { Id = 1, Name = "HN" },
                        WardId = 2,     Ward     = new Ward     { Id = 2, Name = "Ward 1" },
                        StreetId = 3,   Street   = new Street   { Id = 3, Name = "Street A" },
                        DetailedAddress = "123 Street A"
                    },

                    Images = new List<PropertyImage> { new PropertyImage { Url = "img.jpg", IsPrimary = true } },
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

            PropertyRepo.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);
            PropertyRepo.Setup(r => r.GetPropertiesByLandlordIdAsync(1)).ReturnsAsync(properties);

            // Nếu service có đánh dấu IsFavorite cho viewer → mock repo trả rỗng
            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());

            // Act (viewerUserId có thể null hoặc 0 tuỳ service)
            var result = await Svc.GetUserProfileWithPropertiesAsync(1, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Alice", result.Name);

            var dto = result.Properties.First();
            Assert.AreEqual("Villa", dto.Title);
            Assert.AreEqual("HN", dto.Province);
            Assert.AreEqual("Ward 1", dto.Ward);
            Assert.AreEqual("Street A", dto.Street);
            Assert.AreEqual("Gold", dto.PromotionPackageName);
            Assert.AreEqual("Pool", dto.Amenities.First());
            Assert.AreEqual("img.jpg", dto.PrimaryImageUrl);
        }

        [TestMethod]
        public async Task Handles_Null_Optional_Fields()
        {
            // Arrange: user hợp lệ
            var user = new ApplicationUser { Id = 3, Name = "Charlie" };

            // Để tránh NRE trong service khi map, KHÔNG để Address = null;
            // dùng Address "rỗng an toàn" (các name có thể null).
            var props = new List<Property>
            {
                new Property
                {
                    Id = 301,
                    Title = "Simple House",
                    LandlordId = 3,
                    Bedrooms = 2,
                    Bathrooms = 1,
                    Floors = 1,
                    Price = 10,
                    Status = "Pending",
                    Location = "HCM",
                    CreatedAt = DateTime.UtcNow,
                    ViewsCount = 0,

                    PropertyTypeId = 1,
                    PropertyType = new PropertyType { Id = 1, Name = "House" },

                    AddressId = 10,
                    Address = new Address
                    {
                        Id = 10,
                        ProvinceId = 0, Province = new Province { Id = 0, Name = null },
                        WardId = 0,     Ward     = new Ward     { Id = 0, Name = null },
                        StreetId = 0,   Street   = new Street   { Id = 0, Name = null },
                        DetailedAddress = null
                    },

                    Images = null,                 // null → PrimaryImageUrl phải null-safe
                    PropertyAmenities = null,      // null → DTO.Amenities = []
                    PropertyPromotions = null      // null → DTO.PromotionPackageName = null
                }
            };

            PropertyRepo.Setup(r => r.GetUserByIdAsync(3)).ReturnsAsync(user);
            PropertyRepo.Setup(r => r.GetPropertiesByLandlordIdAsync(3)).ReturnsAsync(props);
            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());

            // Act
            var result = await Svc.GetUserProfileWithPropertiesAsync(3, null);

            // Assert
            Assert.IsNotNull(result);
            var dto = result.Properties.First();
            Assert.AreEqual(301, dto.Id);
            Assert.IsNull(dto.Province);
            Assert.IsNull(dto.Ward);
            Assert.IsNull(dto.Street);
            Assert.IsNull(dto.PrimaryImageUrl);
            Assert.AreEqual(0, dto.Amenities.Count);
            Assert.IsNull(dto.PromotionPackageName);
        }
    }
}
