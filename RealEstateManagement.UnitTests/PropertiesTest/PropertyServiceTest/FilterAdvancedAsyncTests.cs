using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.Reviews;
using RealEstateManagement.Data.Entity.AddressEnity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateManagement.Data.Entity;

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
                    Bathrooms = 1,
                    Floors = 1,
                    ViewsCount = 0,
                    Status = "Active",
                    Location = "HCMC",
                    CreatedAt = System.DateTime.UtcNow,
                    LandlordId = 123,
                    PropertyTypeId = 1,
                    PropertyType = new PropertyType { Id = 1, Name = "Apartment" },
                    AddressId = 1,
                    Address = new Address
                    {
                        Id = 1,
                        Province = new Province { Name = "HCM" },
                        Ward = new Ward { Name = "W1" },
                        Street = new Street { Name = "S1" },
                        DetailedAddress = "D1"
                    },
                    Images = new List<PropertyImage>
                    {
                        new PropertyImage { Url = "apt.jpg", IsPrimary = true }
                    },
                    Landlord = new RealEstateManagement.Data.Entity.User.ApplicationUser { Name = "Alice", PhoneNumber = "123456" },
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        new PropertyAmenity { Amenity = new Amenity { Name = "Gym" } }
                    },
                    Reviews = new List<Review>()
                }
            };

            PropertyRepo.Setup(r => r.FilterAdvancedAsync(It.IsAny<PropertyFilterDTO>()))
                        .ReturnsAsync(props);

            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());

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
                        .ReturnsAsync(new List<Property>());

            // Act
            var result = await Svc.FilterAdvancedAsync(filter);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Throws_ArgumentNull_When_Repository_Returns_Null()
        {
            // Arrange
            var filter = new PropertyFilterDTO { UserId = 0 };

            PropertyRepo.Setup(r => r.FilterAdvancedAsync(It.IsAny<PropertyFilterDTO>()))
                        .ReturnsAsync((IEnumerable<Property>)null);

            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());

            // Act + Assert
            await Assert.ThrowsExceptionAsync<System.ArgumentNullException>(() => Svc.FilterAdvancedAsync(filter));
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
                    Bathrooms = 4,
                    Floors = 2,
                    ViewsCount = 0,
                    Status = "Active",
                    Location = "DN",
                    CreatedAt = System.DateTime.UtcNow,
                    LandlordId = 1,
                    PropertyTypeId = 2,
                    PropertyType = new PropertyType { Id = 2, Name = "Villa" },
                    AddressId = 10,
                    Address = new Address
                    {
                        Id = 10,
                        Province = new Province { Name = "DN" },
                        Ward = new Ward { Name = "W2" },
                        Street = new Street { Name = "S2" },
                        DetailedAddress = "D2"
                    },
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        new PropertyAmenity { Amenity = new Amenity { Name = "Pool" } }
                    },
                    Images = new List<PropertyImage>(),
                    Reviews = new List<Review>()
                },
                new Property
                {
                    Id = 11,
                    Title = "Small House",
                    Area = 80,
                    Price = 100000,
                    Bedrooms = 2,
                    Bathrooms = 1,
                    Floors = 1,
                    ViewsCount = 0,
                    Status = "Active",
                    Location = "HN",
                    CreatedAt = System.DateTime.UtcNow,
                    LandlordId = 2,
                    PropertyTypeId = 1,
                    PropertyType = new PropertyType { Id = 1, Name = "House" },
                    AddressId = 11,
                    Address = new Address
                    {
                        Id = 11,
                        Province = new Province { Name = "HN" },
                        Ward = new Ward { Name = "W3" },
                        Street = new Street { Name = "S3" },
                        DetailedAddress = "D3"
                    },
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        new PropertyAmenity { Amenity = new Amenity { Name = "Garden" } }
                    },
                    Images = new List<PropertyImage>(),
                    Reviews = new List<Review>()
                }
            };

            PropertyRepo.Setup(r => r.FilterAdvancedAsync(It.IsAny<PropertyFilterDTO>()))
                        .ReturnsAsync(props);

            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());

            // Act
            var result = (await Svc.FilterAdvancedAsync(filter)).ToList();

            // Assert (service không tự lọc amenities – repo chịu trách nhiệm)
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(r => r.Amenities.Contains("Pool") || r.Amenities.Contains("Garden")));
        }
    }
}
