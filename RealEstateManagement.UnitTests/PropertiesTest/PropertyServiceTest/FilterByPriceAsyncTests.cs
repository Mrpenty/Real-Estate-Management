using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class FilterByPriceAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Properties_Within_Range()
        {
            // Arrange
            var props = new List<Property>
            {
                new Property { Id = 1, Title = "House A", Price = 200000, Area = 50, Bedrooms = 2, AddressId = 1, Status = "Available", CreatedAt = DateTime.UtcNow },
                new Property { Id = 2, Title = "House B", Price = 800000, Area = 70, Bedrooms = 3, AddressId = 2, Status = "Available", CreatedAt = DateTime.UtcNow }
            };

            PropertyRepo.Setup(r => r.FilterByPriceAsync(100000, 900000))
                        .ReturnsAsync(props);

            // Act
            var result = (await Svc.FilterByPriceAsync(100000, 900000)).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("House A", result[0].Title);
            Assert.AreEqual("House B", result[1].Title);
        }

        [TestMethod]
        public async Task Returns_Empty_When_No_Properties_Exist()
        {
            // Arrange
            PropertyRepo.Setup(r => r.FilterByPriceAsync(100000, 900000))
                        .ReturnsAsync(new List<Property>());

            // Act
            var result = await Svc.FilterByPriceAsync(100000, 900000);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Handles_Null_From_Repository()
        {
            // Arrange
            PropertyRepo.Setup(r => r.FilterByPriceAsync(It.IsAny<decimal?>(), It.IsAny<decimal?>()))
                        .ReturnsAsync((IEnumerable<Property>)null);

            // Act
            var result = await Svc.FilterByPriceAsync(500000, 1000000);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Maps_Important_Fields_Correctly()
        {
            // Arrange
            var props = new List<Property>
            {
                new Property
                {
                    Id = 2,
                    Title = "Luxury House",
                    Description = "Big house",
                    Price = 500000,
                    Area = 120,
                    Bedrooms = 4,
                    AddressId = 3,
                    Status = "Available",
                    CreatedAt = DateTime.UtcNow,
                    ViewsCount = 99,
                    Images = new List<PropertyImage>
                    {
                        new PropertyImage { Url = "main.jpg", IsPrimary = true }
                    },
                    Landlord = new Data.Entity.User.ApplicationUser { Name = "Mr B", PhoneNumber = "123456", ProfilePictureUrl = "avatar.jpg" },
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        new PropertyAmenity { Amenity = new Amenity { Name = "Pool" } }
                    },
                    PropertyPromotions = new List<PropertyPromotion>
                    {
                        new PropertyPromotion
                        {
                            PromotionPackage = new Data.Entity.Payment.PromotionPackage { Name = "Premium", Level = 2 }
                        }
                    }
                }
            };

            PropertyRepo.Setup(r => r.FilterByPriceAsync(400000, 600000))
                        .ReturnsAsync(props);

            // Act
            var result = (await Svc.FilterByPriceAsync(400000, 600000)).First();

            // Assert
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("Luxury House", result.Title);
            Assert.AreEqual("Premium", result.PromotionPackageName);
            Assert.AreEqual("Pool", result.Amenities.First());
            Assert.AreEqual("main.jpg", result.PrimaryImageUrl);
            Assert.AreEqual("Mr B", result.LandlordName);
        }

        [TestMethod]
        public async Task Returns_Only_Within_Bounds()
        {
            // Arrange
            var props = new List<Property>
            {
                new Property { Id = 1, Title = "Cheap", Price = 100000 },
                new Property { Id = 2, Title = "Mid", Price = 500000 },
                new Property { Id = 3, Title = "Expensive", Price = 1000000 }
            };

            PropertyRepo.Setup(r => r.FilterByPriceAsync(200000, 800000))
                        .ReturnsAsync(props.Where(p => p.Price >= 200000 && p.Price <= 800000).ToList());

            // Act
            var result = (await Svc.FilterByPriceAsync(200000, 800000)).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Mid", result[0].Title);
        }
    }
}
