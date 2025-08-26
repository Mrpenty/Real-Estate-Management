//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
////using RealEstateManagement.Business.Services.PropertyService;
//using RealEstateManagement.Business.DTO.Properties;
//using RealEstateManagement.Data.Entity.PropertyEntity;
//using RealEstateManagement.Data.Entity.AddressEnity;
//using RealEstateManagement.Data.Entity.User;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System;
//using RealEstateManagement.Business.Repositories.Properties;
//using RealEstateManagement.Business.Services.Properties;
//using RealEstateManagement.Data.Entity;

//namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
//{
//    [TestClass]
//    public class FilterByTypeAsyncTests
//    {
//        private Mock<IPropertyRepository> _mockRepo;
//        private PropertyService _svc;

//        [TestInitialize]
//        public void Setup()
//        {
//            _mockRepo = new Mock<IPropertyRepository>(MockBehavior.Strict);
//            _svc = new PropertyService(_mockRepo.Object, null, null);
//            // tuỳ ctor của bạn, mock thêm các dependency nếu có
//        }

//        [TestMethod]
//        public async Task Returns_Properties_When_Found()
//        {
//            // Arrange
//            var props = new List<Property>
//            {
//                new Property
//                {
//                    Id = 1,
//                    Title = "Nice House",
//                    Description = "Desc",
//                    Type = "Apartment",
//                    AddressId = 11,
//                    Area = 70,
//                    Bedrooms = 2,
//                    Price = 500,
//                    Status = "Available",
//                    Location = "HN",
//                    CreatedAt = DateTime.UtcNow,
//                    ViewsCount = 100,
//                    Address = new Address
//                    {
//                        ProvinceId = 1, Province = new Province { Id = 1, Name = "HN" },
//                        WardId = 2, Ward = new Ward { Id = 2, Name = "Ward1" },
//                        StreetId = 3, Street = new Street { Id = 3, Name = "Street A" }
//                    },
//                    Images = new List<PropertyImage> { new PropertyImage { Url = "img.jpg", IsPrimary = true } },
//                    Landlord = new ApplicationUser { Name = "Mr A", PhoneNumber = "123", ProfilePictureUrl = "avatar.png" },
//                    PropertyAmenities = new List<PropertyAmenity> { new PropertyAmenity { Amenity = new Amenity { Name = "Pool" } } },
//                    PropertyPromotions = new List<PropertyPromotion>
//                    {
//                        new PropertyPromotion { PromotionPackage = new Data.Entity.Payment.PromotionPackage { Name = "Gold", Level = 2 } }
//                    }
//                }
//            };

//            _mockRepo.Setup(r => r.FilterByTypeAsync("Apartment")).ReturnsAsync(props);

//            // Act
//            var result = (await _svc.FilterByTypeAsync("Apartment")).ToList();

//            // Assert
//            Assert.AreEqual(1, result.Count);
//            var dto = result[0];
//            Assert.AreEqual("Nice House", dto.Title);
//            Assert.AreEqual("Gold", dto.PromotionPackageName);
//            Assert.AreEqual("Pool", dto.Amenities.First());
//            Assert.AreEqual("Mr A", dto.LandlordName);
//            Assert.AreEqual("img.jpg", dto.PrimaryImageUrl);
//        }

//        [TestMethod]
//        public async Task Returns_Empty_When_No_Properties()
//        {
//            // Arrange
//            _mockRepo.Setup(r => r.FilterByTypeAsync("Villa")).ReturnsAsync(new List<Property>());

//            // Act
//            var result = await _svc.FilterByTypeAsync("Villa");

//            // Assert
//            Assert.IsNotNull(result);
//            Assert.AreEqual(0, result.Count());
//        }

//        [TestMethod]
//        public async Task Handles_Null_Optional_Fields()
//        {
//            // Arrange
//            var props = new List<Property>
//            {
//                new Property
//                {
//                    Id = 2,
//                    Title = "No Landlord",
//                    Type = "Apartment",
//                    AddressId = 11,
//                    Area = 50,
//                    Bedrooms = 1,
//                    Price = 100,
//                    Status = "Pending",
//                    Location = "HCM",
//                    CreatedAt = DateTime.UtcNow,
//                    ViewsCount = 10,
//                    Address = new Address
//                    {
//                        ProvinceId = 1, Province = new Province { Id = 1, Name = "HCM" },
//                        WardId = 2, Ward = new Ward { Id = 2, Name = "Ward2" },
//                        StreetId = 3, Street = new Street { Id = 3, Name = "Street B" }
//                    },
//                    Images = null,
//                    Landlord = null,
//                    PropertyAmenities = null,
//                    PropertyPromotions = null
//                }
//            };

//            _mockRepo.Setup(r => r.FilterByTypeAsync("Apartment")).ReturnsAsync(props);

//            // Act
//            var result = (await _svc.FilterByTypeAsync("Apartment")).First();

//            // Assert
//            Assert.IsNull(result.LandlordName);
//            Assert.IsNull(result.PrimaryImageUrl);
//            Assert.AreEqual(0, result.Amenities.Count);
//            Assert.IsNull(result.PromotionPackageName);
//        }
//    }
//}
