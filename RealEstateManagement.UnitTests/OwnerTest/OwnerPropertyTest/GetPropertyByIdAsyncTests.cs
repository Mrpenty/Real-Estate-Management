// GetPropertyByIdAsyncTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace RealEstateManagement.UnitTests.OwnerTest.OwnerPropertyTest
{
    [TestClass]
    public class GetPropertyByIdAsyncTests : OwnerPropertyTestBase_InMemory
    {
        [TestMethod]
        public async Task Throws_When_NotFound()
        {
            // Service đang gọi Repo.GetByIdAsync => phải setup null để service ném Exception
            Repo.Setup(r => r.GetByIdAsync(99, 1)).ReturnsAsync((Property)null!);

            await Assert.ThrowsExceptionAsync<Exception>(() =>
                Svc.GetPropertyByIdAsync(id: 99, landlordId: 1));

            Repo.Verify(r => r.GetByIdAsync(99, 1), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Maps_Fields_When_Found()
        {
            var entity = new Property
            {
                Id = 10,
                LandlordId = 5,
                // 🔴 Required của Property:
                Title = "Nice",
                Description = "Good",
                PropertyTypeId = 1,
                PropertyType = new PropertyType { Id = 1, Name = "Condo" },
                Area = 80,
                Bedrooms = 3,
                Bathrooms = 2,
                Floors = 1,
                Status = "Pending",
                IsPromoted = false,
                Price = 2000,
                IsVerified = true,
                ViewsCount = 0,
                Location = "HN",
                CreatedAt = DateTime.UtcNow,
                AddressId = 1,
                Address = new Address
                {
                    Id = 1,
                    ProvinceId = 1,
                    Province = new Province { Id = 1, Name = "HN" },
                    WardId = 2,
                    Ward = new Ward { Id = 2, Name = "W2" },
                    StreetId = 3,
                    Street = new Street { Id = 3, Name = "Tran Duy Hung" },
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
                        RentalContract = new RentalContract
                        {
                            Id = 777,
                            PropertyPostId = 88,
                            MonthlyRent = 2000,
                            // 🔴 Required của RentalContract theo lỗi bạn nhận:
                            ContactInfo = "0900-000-000",
                            PaymentMethod = "Cash"
                        }
                    }
                }
            };

            // Service gọi Repo => trả entity trực tiếp từ repo
            Repo.Setup(r => r.GetByIdAsync(10, 5)).ReturnsAsync(entity);

            var dto = await Svc.GetPropertyByIdAsync(id: 10, landlordId: 5);

            // Address
            Assert.AreEqual(10, dto.Id);
            Assert.AreEqual("HN", dto.Province);
            Assert.AreEqual("W2", dto.Ward);
            Assert.AreEqual("Tran Duy Hung", dto.Street);
            Assert.AreEqual("No.1", dto.DetailedAddress);

            // Basics
            Assert.AreEqual("Nice", dto.Title);
            Assert.AreEqual("Good", dto.Description);
            Assert.AreEqual(2000, dto.Price);
            Assert.AreEqual(80, dto.Area);
            Assert.AreEqual(3, dto.Bedrooms);
            Assert.AreEqual("HN", dto.Location);
            Assert.IsFalse(dto.IsPromoted);
            Assert.IsTrue(dto.IsVerified);

            // Images
            Assert.AreEqual("p1", dto.PrimaryImageUrl);
            CollectionAssert.AreEquivalent(new[] { "p1", "p2" }, dto.ImageUrls!);

            // Amenities
            CollectionAssert.AreEquivalent(new[] { "Gym", "Pool" }, dto.Amenities!);

            // Posts + contract
            Assert.IsNotNull(dto.Posts);
            Assert.AreEqual(1, dto.Posts!.Count);
            Assert.IsNotNull(dto.Posts[0].RentalContract);
            Assert.AreEqual(777, dto.Posts[0].RentalContract!.Id);

            Repo.Verify(r => r.GetByIdAsync(10, 5), Times.Once);
            Repo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task NullSafe_When_Address_Images_Amenities_Posts_Null()
        {
            var entity = new Property
            {
                Id = 20,
                LandlordId = 3,
                // 🔴 Required của Property:
                Title = "Nullables",
                Description = "N",
                PropertyTypeId = 1,
                Area = 1,
                Bedrooms = 1,
                Bathrooms = 1,
                Floors = 1,
                Status = "Active",
                IsPromoted = false,
                Price = 1,
                IsVerified = false,
                ViewsCount = 0,
                Location = "HN",  // bắt buộc theo lỗi trước
                CreatedAt = DateTime.UtcNow,
                AddressId = 0,    // nếu FK required, bạn có thể để 0 và Address=null nếu model cho phép
                Address = null,
                Images = null,
                PropertyAmenities = null,
                Posts = null
            };

            Repo.Setup(r => r.GetByIdAsync(20, 3)).ReturnsAsync(entity);

            var dto = await Svc.GetPropertyByIdAsync(id: 20, landlordId: 3);

            // basics
            Assert.AreEqual(20, dto.Id);
            Assert.AreEqual("Nullables", dto.Title);
            Assert.AreEqual("N", dto.Description);
            Assert.AreEqual(1, dto.Price);
            Assert.AreEqual(1, dto.Area);
            Assert.AreEqual(1, dto.Bedrooms);

            // address null-safe
            Assert.IsNull(dto.Province);
            Assert.IsNull(dto.Ward);
            Assert.IsNull(dto.Street);
            Assert.IsNull(dto.DetailedAddress);

            // images null-safe
            Assert.IsTrue(dto.ImageUrls == null || dto.ImageUrls.Count == 0);
            Assert.IsTrue(string.IsNullOrEmpty(dto.PrimaryImageUrl));

            // amenities null-safe
            Assert.AreEqual(0, dto.Amenities?.Count ?? 0);

            // posts null-safe
            Assert.IsTrue(dto.Posts == null || dto.Posts.Count == 0);

            Repo.Verify(r => r.GetByIdAsync(20, 3), Times.Once);
            Repo.VerifyNoOtherCalls();
        }
    }
}
