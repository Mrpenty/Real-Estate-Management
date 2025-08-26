using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.Services.Properties;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.PropertiesTest.PropertyServiceTest
{
    [TestClass]
    public class GetPaginatedPropertiesAsyncTests : PropertyTestBase
    {
        [TestMethod]
        public async Task Returns_Empty_When_No_Properties()
        {
            // Arrange
            PropertyRepo.Setup(r => r.GetAllAsync())
                        .ReturnsAsync(new List<Property>());

            // Act
            var result = await Svc.GetPaginatedPropertiesAsync(page: 1, pageSize: 10, userId: null);

            // Assert
            Assert.AreEqual(0, result.TotalCount);
            Assert.AreEqual(0, result.Data.Count());
            Assert.AreEqual(0, result.TotalPages);
            Assert.IsFalse(result.HasNextPage);
            Assert.IsFalse(result.HasPreviousPage);

            PropertyRepo.Verify(r => r.GetAllAsync(), Times.Once);
            PropertyRepo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Returns_Properties_When_Exist()
        {
            // Arrange
            var prop = new Property
            {
                Id = 1,
                Title = "Test Property",
                Description = "Desc",
                Type = "Apartment",
                AddressId = 1,
                Area = 50,
                Bedrooms = 2,
                Price = 1000,
                Status = "active",
                Location = "Loc",
                CreatedAt = DateTime.UtcNow,
                ViewsCount = 10,
                Images = new List<PropertyImage> { new PropertyImage { Url = "img.png", IsPrimary = true } },
                PropertyAmenities = new List<PropertyAmenity>()
            };

            PropertyRepo.Setup(r => r.GetAllAsync())
                        .ReturnsAsync(new List<Property> { prop });

            // Có userId để IsFavorite hoạt động
            DbContext.UserFavoriteProperties.Add(new UserFavoriteProperty
            {
                UserId = 99,
                PropertyId = 1,
                CreatedAt = DateTime.UtcNow
            });
            DbContext.SaveChanges();

            // Act
            var result = await Svc.GetPaginatedPropertiesAsync(page: 1, pageSize: 10, userId: 99);

            // Assert
            Assert.AreEqual(1, result.TotalCount);
            Assert.AreEqual(1, result.Data.Count());
            var item = result.Data.First();
            Assert.AreEqual("Test Property", item.Title);
            Assert.IsTrue(item.IsFavorite);

            PropertyRepo.Verify(r => r.GetAllAsync(), Times.Once);
            PropertyRepo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Applies_Pagination_Correctly()
        {
            // Arrange: 15 properties với CreatedAt khác nhau để sort Desc ổn định
            var baseTime = DateTime.UtcNow;
            var list = new List<Property>();
            for (int i = 1; i <= 15; i++)
            {
                list.Add(new Property
                {
                    Id = i,
                    Title = $"P{i}",
                    Type = "Apartment",
                    AddressId = i,
                    Area = 40 + i,
                    Bedrooms = 1 + i % 3,
                    Price = 900 + i,
                    Status = "active",
                    Location = "L",
                    CreatedAt = baseTime.AddMinutes(i), // i lớn hơn => mới hơn
                    ViewsCount = i
                });
            }
            // Sort desc theo CreatedAt: P15 mới nhất... P1 cũ nhất
            PropertyRepo.Setup(r => r.GetAllAsync())
                        .ReturnsAsync(list);

            // Act: page 2 size 5 => kỳ vọng P10..P6
            var result = await Svc.GetPaginatedPropertiesAsync(page: 2, pageSize: 5, userId: null);

            // Assert
            Assert.AreEqual(15, result.TotalCount);
            Assert.AreEqual(3, result.TotalPages);
            Assert.AreEqual(5, result.Data.Count());

            var titles = result.Data.Select(x => x.Title).ToList();
            CollectionAssert.AreEqual(new List<string> { "P10", "P9", "P8", "P7", "P6" }, titles);

            PropertyRepo.Verify(r => r.GetAllAsync(), Times.Once);
            PropertyRepo.VerifyNoOtherCalls();
        }
    }
}
