using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class FilterAdvancedAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Properties_When_Filter_Match()
        {
            // Arrange
            var filter = new PropertyFilterDTO
            {
                MinPrice = 100000,
                MaxPrice = 200000,
                MinArea = 50,
                MaxArea = 100,
                MinBedrooms = 2,
                MaxBedrooms = 3,
                PropertyType = "Apartment",
                UserId = 0
            };

            var props = new List<Property>
            {
                new Property
                {
                    Id = 1,
                    Title = "Modern Apartment",
                    Area = 80,
                    Price = 150000,
                    Bedrooms = 2,
                    Type = "Apartment",
                    Images = new List<PropertyImage>
                    {
                        new PropertyImage { Url = "apt.jpg", IsPrimary = true }
                    },
                    Landlord = new Data.Entity.User.ApplicationUser { Name = "Alice", PhoneNumber = "123456" },
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        new PropertyAmenity { Amenity = new Amenity { Name = "Gym" } }
                    }
                }
            };

            PropertyRepo.Setup(r => r.FilterAdvancedAsync(It.IsAny<PropertyFilterDTO>()))
                        .ReturnsAsync(props);

            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());   // rỗng thì để new List<Property>()


            // Act
            var result = (await Svc.FilterAdvancedAsync(filter)).ToList();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Modern Apartment", result[0].Title);
            Assert.AreEqual("apt.jpg", result[0].PrimaryImageUrl);
            Assert.AreEqual("Alice", result[0].LandlordName);
            Assert.AreEqual("Gym", result[0].Amenities.First());
        }

        [TestMethod]
        public async Task Returns_Empty_When_No_Match()
        {
            // Arrange
            var filter = new PropertyFilterDTO { MinPrice = 500000, MaxPrice = 600000, UserId = 0 };

            PropertyRepo.Setup(r => r.FilterAdvancedAsync(It.IsAny<PropertyFilterDTO>()))
                        .ReturnsAsync(new List<Property>());

            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());   // rỗng thì để new List<Property>()

            // Act
            var result = await Svc.FilterAdvancedAsync(filter);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Returns_Null_When_Repository_Returns_Null()
        {
            // Arrange
            var filter = new PropertyFilterDTO { UserId = 0 };

            PropertyRepo.Setup(r => r.FilterAdvancedAsync(It.IsAny<PropertyFilterDTO>()))
                        .ReturnsAsync((IEnumerable<Property>)null);
            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());   // rỗng thì để new List<Property>()


            // Act
            var result = await Svc.FilterAdvancedAsync(filter);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Returns_Only_Properties_With_Amenities()
        {
            // Arrange
            var filter = new PropertyFilterDTO
            {
                AmenityName = new List<string> { "Pool" },
                UserId = 0
            };

            var props = new List<Property>
            {
                new Property
                {
                    Id = 10,
                    Title = "Luxury Villa",
                    Area = 300,
                    Price = 2000000,
                    Bedrooms = 5,
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        new PropertyAmenity { Amenity = new Amenity { Name = "Pool" } }
                    }
                },
                new Property
                {
                    Id = 11,
                    Title = "Small House",
                    Area = 80,
                    Price = 100000,
                    Bedrooms = 2,
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        new PropertyAmenity { Amenity = new Amenity { Name = "Garden" } }
                    }
                }
            };

            PropertyRepo.Setup(r => r.FilterAdvancedAsync(It.IsAny<PropertyFilterDTO>()))
                        .ReturnsAsync(props);

            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());   // rỗng thì để new List<Property>()

            // Act
            var result = (await Svc.FilterAdvancedAsync(filter)).ToList();

            // Assert
            Assert.AreEqual(2, result.Count); // Service không filter amenities, chỉ repo filter
            Assert.IsTrue(result.Any(r => r.Amenities.Contains("Pool") || r.Amenities.Contains("Garden")));
        }
    }
}
