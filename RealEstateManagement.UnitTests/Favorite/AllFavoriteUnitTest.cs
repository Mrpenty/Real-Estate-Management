using Moq;
using RealEstateManagement.Business.Repositories.FavoriteRepository;
using RealEstateManagement.Business.Services.Favorite;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.Payment;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Favorite
{
    [TestClass]
    public class AllFavoriteUnitTest
    {
        private Mock<IFavoriteRepository> _mockRepo = null!;
        private FavoriteService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IFavoriteRepository>();
            _service = new FavoriteService(_mockRepo.Object);
        }

        [TestMethod]
        public async Task AllFavoritePropertyAsync_MapsEntitiesToDtos_Correctly()
        {
            // Arrange
            var userId = 7;

            var p1 = new Property
            {
                Id = 10,
                Title = "Căn hộ cao cấp",
                Description = "View đẹp",
                Type = "Apartment",
                AddressId = 22,
                Area = 80,
                Bedrooms = 2,
                Price = 1500m,
                Status = "Available",
                Location = "Hanoi",
                Address = new Address
                {
                    DetailedAddress = "123 Hàng Bông",
                    Province = new Province { Name = "Hà Nội" },
                    Ward = new Ward { Name = "Phường A" },
                    Street = new Street { Name = "Hàng Bông" }
                },
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ViewsCount = 42,
                Images = new List<PropertyImage>
                {
                    new PropertyImage { Url = "img1.jpg",     IsPrimary = false },
                    new PropertyImage { Url = "primary1.jpg", IsPrimary = true  },
                },
                Landlord = new Data.Entity.User.ApplicationUser
                {
                    Name = "Mr. A",
                    PhoneNumber = "0909xxxxxx",
                    ProfilePictureUrl = "avt.jpg"
                },
                PropertyAmenities = new List<PropertyAmenity>
                {
                    new PropertyAmenity { Amenity = new Amenity { Name = "Pool" } },
                    new PropertyAmenity { Amenity = new Amenity { Name = "Gym"  } },
                },
                PropertyPromotions = new List<PropertyPromotion>
                {
                    new PropertyPromotion { PromotionPackage = new PromotionPackage { Name = "Silver", Level = 1 } },
                    new PropertyPromotion { PromotionPackage = new PromotionPackage { Name = "Gold",   Level = 3 } },
                    new PropertyPromotion { PromotionPackage = new PromotionPackage { Name = "Bronze", Level = 0 } },
                }
            };

            var p2 = new Property
            {
                Id = 20,
                Title = "Nhà phố",
                Description = "Gần trung tâm",
                Type = "House",
                AddressId = 33,
                Area = 120,
                Bedrooms = 3,
                Price = 2500m,
                Status = "Available",
                Location = "HCMC",
                Address = new Address
                {
                    DetailedAddress = "456 Lê Lợi",
                    Province = new Province { Name = "TP.HCM" },
                    Ward = new Ward { Name = "Phường B" },
                    Street = new Street { Name = "Lê Lợi" }
                },
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                ViewsCount = 12,
                Images = new List<PropertyImage>
                {
                    new PropertyImage { Url = "p2_primary.jpg", IsPrimary = true }
                },
                Landlord = new Data.Entity.User.ApplicationUser { Name = "Ms. B", PhoneNumber = "0912xxxxxx", ProfilePictureUrl = "b.jpg" },
                PropertyAmenities = new List<PropertyAmenity>
                {
                    new PropertyAmenity { Amenity = new Amenity { Name = "Parking" } },
                },
                PropertyPromotions = new List<PropertyPromotion>
                {
                    new PropertyPromotion { PromotionPackage = new PromotionPackage { Name = "Silver", Level = 1 } }
                }
            };

            _mockRepo.Setup(r => r.AllFavoritePropertyAsync(userId))
                     .ReturnsAsync(new List<Property> { p1, p2 });

            // Act
            var dtos = (await _service.AllFavoritePropertyAsync(userId)).ToList();

            // Assert
            Assert.AreEqual(2, dtos.Count);

            var d1 = dtos.First(d => d.Id == 10);
            Assert.AreEqual(p1.Title, d1.Title);
            Assert.AreEqual(p1.Description, d1.Description);
            Assert.AreEqual(p1.Type, d1.Type);
            Assert.AreEqual(p1.AddressId, d1.AddressID);
            Assert.AreEqual(p1.Area, d1.Area);
            Assert.AreEqual(p1.Bedrooms, d1.Bedrooms);
            Assert.AreEqual(p1.Price, d1.Price);
            Assert.AreEqual(p1.Status, d1.Status);
            Assert.AreEqual(p1.Location, d1.Location);
            Assert.AreEqual("123 Hàng Bông", d1.DetailedAddress);
            Assert.AreEqual("Hà Nội", d1.Province);
            Assert.AreEqual("Phường A", d1.Ward);
            Assert.AreEqual("Hàng Bông", d1.Street);
            Assert.AreEqual(p1.CreatedAt, d1.CreatedAt);
            Assert.AreEqual(p1.ViewsCount, d1.ViewsCount);
            Assert.AreEqual("primary1.jpg", d1.PrimaryImageUrl); // chọn ảnh IsPrimary
            Assert.AreEqual("Mr. A", d1.LandlordName);
            Assert.AreEqual("0909xxxxxx", d1.LandlordPhoneNumber);
            Assert.AreEqual("avt.jpg", d1.LandlordProfilePictureUrl);
            CollectionAssert.AreEquivalent(new[] { "Pool", "Gym" }, d1.Amenities!.ToList());
            Assert.AreEqual("Gold", d1.PromotionPackageName);     // Level cao nhất

            var d2 = dtos.First(d => d.Id == 20);
            Assert.AreEqual("p2_primary.jpg", d2.PrimaryImageUrl);
            Assert.AreEqual("Silver", d2.PromotionPackageName);
            Assert.AreEqual("Ms. B", d2.LandlordName);
        }

        [TestMethod]
        public async Task AllFavoritePropertyAsync_RepositoryThrows_PropagatesException()
        {
            var userId = 99;

            _mockRepo.Setup(r => r.AllFavoritePropertyAsync(userId))
                     .ThrowsAsync(new InvalidOperationException("DB error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _service.AllFavoritePropertyAsync(userId));
        }
    }
}
