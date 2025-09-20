using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.AddressEnity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class FilterByPriceAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Properties_Within_Range()
        {
            var props = new List<Property>
            {
                new Property {
                    Id = 1, Title = "House A", Price = 200000, Area = 50, Bedrooms = 2,
                    Bathrooms = 1, Floors = 1, ViewsCount = 0, Status = "Available",
                    Location = "LocA", CreatedAt = DateTime.UtcNow, LandlordId = 10,
                    PropertyTypeId = 1, PropertyType = new PropertyType { Id = 1, Name = "House" },
                    AddressId = 1, Address = new Address {
                        Id = 1,
                        Province = new Province { Name = "P1" },
                        Ward = new Ward { Name = "W1" },
                        Street = new Street { Name = "S1" },
                        DetailedAddress = "D1"
                    },
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>()
                },
                new Property {
                    Id = 2, Title = "House B", Price = 800000, Area = 70, Bedrooms = 3,
                    Bathrooms = 2, Floors = 1, ViewsCount = 0, Status = "Available",
                    Location = "LocB", CreatedAt = DateTime.UtcNow, LandlordId = 11,
                    PropertyTypeId = 1, PropertyType = new PropertyType { Id = 1, Name = "House" },
                    AddressId = 2, Address = new Address {
                        Id = 2,
                        Province = new Province { Name = "P2" },
                        Ward = new Ward { Name = "W2" },
                        Street = new Street { Name = "S2" },
                        DetailedAddress = "D2"
                    },
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>()
                }
            };

            PropertyRepo.Setup(r => r.FilterByPriceAsync(100000, 900000))
                        .ReturnsAsync(props);

            var result = (await Svc.FilterByPriceAsync(100000, 900000)).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("House A", result[0].Title);
            Assert.AreEqual("House B", result[1].Title);
        }

        [TestMethod]
        public async Task Returns_Empty_When_No_Properties_Exist()
        {
            PropertyRepo.Setup(r => r.FilterByPriceAsync(500000, 1000000))
                        .ReturnsAsync(new List<Property>());

            var result = await Svc.FilterByPriceAsync(500000, 1000000);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Maps_Important_Fields_Correctly()
        {
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
                    Bathrooms = 2,
                    Floors = 2,
                    ViewsCount = 99,
                    Status = "Available",
                    Location = "HN",
                    CreatedAt = DateTime.UtcNow,
                    LandlordId = 77,
                    PropertyTypeId = 1,
                    PropertyType = new PropertyType { Id = 1, Name = "House" },
                    AddressId = 3,
                    Address = new Address
                    {
                        Id = 3,
                        Province = new Province { Name = "HN" },
                        Ward = new Ward { Name = "W1" },
                        Street = new Street { Name = "S1" },
                        DetailedAddress = "D1"
                    },
                    Images = new List<PropertyImage>
                    {
                        new PropertyImage { Url = "main.jpg", IsPrimary = true }
                    },
                    Landlord = new RealEstateManagement.Data.Entity.User.ApplicationUser
                    {
                        Id = 77, Name = "Mr B", PhoneNumber = "123456", ProfilePictureUrl = "avatar.jpg"
                    },
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        new PropertyAmenity { Amenity = new Amenity { Name = "Pool" } }
                    },
                    PropertyPromotions = new List<PropertyPromotion>
                    {
                        new PropertyPromotion
                        {
                            PromotionPackage = new RealEstateManagement.Data.Entity.Payment.PromotionPackage
                            { Name = "Premium", Level = 2 }
                        }
                    }
                }
            };

            PropertyRepo.Setup(r => r.FilterByPriceAsync(400000, 600000))
                        .ReturnsAsync(props);

            var dto = (await Svc.FilterByPriceAsync(400000, 600000)).First();

            Assert.AreEqual(2, dto.Id);
            Assert.AreEqual("Luxury House", dto.Title);
            Assert.AreEqual("Premium", dto.PromotionPackageName);
            Assert.AreEqual("Pool", dto.Amenities.First());
            Assert.AreEqual("main.jpg", dto.PrimaryImageUrl);
            Assert.AreEqual("Mr B", dto.LandlordName);
        }

        [TestMethod]
        public async Task Returns_Only_Within_Bounds()
        {
            var props = new List<Property>
            {
                new Property {
                    Id = 1, Title = "Cheap", Price = 100000,
                    Bedrooms = 1, Bathrooms = 1, Floors = 1, ViewsCount = 0,
                    Status = "Active", Location = "A", CreatedAt = DateTime.UtcNow, LandlordId = 1,
                    PropertyTypeId = 1, PropertyType = new PropertyType{ Id = 1, Name = "House" },
                    AddressId = 1, Address = new Address {
                        Id = 1,
                        Province = new Province{ Name="P" },
                        Ward = new Ward{ Name="W" },
                        Street = new Street{ Name="S" },
                        DetailedAddress = "D"
                    }
                },
                new Property {
                    Id = 2, Title = "Mid", Price = 500000,
                    Bedrooms = 2, Bathrooms = 1, Floors = 1, ViewsCount = 0,
                    Status = "Active", Location = "B", CreatedAt = DateTime.UtcNow, LandlordId = 2,
                    PropertyTypeId = 1, PropertyType = new PropertyType{ Id = 1, Name = "House" },
                    AddressId = 2, Address = new Address {
                        Id = 2,
                        Province = new Province{ Name="P" },
                        Ward = new Ward{ Name="W" },
                        Street = new Street{ Name="S" },
                        DetailedAddress = "D"
                    }
                },
                new Property {
                    Id = 3, Title = "Expensive", Price = 1000000,
                    Bedrooms = 3, Bathrooms = 2, Floors = 2, ViewsCount = 0,
                    Status = "Active", Location = "C", CreatedAt = DateTime.UtcNow, LandlordId = 3,
                    PropertyTypeId = 1, PropertyType = new PropertyType{ Id = 1, Name = "House" },
                    AddressId = 3, Address = new Address {
                        Id = 3,
                        Province = new Province{ Name="P" },
                        Ward = new Ward{ Name="W" },
                        Street = new Street{ Name="S" },
                        DetailedAddress = "D"
                    }
                }
            };

            PropertyRepo.Setup(r => r.FilterByPriceAsync(200000, 800000))
                        .ReturnsAsync(props.Where(p => p.Price >= 200000 && p.Price <= 800000).ToList());

            var result = (await Svc.FilterByPriceAsync(200000, 800000)).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Mid", result[0].Title);
        }
    }
}
