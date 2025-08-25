using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RealEstateManagement.Data.Entity.PropertyEntity.PropertyPost;

namespace RealEstateManagement.UnitTests.OwnerTest.OwnerPropertyTest
{
    [TestClass]
    public class UpdatePropertyAsyncTests : OwnerPropertyTestBase
    {
        [TestMethod]
        public async Task Throws_When_NotFound()
        {
            Repo.Setup(r => r.GetByIdAsync(100, 9)).ReturnsAsync((Property)null!);

            await Assert.ThrowsExceptionAsync<System.Exception>(() =>
                Svc.UpdatePropertyAsync(new PropertyCreateRequestDto(), 9, 100));

            Repo.Verify(r => r.GetByIdAsync(100, 9), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Throws_When_Status_Sold_Or_Rented()
        {
            var prop = new Property
            {
                Id = 1,
                Posts = new List<PropertyPost> { new PropertyPost { Status = PropertyPostStatus.Rented } }
            };

            Repo.Setup(r => r.GetByIdAsync(1, 9)).ReturnsAsync(prop);

            await Assert.ThrowsExceptionAsync<System.InvalidOperationException>(() =>
                Svc.UpdatePropertyAsync(new PropertyCreateRequestDto(), 9, 1));

            Repo.Verify(r => r.GetByIdAsync(1, 9), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Updates_Fields_Sets_Pending_And_Calls_Address_Amenities_And_Update()
        {
            var prop = new Property
            {
                Id = 2,
                Title = "Old",
                Description = "OldD",
                Price = 1,
                Area = 10,
                Bedrooms = 1,
                Type = "Apartment",
                Location = "OldLoc",
                Posts = new List<PropertyPost> { new PropertyPost { Status = PropertyPostStatus.Pending } }
            };

            Repo.Setup(r => r.GetByIdAsync(2, 9)).ReturnsAsync(prop);
            Repo.Setup(r => r.UpdateAddressAsync(2, 1, 2, 3, "X")).Returns(Task.CompletedTask);
            Repo.Setup(r => r.UpdateAmenitiesAsync(2, It.IsAny<List<int>>())).Returns(Task.CompletedTask);
            Repo.Setup(r => r.UpdateAsync(prop)).Returns(Task.CompletedTask);

            var dto = new PropertyCreateRequestDto
            {
                Title = "New",
                Description = "NewD",
                Price = 999,
                Area = 77,
                Bedrooms = 4,
                Type = "House",
                Location = "NewLoc",
                ProvinceId = 1,
                WardId = 2,
                StreetId = 3,
                DetailedAddress = "X",
                AmenityIds = new List<int> { 5, 6 }
            };

            await Svc.UpdatePropertyAsync(dto, 9, 2);

            Assert.AreEqual("New", prop.Title);
            Assert.AreEqual("NewD", prop.Description);
            Assert.AreEqual(999, prop.Price);
            Assert.AreEqual(77, prop.Area);
            Assert.AreEqual(4, prop.Bedrooms);
            Assert.AreEqual("House", prop.Type);
            Assert.AreEqual("NewLoc", prop.Location);
            Assert.AreEqual(PropertyPostStatus.Pending, prop.Posts.First().Status);

            // ✅ Bổ sung verify cho GetByIdAsync
            Repo.Verify(r => r.GetByIdAsync(2, 9), Times.Once);

            Repo.Verify(r => r.UpdateAddressAsync(2, 1, 2, 3, "X"), Times.Once);
            Repo.Verify(r => r.UpdateAmenitiesAsync(2, It.Is<List<int>>(ids => ids.SequenceEqual(new[] { 5, 6 }))), Times.Once);
            Repo.Verify(r => r.UpdateAsync(prop), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Leaves_Draft_Status_And_Skips_Amenities_When_Not_Provided()
        {
            var prop = new Property
            {
                Id = 3,
                Posts = new List<PropertyPost> { new PropertyPost { Status = PropertyPostStatus.Draft } }
            };

            Repo.Setup(r => r.GetByIdAsync(3, 9)).ReturnsAsync(prop);

            // Cho phép gọi UpdateAddressAsync nếu service thực thi với 0/0/0/null
            Repo.Setup(r => r.UpdateAddressAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()
            )).Returns(Task.CompletedTask);

            Repo.Setup(r => r.UpdateAsync(prop)).Returns(Task.CompletedTask);

            var dto = new PropertyCreateRequestDto
            {
                Title = "T",
                Description = "D",
                Price = 10,
                Area = 20,
                Bedrooms = 2,
                Type = "A",
                Location = "Loc"
                // Không cung cấp address & amenities
            };

            await Svc.UpdatePropertyAsync(dto, 9, 3);

            Assert.AreEqual(PropertyPostStatus.Draft, prop.Posts.First().Status);

            // ✅ Bổ sung verify cho GetByIdAsync
            Repo.Verify(r => r.GetByIdAsync(3, 9), Times.Once);

            // Không có amenities -> không gọi
            Repo.Verify(r => r.UpdateAmenitiesAsync(It.IsAny<int>(), It.IsAny<List<int>>()), Times.Never);

            Repo.Verify(r => r.UpdateAsync(prop), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
