using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
//using RealEstateManagement.Business.Services.PropertyService;
//using RealEstateManagement.Business.Repositories.PropertyRepository;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.Reviews;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class ComparePropertiesAsyncTests
    {
        private Mock<IPropertyRepository> _mockRepo;
        private PropertyService _svc;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IPropertyRepository>(MockBehavior.Strict);
            _svc = new PropertyService(_mockRepo.Object, null!, null!); // tuỳ constructor bạn
        }

        [TestMethod]
        public async Task Returns_Comparison_Result_When_Ids_Exist()
        {
            // Arrange
            var props = new List<Property>
            {
                new Property { Id = 1, Title = "House A", Price = 100, Area = 50, Bedrooms = 2, Reviews = new List<Review>(), Images = new List<PropertyImage>(), PropertyAmenities = new List<PropertyAmenity>() },
                new Property { Id = 2, Title = "House B", Price = 80, Area = 70, Bedrooms = 3, Reviews = new List<Review>(), Images = new List<PropertyImage>(), PropertyAmenities = new List<PropertyAmenity>() }
            };

            _mockRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                     .ReturnsAsync((List<int> ids) => props.Where(p => ids.Contains(p.Id)).ToList());

            // Act
            var result = (await _svc.ComparePropertiesAsync(new List<int> { 1, 2 })).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(r => r.Id == 1));
            Assert.IsTrue(result.Any(r => r.Id == 2));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task Throws_When_Some_Id_Not_Found()
        {
            // Arrange
            var props = new List<Property>
            {
                new Property { Id = 1, Title = "House A", Price = 100, Area = 50, Bedrooms = 2, Reviews = new List<Review>(), Images = new List<PropertyImage>(), PropertyAmenities = new List<PropertyAmenity>() }
            };

            _mockRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                     .ReturnsAsync(props);

            // Act
            await _svc.ComparePropertiesAsync(new List<int> { 1, 99 });
        }
        [TestMethod]
        public async Task Returns_Empty_When_No_Ids()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                     .ReturnsAsync(new List<Property>());

            // Act
            var result = await _svc.ComparePropertiesAsync(new List<int>());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
        [TestMethod]
        public async Task Marks_IsMostBedrooms_When_Property_Has_Max_Bedrooms()
        {
            // Arrange
            var props = new List<Property>
    {
        new Property { Id = 1, Title = "Small", Price = 100, Area = 50, Bedrooms = 2, Reviews = new List<Review>(), Images = new List<PropertyImage>(), PropertyAmenities = new List<PropertyAmenity>() },
        new Property { Id = 2, Title = "Big", Price = 150, Area = 70, Bedrooms = 5, Reviews = new List<Review>(), Images = new List<PropertyImage>(), PropertyAmenities = new List<PropertyAmenity>() }
    };

            _mockRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                     .ReturnsAsync(props);

            // Act
            var result = (await _svc.ComparePropertiesAsync(new List<int> { 1, 2 })).ToList();

            // Assert
            Assert.IsTrue(result.Single(r => r.Id == 2).IsMostBedrooms);
            Assert.IsFalse(result.Single(r => r.Id == 1).IsMostBedrooms);
        }


    }
}
