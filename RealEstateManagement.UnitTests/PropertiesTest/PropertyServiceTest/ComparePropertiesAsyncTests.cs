using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealEstateManagement.Business.Repositories.Properties;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.Reviews;
using RealEstateManagement.Data.Entity.AddressEnity;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class ComparePropertiesAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Comparison_Result_When_Ids_Exist()
        {
            // Arrange: seed đủ navs mà service dùng (PropertyType, Address)
            var props = new List<Property>
            {
                new Property {
                    Id = 1, Title = "House A", Price = 100, Area = 50, Bedrooms = 2,
                    Bathrooms = 1, Floors = 1, LandlordId = 10, Status = "Active",
                    ViewsCount = 0, Location = "LocA", CreatedAt = System.DateTime.UtcNow,
                    PropertyTypeId = 1, PropertyType = new PropertyType { Id = 1, Name = "House" },
                    AddressId = 1, Address = new Address {
                        Id = 1,
                        Province = new Province{ Name="P1"},
                        Ward = new Ward{ Name="W1"},
                        Street = new Street{ Name="S1"},
                        DetailedAddress = "D1"
                    },
                    Reviews = new List<Review>(),
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>()
                },
                new Property {
                    Id = 2, Title = "House B", Price = 80, Area = 70, Bedrooms = 3,
                    Bathrooms = 2, Floors = 1, LandlordId = 11, Status = "Active",
                    ViewsCount = 0, Location = "LocB", CreatedAt = System.DateTime.UtcNow,
                    PropertyTypeId = 1, PropertyType = new PropertyType { Id = 1, Name = "House" },
                    AddressId = 2, Address = new Address {
                        Id = 2,
                        Province = new Province{ Name="P2"},
                        Ward = new Ward{ Name="W2"},
                        Street = new Street{ Name="S2"},
                        DetailedAddress = "D2"
                    },
                    Reviews = new List<Review>(),
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>()
                }
            };

            PropertyRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                        .ReturnsAsync((List<int> ids) => props.Where(p => ids.Contains(p.Id)).ToList());

            // Act
            var result = (await Svc.ComparePropertiesAsync(new List<int> { 1, 2 })).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(r => r.Id == 1));
            Assert.IsTrue(result.Any(r => r.Id == 2));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task Throws_When_Some_Id_Not_Found()
        {
            var props = new List<Property>
            {
                new Property {
                    Id = 1, Title = "House A", Price = 100, Area = 50, Bedrooms = 2,
                    Bathrooms = 1, Floors = 1, LandlordId = 10, Status = "Active",
                    ViewsCount = 0, Location = "LocA", CreatedAt = System.DateTime.UtcNow,
                    PropertyTypeId = 1, PropertyType = new PropertyType { Id = 1, Name = "House" },
                    AddressId = 1, Address = new Address {
                        Id = 1,
                        Province = new Province{ Name="P1"},
                        Ward = new Ward{ Name="W1"},
                        Street = new Street{ Name="S1"},
                        DetailedAddress = "D1"
                    },
                    Reviews = new List<Review>(),
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>()
                }
            };

            PropertyRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                        .ReturnsAsync(props);

            // Act -> thiếu id 99 nên service nên ném KeyNotFoundException
            await Svc.ComparePropertiesAsync(new List<int> { 1, 99 });
        }

        [TestMethod]
        public async Task Returns_Empty_When_No_Ids()
        {
            // (Service thường return sớm mà không gọi repo, nhưng cứ setup an toàn)
            PropertyRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                        .ReturnsAsync(new List<Property>());

            var result = await Svc.ComparePropertiesAsync(new List<int>());

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Marks_IsMostBedrooms_When_Property_Has_Max_Bedrooms()
        {
            var props = new List<Property>
            {
                new Property {
                    Id = 1, Title = "Small", Price = 100, Area = 50, Bedrooms = 2,
                    Bathrooms = 1, Floors = 1, LandlordId = 10, Status = "Active",
                    ViewsCount = 0, Location = "LocA", CreatedAt = System.DateTime.UtcNow,
                    PropertyTypeId = 1, PropertyType = new PropertyType { Id = 1, Name = "House" },
                    AddressId = 1, Address = new Address {
                        Id = 1,
                        Province = new Province{ Name="P1"},
                        Ward = new Ward{ Name="W1"},
                        Street = new Street{ Name="S1"},
                        DetailedAddress = "D1"
                    },
                    Reviews = new List<Review>(),
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>()
                },
                new Property {
                    Id = 2, Title = "Big", Price = 150, Area = 70, Bedrooms = 5,
                    Bathrooms = 2, Floors = 2, LandlordId = 11, Status = "Active",
                    ViewsCount = 0, Location = "LocB", CreatedAt = System.DateTime.UtcNow,
                    PropertyTypeId = 1, PropertyType = new PropertyType { Id = 1, Name = "House" },
                    AddressId = 2, Address = new Address {
                        Id = 2,
                        Province = new Province{ Name="P2"},
                        Ward = new Ward{ Name="W2"},
                        Street = new Street{ Name="S2"},
                        DetailedAddress = "D2"
                    },
                    Reviews = new List<Review>(),
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>()
                }
            };

            PropertyRepo.Setup(r => r.GetPropertiesByIdsAsync(It.IsAny<List<int>>()))
                        .ReturnsAsync(props);

            var result = (await Svc.ComparePropertiesAsync(new List<int> { 1, 2 })).ToList();

            Assert.IsTrue(result.Single(r => r.Id == 2).IsMostBedrooms);
            Assert.IsFalse(result.Single(r => r.Id == 1).IsMostBedrooms);
        }
    }
}
