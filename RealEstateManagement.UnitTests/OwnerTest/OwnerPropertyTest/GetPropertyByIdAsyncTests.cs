using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.AddressEnity;
using System.Collections.Generic;
using System.Threading.Tasks;
using RealEstateManagement.Data.Entity;

namespace RealEstateManagement.UnitTests.OwnerTest.OwnerPropertyTest
{
    [TestClass]
    public class GetPropertyByIdAsyncTests : OwnerPropertyTestBase
    {
        [TestMethod]
        public async Task Throws_When_NotFound()
        {
            Repo.Setup(r => r.GetByIdAsync(99, 1)).ReturnsAsync((Property)null!);

            await Assert.ThrowsExceptionAsync<System.Exception>(() =>
                Svc.GetPropertyByIdAsync(99, 1));

            Repo.Verify(r => r.GetByIdAsync(99, 1), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Maps_Fields_When_Found()
        {
            var entity = new Property
            {
                Id = 10,
                Title = "Nice",
                Description = "Good",
                Price = 2000,
                Area = 80,
                Bedrooms = 3,
                Type = "House",
                IsPromoted = false,
                IsVerified = true,
                Location = "HN",
                Address = new Address
                {
                    ProvinceId = 1,
                    Province = new Province { Name = "HN" },
                    WardId = 2,
                    Ward = new Ward { Name = "W2" },
                    StreetId = 3,
                    Street = new Street { Name = "Tran Duy Hung" },
                    DetailedAddress = "No.1"
                },
                Images = new List<PropertyImage>
                {
                    new PropertyImage{ Id=1, Url="p1", IsPrimary=true },
                    new PropertyImage{ Id=2, Url="p2", IsPrimary=false }
                },
                PropertyAmenities = new List<PropertyAmenity>
                {
                    new PropertyAmenity{ Amenity=new Amenity{ Name="Gym"} },
                    new PropertyAmenity{ Amenity=new Amenity{ Name="Pool"} },
                },
                Posts = new List<PropertyPost>
                {
                    new PropertyPost
                    {
                        Id = 88,
                        Status = PropertyPost.PropertyPostStatus.Pending,
                        RentalContract = new RentalContract { Id=777, PropertyPostId=88, MonthlyRent=2000 }
                    }
                }
            };

            Repo.Setup(r => r.GetByIdAsync(10, 5)).ReturnsAsync(entity);

            var dto = await Svc.GetPropertyByIdAsync(10, 5);

            Assert.AreEqual(10, dto.Id);
            Assert.AreEqual("HN", dto.Province);
            Assert.AreEqual("W2", dto.Ward);
            Assert.AreEqual("Tran Duy Hung", dto.Street);
            Assert.AreEqual("p1", dto.PrimaryImageUrl);
            Assert.AreEqual(2, dto.ImageUrls!.Count);
            CollectionAssert.AreEquivalent(new[] { "Gym", "Pool" }, dto.Amenities!);
            Assert.IsNotNull(dto.Posts);
            Assert.AreEqual(1, dto.Posts!.Count);
            Assert.AreEqual(777, dto.Posts[0].RentalContract!.Id);

            Repo.Verify(r => r.GetByIdAsync(10, 5), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task NullSafe_When_Address_Images_Posts_Null()
        {
            var entity = new Property
            {
                Id = 20,
                Title = "Nullables",
                Description = "N",
                Price = 1,
                Area = 1,
                Bedrooms = 1,
                Type = "A",
                Address = null,
                Images = null,
                PropertyAmenities = null,
                Posts = null
            };

            Repo.Setup(r => r.GetByIdAsync(20, 3)).ReturnsAsync(entity);

            var dto = await Svc.GetPropertyByIdAsync(20, 3);

            Assert.AreEqual(20, dto.Id);
            Assert.IsNull(dto.Province);
            Assert.IsNull(dto.PrimaryImageUrl);
            Assert.IsNull(dto.ImageUrls);
            Assert.AreEqual(0, dto.Amenities!.Count);
            Assert.IsNull(dto.Posts);

            Repo.Verify(r => r.GetByIdAsync(20, 3), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
