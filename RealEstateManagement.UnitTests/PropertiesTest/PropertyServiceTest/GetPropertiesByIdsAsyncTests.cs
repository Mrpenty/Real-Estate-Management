using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.AddressEnity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class GetPropertiesByIdsAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Properties_When_Ids_Exist()
        {
            // Arrange: seed đủ nav/field để tránh NRE trong mapping
            var props = new List<Property>
            {
                new Property
                {
                    Id = 1, Title = "House A", Area = 100, Price = 100000,
                    Bedrooms = 2, Bathrooms = 1, Floors = 1, ViewsCount = 0,
                    Status = "Active", Location = "LocA", CreatedAt = DateTime.UtcNow,
                    LandlordId = 10,
                    PropertyTypeId = 1, PropertyType = new PropertyType { Id = 1, Name = "House" },
                    AddressId = 101, Address = new Address
                    {
                        Id = 101,
                        Province = new Province { Name = "P1" },
                        Ward = new Ward { Name = "W1" },
                        Street = new Street { Name = "S1" },
                        DetailedAddress = "D1"
                    },
                    Images = new List<PropertyImage> { new PropertyImage { Url = "a.jpg", IsPrimary = true } },
                    PropertyAmenities = new List<PropertyAmenity>(),
                    PropertyPromotions = new List<PropertyPromotion>()
                },
                new Property
                {
                    Id = 2, Title = "House B", Area = 200, Price = 200000,
                    Bedrooms = 3, Bathrooms = 2, Floors = 1, ViewsCount = 0,
                    Status = "Active", Location = "LocB", CreatedAt = DateTime.UtcNow,
                    LandlordId = 11,
                    PropertyTypeId = 1, PropertyType = new PropertyType { Id = 1, Name = "House" },
                    AddressId = 102, Address = new Address
                    {
                        Id = 102,
                        Province = new Province { Name = "P2" },
                        Ward = new Ward { Name = "W2" },
                        Street = new Street { Name = "S2" },
                        DetailedAddress = "D2"
                    },
                    Images = new List<PropertyImage> { new PropertyImage { Url = "b.jpg", IsPrimary = true } },
                    PropertyAmenities = new List<PropertyAmenity>(),
                    PropertyPromotions = new List<PropertyPromotion>()
                }
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
            // Arrange: seed 3 căn, mapping-safe
            var props = new List<Property>
            {
                new Property
                {
                    Id = 1, Title = "House A",
                    Bedrooms = 2, Bathrooms = 1, Floors = 1, ViewsCount = 0,
                    Status = "Active", Location = "A", CreatedAt = DateTime.UtcNow,
                    LandlordId = 1,
                    PropertyTypeId = 1, PropertyType = new PropertyType{ Id = 1, Name = "House" },
                    AddressId = 1, Address = new Address
                    {
                        Id = 1,
                        Province = new Province{ Name="P" },
                        Ward = new Ward{ Name="W" },
                        Street = new Street{ Name="S" },
                        DetailedAddress = "D"
                    },
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>(),
                    PropertyPromotions = new List<PropertyPromotion>()
                },
                new Property
                {
                    Id = 2, Title = "House B",
                    Bedrooms = 3, Bathrooms = 2, Floors = 1, ViewsCount = 0,
                    Status = "Active", Location = "B", CreatedAt = DateTime.UtcNow,
                    LandlordId = 2,
                    PropertyTypeId = 1, PropertyType = new PropertyType{ Id = 1, Name = "House" },
                    AddressId = 2, Address = new Address
                    {
                        Id = 2,
                        Province = new Province{ Name="P" },
                        Ward = new Ward{ Name="W" },
                        Street = new Street{ Name="S" },
                        DetailedAddress = "D"
                    },
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>(),
                    PropertyPromotions = new List<PropertyPromotion>()
                },
                new Property
                {
                    Id = 3, Title = "House C",
                    Bedrooms = 4, Bathrooms = 2, Floors = 2, ViewsCount = 0,
                    Status = "Active", Location = "C", CreatedAt = DateTime.UtcNow,
                    LandlordId = 3,
                    PropertyTypeId = 1, PropertyType = new PropertyType{ Id = 1, Name = "House" },
                    AddressId = 3, Address = new Address
                    {
                        Id = 3,
                        Province = new Province{ Name="P" },
                        Ward = new Ward{ Name="W" },
                        Street = new Street{ Name="S" },
                        DetailedAddress = "D"
                    },
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>(),
                    PropertyPromotions = new List<PropertyPromotion>()
                }
            };

            PropertyRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                        .ReturnsAsync((List<int> ids) => props.Where(p => ids.Contains(p.Id)).ToList());

            // Act
            var result = (await Svc.GetPropertiesByIdsAsync(new List<int> { 1, 3 })).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(r => r.Id == 1));
            Assert.IsTrue(result.Any(r => r.Id == 3));
        }
    }
}
