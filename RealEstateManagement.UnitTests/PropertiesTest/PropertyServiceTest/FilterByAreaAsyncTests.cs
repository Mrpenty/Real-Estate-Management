using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.Reviews;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class FilterByAreaAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Properties_Within_Range()
        {
            // Arrange: seed đủ nav (PropertyType, Address) để tránh NRE trong mapping
            var props = new List<Property>
            {
                new Property
                {
                    Id = 1, Title = "Small House", Area = 50, Price = 100000,
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
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>(),
                    Reviews = new List<Review>()
                },
                new Property
                {
                    Id = 2, Title = "Big House", Area = 150, Price = 200000,
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
                    Images = new List<PropertyImage>(),
                    PropertyAmenities = new List<PropertyAmenity>(),
                    Reviews = new List<Review>()
                }
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
        public async Task Maps_Important_Fields_Correctly()
        {
            // Arrange: seed đủ dữ liệu cần cho mapping (images primary, landlord, amenities, promotion package)
            var props = new List<Property>
            {
                new Property
                {
                    Id = 10,
                    Title = "Luxury Villa",
                    Description = "Sea view villa",
                    Area = 300,
                    Price = 1_000_000,
                    Bedrooms = 5,
                    Bathrooms = 4,
                    Floors = 2,
                    ViewsCount = 50,
                    Status = "Available",
                    Location = "DN",
                    CreatedAt = DateTime.UtcNow,
                    LandlordId = 99,
                    PropertyTypeId = 2,
                    PropertyType = new PropertyType { Id = 2, Name = "Villa" },
                    AddressId = 9,
                    Address = new Address
                    {
                        Id = 9,
                        Province = new Province{ Name = "DN" },
                        Ward = new Ward{ Name = "W2" },
                        Street = new Street{ Name = "Vo Nguyen Giap" },
                        DetailedAddress = "S9"
                    },
                    Images = new List<PropertyImage>
                    {
                        new PropertyImage { Url = "villa.jpg", IsPrimary = true }
                    },
                    Landlord = new RealEstateManagement.Data.Entity.User.ApplicationUser
                    {
                        Id = 99,
                        Name = "Mr C",
                        PhoneNumber = "987654",
                        ProfilePictureUrl = "landlord.jpg"
                    },
                    PropertyAmenities = new List<PropertyAmenity>
                    {
                        new PropertyAmenity { Amenity = new Amenity { Name = "Gym" } }
                    },
                    PropertyPromotions = new List<PropertyPromotion>
                    {
                        new PropertyPromotion
                        {
                            PromotionPackage = new RealEstateManagement.Data.Entity.Payment.PromotionPackage
                            {
                                Name = "VIP",
                                Level = 3
                            }
                        }
                    },
                    Reviews = new List<Review>()
                }
            };

            PropertyRepo.Setup(r => r.FilterByAreaAsync(200, 400))
                        .ReturnsAsync(props);

            // Act
            var dto = (await Svc.FilterByAreaAsync(200, 400)).First();

            // Assert
            Assert.AreEqual(10, dto.Id);
            Assert.AreEqual("Luxury Villa", dto.Title);
            Assert.AreEqual("VIP", dto.PromotionPackageName);
            Assert.AreEqual("Gym", dto.Amenities.First());
            Assert.AreEqual("villa.jpg", dto.PrimaryImageUrl);
            Assert.AreEqual("Mr C", dto.LandlordName);
        }

        [TestMethod]
        public async Task Returns_Only_Within_Bounds()
        {
            // Arrange
            var props = new List<Property>
            {
                new Property
                {
                    Id = 1, Title = "Tiny", Area = 20,
                    Bedrooms = 1, Bathrooms = 1, Floors = 1, ViewsCount = 0,
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
                    }
                },
                new Property
                {
                    Id = 2, Title = "Medium", Area = 100,
                    Bedrooms = 2, Bathrooms = 1, Floors = 1, ViewsCount = 0,
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
                    }
                },
                new Property
                {
                    Id = 3, Title = "Huge", Area = 500,
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
                    }
                }
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
