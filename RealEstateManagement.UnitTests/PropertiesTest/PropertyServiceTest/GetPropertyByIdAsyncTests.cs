using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nest;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.Reviews;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class GetPropertyByIdAsyncTests : PropertyTestBase
    {

        [TestMethod]
        public async Task Returns_Property_When_Exists()
        {
            // Arrange
            var property = new Property
            {
                Id = 1,
                Title = "Test Property",
                Description = "Nice house",
                Status = "Available",
                Location = "City Center",
                AddressId = 1,
                Address = new Address
                {
                    Street = new Street { Name = "Main St" },
                    Province = new Province { Name = "Province X" },
                    Ward = new Ward { Name = "Ward Y" },
                    DetailedAddress = "123/4 Main St"
                }
            };

            PropertyRepo
                .Setup(r => r.GetPropertyByIdAsync(1))
                .ReturnsAsync(property);

            FavoriteRepo
                .Setup(r => r.GetFavoritePropertyByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((UserFavoriteProperty)null); // 👈 quan trọng

            // Act
            var result = await Svc.GetPropertyByIdAsync(1, 0);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test Property", result.Title);
        }


        [TestMethod]
        public async Task Returns_Null_When_Not_Found()
        {
            PropertyRepo
                .Setup(r => r.GetPropertyByIdAsync(999))
                .ReturnsAsync((Property?)null);

            var result = await Svc.GetPropertyByIdAsync(999);

            Assert.IsNull(result);   // ✅ nếu service return null
                                     // hoặc Assert.ThrowsExceptionAsync<KeyNotFoundException>(...) nếu service throw
        }

        [TestMethod]
        public async Task Maps_Important_Fields_Correctly()
        {
            var property = new Property
            {
                Id = 2,
                Title = "Luxury Apartment",
                Description = "Nice View",
                Type = "Apartment",
                Price = 2500,
                Area = 75,
                Bedrooms = 3,
                CreatedAt = DateTime.UtcNow,
                Status = "Available",
                Location = "HCMC, Vietnam",
                Reviews = new List<Review>(),
                Images = new List<PropertyImage>(),
                PropertyAmenities = new List<PropertyAmenity>(),
                PropertyPromotions = new List<PropertyPromotion>()
            };

            PropertyRepo
                .Setup(r => r.GetPropertyByIdAsync(2))
                .ReturnsAsync(property);
            FavoriteRepo
                .Setup(r => r.GetFavoritePropertyByIdAsync(0, 2))
                .ReturnsAsync((UserFavoriteProperty)null);
            var result = await Svc.GetPropertyByIdAsync(2);

            Assert.IsNotNull(result);
            Assert.AreEqual(property.Id, result.Id);
            Assert.AreEqual(property.Title, result.Title);
            Assert.AreEqual(property.Type, result.Type);
            Assert.AreEqual(property.Price, result.Price);
        }


    }
}
