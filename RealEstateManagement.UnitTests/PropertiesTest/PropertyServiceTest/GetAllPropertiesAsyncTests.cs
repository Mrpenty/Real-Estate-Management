using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using RealEstateManagement.Data.Entity.AddressEnity;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class GetAllPropertiesAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Properties_When_Exist()
        {
            // Arrange: seed đủ nav để tránh NRE khi map
            var property = new Property
            {
                Id = 1,
                Title = "House A",
                Description = "Nice house",
                Price = 1000,
                Bedrooms = 2,
                Bathrooms = 1,
                Floors = 1,
                ViewsCount = 10,
                Status = "Available",
                Location = "HCM",
                CreatedAt = DateTime.UtcNow,
                LandlordId = 2,
                Landlord = new ApplicationUser { Id = 2, Name = "Owner" },

                PropertyTypeId = 1,
                PropertyType = new PropertyType { Id = 1, Name = "House" },

                AddressId = 1,
                Address = new Address
                {
                    Id = 1,
                    ProvinceId = 10,
                    Province = new Province { Id = 10, Name = "HCM" },
                    WardId = 20,
                    Ward = new Ward { Id = 20, Name = "W1" },
                    StreetId = 30,
                    Street = new Street { Id = 30, Name = "Nguyen Hue" },
                    DetailedAddress = "No. 1"
                },

                Images = new List<PropertyImage> { new PropertyImage { Url = "img.jpg", IsPrimary = true } },
                PropertyAmenities = new List<PropertyAmenity>()
            };

            PropertyRepo.Setup(r => r.GetAllAsync())
                        .ReturnsAsync(new List<Property> { property });

            // ✅ Seed favorite trong DbContext (service có thể dùng DbContext để xác định IsFavorite)
            DbContext.UserFavoriteProperties.Add(new UserFavoriteProperty { UserId = 99, PropertyId = 1 });
            DbContext.SaveChanges();

            // ✅ Đồng thời setup FavoriteRepo an toàn (nếu service dùng repo thay vì DbContext)
            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(99))
                        .ReturnsAsync(new List<Property> { new Property { Id = 1 } });

            var result = await Svc.GetAllPropertiesAsync(99);

            Assert.AreEqual(1, result.Count());
            var dto = result.First();
            Assert.AreEqual("House A", dto.Title);
            Assert.IsTrue(dto.IsFavorite);
        }

        [TestMethod]
        public async Task Returns_Empty_When_No_Properties()
        {
            PropertyRepo.Setup(r => r.GetAllAsync())
                        .ReturnsAsync(new List<Property>());

            // an toàn: service có thể vẫn gọi FavoriteRepo
            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());

            var result = await Svc.GetAllPropertiesAsync(1);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public async Task Sets_Defaults_When_Missing_Data()
        {

            var property = new Property
            {
                Id = 2,
                Title = "House B",
                Description = "Test",
                Price = 2000,
                Bedrooms = 1,
                Bathrooms = 1,
                Floors = 1,
                ViewsCount = 5,
                Status = "Available",
                Location = "HN",
                CreatedAt = DateTime.UtcNow,

                // Landlord “rỗng” để map ra 0 thay vì NRE
                LandlordId = 0,
                Landlord = new ApplicationUser { Id = 0, Name = null },

                PropertyTypeId = 1,
                PropertyType = new PropertyType { Id = 1, Name = "House" },

                AddressId = 2,
                Address = new Address
                {
                    Id = 2,
                    ProvinceId = 1,
                    Province = new Province { Id = 1, Name = null },
                    WardId = 1,
                    Ward = new Ward { Id = 1, Name = null },
                    StreetId = 1,
                    Street = new Street { Id = 1, Name = null },
                    DetailedAddress = null
                },

                Images = new List<PropertyImage>(),           // rỗng => PrimaryImageUrl = null
                PropertyAmenities = new List<PropertyAmenity>()// rỗng
            };

            PropertyRepo.Setup(r => r.GetAllAsync())
                        .ReturnsAsync(new List<Property> { property });

            FavoriteRepo.Setup(r => r.AllFavoritePropertyAsync(It.IsAny<int>()))
                        .ReturnsAsync(new List<Property>());

            var dto = (await Svc.GetAllPropertiesAsync(1)).First();

            Assert.AreEqual(2, dto.Id);
            Assert.AreEqual(0, dto.LandlordId);          // default khi landlord.Id = 0
            Assert.IsNull(dto.PrimaryImageUrl);          // không có ảnh primary

        }
    }
}
