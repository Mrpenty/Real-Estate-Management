using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class FilterByAreaAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Properties_Within_Range()
        {
            // Arrange
            var props = new List<Property>
            {
                new Property { Id = 1, Title = "Small House", Area = 50, Price = 100000 },
                new Property { Id = 2, Title = "Big House", Area = 150, Price = 200000 }
            };

            PropertyRepo.Setup(r => r.FilterByAreaAsync(40, 200))
                        .ReturnsAsync(props);

            // Act
            var result = (await Svc.FilterByAreaAsync(40, 200)).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Small House", result[0].Title);
            Assert.AreEqual("Big House", result[1].Title);
        }

        [TestMethod]
        public async Task Returns_Empty_When_No_Properties_Exist()
        {
            // Arrange
            PropertyRepo.Setup(r => r.FilterByAreaAsync(60, 120))
                        .ReturnsAsync(new List<Property>());

            // Act
            var result = await Svc.FilterByAreaAsync(60, 120);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Handles_Null_From_Repository()
        {
            // Arrange
            PropertyRepo.Setup(r => r.FilterByAreaAsync(It.IsAny<decimal?>(), It.IsAny<decimal?>()))
                        .ReturnsAsync((IEnumerable<Property>)null);

            // Act
            var result = await Svc.FilterByAreaAsync(80, 200);

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
                    Id = 10,
                    Title = "Luxury Villa",
                    Description = "Sea view villa",
                    Area = 300,
                    Price = 1000000,
                    Bedrooms = 5,
                    AddressId = 9,
                    Status = "Available",
                    CreatedAt = DateTime.UtcNow,
                    ViewsCount = 50,
                    Images = new List<PropertyImage>
                    {
                        new PropertyImage { Url = "villa.jpg", IsPrimary = true }
                    },
                    Landlord = new Data.Entity.User.ApplicationUser { Name = "Mr C", PhoneNumber = "987654", ProfilePictureUrl = "landlord.jpg" },
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        new PropertyAmenity { Amenity = new Amenity { Name = "Gym" } }
                    },
                    PropertyPromotions = new List<PropertyPromotion>
                    {
                        new PropertyPromotion
                        {
                            PromotionPackage = new Data.Entity.Payment.PromotionPackage { Name = "VIP", Level = 3 }
                        }
                    }
                }
            };

            PropertyRepo.Setup(r => r.FilterByAreaAsync(200, 400))
                        .ReturnsAsync(props);

            // Act
            var result = (await Svc.FilterByAreaAsync(200, 400)).First();

            // Assert
            Assert.AreEqual(10, result.Id);
            Assert.AreEqual("Luxury Villa", result.Title);
            Assert.AreEqual("VIP", result.PromotionPackageName);
            Assert.AreEqual("Gym", result.Amenities.First());
            Assert.AreEqual("villa.jpg", result.PrimaryImageUrl);
            Assert.AreEqual("Mr C", result.LandlordName);
        }

        [TestMethod]
        public async Task Returns_Only_Within_Bounds()
        {
            // Arrange
            var props = new List<Property>
            {
                new Property { Id = 1, Title = "Tiny", Area = 20 },
                new Property { Id = 2, Title = "Medium", Area = 100 },
                new Property { Id = 3, Title = "Huge", Area = 500 }
            };

            PropertyRepo.Setup(r => r.FilterByAreaAsync(50, 200))
                        .ReturnsAsync(props.Where(p => p.Area >= 50 && p.Area <= 200).ToList());

            // Act
            var result = (await Svc.FilterByAreaAsync(50, 200)).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Medium", result[0].Title);
        }
    }
}
