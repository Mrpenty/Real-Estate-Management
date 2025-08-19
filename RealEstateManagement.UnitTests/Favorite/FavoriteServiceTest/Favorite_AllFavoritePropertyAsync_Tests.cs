using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.Properties;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.Payment;
using RealEstateManagement.Data.Entity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Favorite.FavoriteServiceTest
{
    [TestClass]
    public class Favorite_AllFavoritePropertyAsync_Tests : FavoriteServiceTestBase
    {
        [TestMethod]
        public async Task ReturnsEmptyList_WhenRepositoryReturnsEmpty()
        {
            Repo.Setup(r => r.AllFavoritePropertyAsync(7))
                .ReturnsAsync(new List<Property>());

            var result = (await Svc.AllFavoritePropertyAsync(7)).ToList();

            Assert.AreEqual(0, result.Count);
            Repo.Verify(r => r.AllFavoritePropertyAsync(7), Times.Once);
        }

        [TestMethod]
        public async Task MapsAllFields_FromEntityToHomePropertyDTO()
        {
            var prop = new Property
            {
                Id = 1,
                Title = "Nice House",
                Description = "Desc",
                Type = "House",              // nếu của bạn là enum, set enum tương ứng
                AddressId = 23,
                Area = 80,
                Bedrooms = 3,
                Price = 123456,
                Status = "Available",        // nếu là enum, set đúng enum
                Location = "HCM",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ViewsCount = 42,

                Address = new Address
                {
                    DetailedAddress = "123 Alley",
                    Province = new Province { Name = "HCM" },
                    Ward = new Ward { Name = "Ward 1" },
                    Street = new Street { Name = "Le Loi" }
                },
                Landlord = new ApplicationUser
                {
                    Name = "Owner A",
                    PhoneNumber = "0900000000",
                    ProfilePictureUrl = "owner.png"
                },
                Images = new List<PropertyImage>
                {
                    new PropertyImage{ Url = "primary.jpg", IsPrimary = true },
                    new PropertyImage{ Url = "other.jpg", IsPrimary = false },
                },
                PropertyAmenities = new List<PropertyAmenity>
                {
                    new PropertyAmenity { Amenity = new Amenity { Name = "Pool" } },
                    new PropertyAmenity { Amenity = new Amenity { Name = "Gym" } },
                },
                PropertyPromotions = new List<PropertyPromotion>
                {
                    new PropertyPromotion { PromotionPackage = new Data.Entity.Payment.PromotionPackage { Level = 2, Name = "Gold" } },
                    new PropertyPromotion { PromotionPackage = new Data.Entity.Payment.PromotionPackage { Level = 1, Name = "Silver" } },
                }
            };

            Repo.Setup(r => r.AllFavoritePropertyAsync(7))
                .ReturnsAsync(new List<Property> { prop });

            var list = (await Svc.AllFavoritePropertyAsync(7)).ToList();

            Assert.AreEqual(1, list.Count);
            var dto = list[0];

            Assert.AreEqual(prop.Id, dto.Id);
            Assert.AreEqual("Nice House", dto.Title);
            Assert.AreEqual("Desc", dto.Description);
            Assert.AreEqual("House", dto.Type);
            Assert.AreEqual(23, dto.AddressID);
            Assert.AreEqual(80, dto.Area);
            Assert.AreEqual(3, dto.Bedrooms);
            Assert.AreEqual(123456, dto.Price);
            Assert.AreEqual("Available", dto.Status);
            Assert.AreEqual("HCM", dto.Location);

            Assert.AreEqual("123 Alley", dto.DetailedAddress);
            Assert.AreEqual("HCM", dto.Province);
            Assert.AreEqual("Ward 1", dto.Ward);
            Assert.AreEqual("Le Loi", dto.Street);

            Assert.AreEqual(prop.CreatedAt, dto.CreatedAt);
            Assert.AreEqual(42, dto.ViewsCount);

            Assert.AreEqual("primary.jpg", dto.PrimaryImageUrl);
            Assert.AreEqual("Owner A", dto.LandlordName);
            Assert.AreEqual("0900000000", dto.LandlordPhoneNumber);
            Assert.AreEqual("owner.png", dto.LandlordProfilePictureUrl);

            CollectionAssert.AreEquivalent(new[] { "Pool", "Gym" }, dto.Amenities);
            Assert.AreEqual("Gold", dto.PromotionPackageName); // chọn gói Level cao nhất

            Repo.Verify(r => r.AllFavoritePropertyAsync(7), Times.Once);
        }

        [TestMethod]
        public async Task PropagatesException_WhenRepositoryThrows()
        {
            Repo.Setup(r => r.AllFavoritePropertyAsync(9))
                .ThrowsAsync(new InvalidOperationException("db error"));

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.AllFavoritePropertyAsync(9));
        }

    }
}
