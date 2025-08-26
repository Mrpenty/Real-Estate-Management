using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class GetPropertiesByIdsAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Properties_When_Ids_Exist()
        {
            // Arrange
            var props = new List<Property>
            {
                new Property { Id = 1, Title = "House A",Images = new List<PropertyImage>(), Area = 100, Price = 100000 },
                new Property { Id = 2, Title = "House B",Images = new List<PropertyImage>(), Area = 200, Price = 200000 }
            };

            PropertyRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                        .ReturnsAsync(props);

            // Act
            var result = (await Svc.GetPropertiesByIdsAsync(new List<int> { 1, 2 })).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("House A", result[0].Title);
            Assert.AreEqual("House B", result[1].Title);
        }

        [TestMethod]
        public async Task Returns_Empty_When_No_Ids_Match()
        {
            // Arrange
            PropertyRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                        .ReturnsAsync(new List<Property>());

            // Act
            var result = await Svc.GetPropertiesByIdsAsync(new List<int> { 99, 100 });

            // Assert
            Assert.AreEqual(0, result.Count());
        }


        [TestMethod]
        public async Task Returns_Only_Requested_Ids()
        {
            // Arrange
            var props = new List<Property>
    {
        new Property { Id = 1, Title = "House A", Images = new List<PropertyImage>(), PropertyAmenities = new List<PropertyAmenity>(), PropertyPromotions = new List<PropertyPromotion>() },
        new Property { Id = 2, Title = "House B", Images = new List<PropertyImage>(), PropertyAmenities = new List<PropertyAmenity>(), PropertyPromotions = new List<PropertyPromotion>() },
        new Property { Id = 3, Title = "House C", Images = new List<PropertyImage>(), PropertyAmenities = new List<PropertyAmenity>(), PropertyPromotions = new List<PropertyPromotion>() }
    };

            PropertyRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                        .ReturnsAsync((List<int> ids) =>
                            props.Where(p => ids.Contains(p.Id)).ToList());

            // Act
            var result = (await Svc.GetPropertiesByIdsAsync(new List<int> { 1, 3 })).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(r => r.Id == 1));
            Assert.IsTrue(result.Any(r => r.Id == 3));
        }

    }
}
