using Microsoft.VisualStudio.TestTools.UnitTesting;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Business.DTO.Properties;
using Moq;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class GetAllPropertiesAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Properties_When_Exist()
        {
            var property = new Property
            {
                Id = 1,
                Title = "House A",
                Description = "Nice house",
                Type = "Apartment",
                Price = 1000,
                AddressId = 1,
                Address = new Data.Entity.AddressEnity.Address { Id = 1, ProvinceId = 10, Province = new Data.Entity.AddressEnity.Province { Id = 10, Name = "HCM" } },
                Landlord = new ApplicationUser { Id = 2, Name = "Owner" },
                CreatedAt = DateTime.UtcNow,
                ViewsCount = 10,
                Images = new List<PropertyImage> { new PropertyImage { Url = "img.jpg", IsPrimary = true } }
            };

            PropertyRepo.Setup(r => r.GetAllAsync())
                        .ReturnsAsync(new List<Property> { property });

            // Thêm favorite trong DbContext
            DbContext.UserFavoriteProperties.Add(new UserFavoriteProperty { UserId = 99, PropertyId = 1 });
            DbContext.SaveChanges();

            var result = await Svc.GetAllPropertiesAsync(99);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("House A", result.First().Title);
            Assert.IsTrue(result.First().IsFavorite);
        }

        [TestMethod]
        public async Task Returns_Empty_When_No_Properties()
        {
            PropertyRepo.Setup(r => r.GetAllAsync())
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
                Type = "Villa",
                Price = 2000,
                AddressId = 2,
                Address = null, // intentionally missing
                Landlord = null,
                CreatedAt = DateTime.UtcNow,
                ViewsCount = 5,
                Images = null
            };

            PropertyRepo.Setup(r => r.GetAllAsync())
                        .ReturnsAsync(new List<Property> { property });

            var result = await Svc.GetAllPropertiesAsync(1);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(0, result.First().LandlordId);
            Assert.IsNull(result.First().PrimaryImageUrl);
        }
    }
}
