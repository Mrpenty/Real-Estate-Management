using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Data.Entity.Reviews;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class GetPropertyByIdAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Property_When_Exists()
        {
            // Arrange: seed đủ các nav dễ bị chạm khi map
            var property = new Property
            {
                Id = 1,
                Title = "Test Property",
                Description = "Nice house",
                Status = "Available",
                Location = "City Center",
                CreatedAt = DateTime.UtcNow,
                ViewsCount = 0,
                Bedrooms = 2,
                Bathrooms = 1,
                Floors = 1,

                LandlordId = 10,
                Landlord = new ApplicationUser { Id = 10, Name = "Owner" },

                PropertyTypeId = 1,
                PropertyType = new PropertyType { Id = 1, Name = "House" },

                AddressId = 1,
                Address = new Address
                {
                    Id = 1,
                    Street = new Street { Id = 1, Name = "Main St" },
                    Province = new Province { Id = 1, Name = "Province X" },
                    Ward = new Ward { Id = 1, Name = "Ward Y" },
                    DetailedAddress = "123/4 Main St"
                },

                Images = new List<PropertyImage>
                {
                    new PropertyImage { Url = "cover.jpg", IsPrimary = true }
                },
                Reviews = new List<Review>(),
                PropertyAmenities = new List<PropertyAmenity>(),
                PropertyPromotions = new List<PropertyPromotion>()
            };

            PropertyRepo
                .Setup(r => r.GetPropertyByIdAsync(1))
                .ReturnsAsync(property);

            // Không có favorite
            FavoriteRepo
                .Setup(r => r.GetFavoritePropertyByIdAsync(0, 1))
                .ReturnsAsync((UserFavoriteProperty)null);

            // Act
            var result = await Svc.GetPropertyByIdAsync(1, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test Property", result.Title);
            // Nếu service map PrimaryImageUrl:
            // Assert.AreEqual("cover.jpg", result.PrimaryImageUrl);
        }

        [TestMethod]
        public async Task Returns_Null_When_Not_Found()
        {
            // Arrange
            PropertyRepo
                .Setup(r => r.GetPropertyByIdAsync(999))
                .ReturnsAsync((Property)null);

            FavoriteRepo
                .Setup(r => r.GetFavoritePropertyByIdAsync(0, 999))
                .ReturnsAsync((UserFavoriteProperty)null);

            // Act
            var result = await Svc.GetPropertyByIdAsync(999, 0);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Maps_Important_Fields_Correctly()
        {
            // Arrange: tối thiểu các field mà service hay map
            var property = new Property
            {
                Id = 2,
                Title = "Luxury Apartment",
                Description = "Nice View",
                Price = 2500,
                Area = 75,
                Bedrooms = 3,
                Bathrooms = 2,
                Floors = 1,
                CreatedAt = DateTime.UtcNow,
                Status = "Available",
                Location = "HCMC, Vietnam",

                LandlordId = 11,
                Landlord = new ApplicationUser { Id = 11, Name = "Alice" },

                PropertyTypeId = 2,
                PropertyType = new PropertyType { Id = 2, Name = "Apartment" },

                AddressId = 12,
                Address = new Address
                {
                    Id = 12,
                    Province = new Province { Id = 2, Name = "HCM" },
                    Ward = new Ward { Id = 20, Name = "Ward 2" },
                    Street = new Street { Id = 30, Name = "Street B" },
                    DetailedAddress = "Block B"
                },

                Images = new List<PropertyImage>
                {
                    new PropertyImage { Url = "main.jpg", IsPrimary = true }
                },
                Reviews = new List<Review>(),
                PropertyAmenities = new List<PropertyAmenity>(),
                PropertyPromotions = new List<PropertyPromotion>()
            };

            PropertyRepo
                .Setup(r => r.GetPropertyByIdAsync(2))
                .ReturnsAsync(property);

            FavoriteRepo
                .Setup(r => r.GetFavoritePropertyByIdAsync(0, 2))
                .ReturnsAsync((UserFavoriteProperty)null);

            // Act
            var result = await Svc.GetPropertyByIdAsync(2, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(property.Id, result.Id);
            Assert.AreEqual(property.Title, result.Title);
            Assert.AreEqual(property.Price, result.Price);
            // Nếu service map thêm:
            // Assert.AreEqual("main.jpg", result.PrimaryImageUrl);
            // Assert.AreEqual("Apartment", result.Type);
            // Assert.AreEqual("HCM", result.Province);
            // Assert.AreEqual("Ward 2", result.Ward);
            // Assert.AreEqual("Street B", result.Street);
        }
    }
}
