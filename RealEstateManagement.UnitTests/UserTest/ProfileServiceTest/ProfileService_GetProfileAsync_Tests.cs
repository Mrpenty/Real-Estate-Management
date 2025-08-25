using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.UserTest.ProfileServiceTest
{
    [TestClass]
    public class ProfileService_GetProfileAsync_Tests
    {
        private Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<ILogger<ProfileService>> _logger;
        private Mock<RealEstateManagement.Business.Services.UploadPicService.IUploadPicService> _upload;
        private ProfileService _service;

        [TestInitialize]
        public void Setup()
        {
            _userManager = UserManagerMockHelper.CreateMock<ApplicationUser>();
            _logger = new Mock<ILogger<ProfileService>>();
            _upload = new Mock<RealEstateManagement.Business.Services.UploadPicService.IUploadPicService>();
            _service = new ProfileService(_userManager.Object, _logger.Object, _upload.Object);
        }

        [TestMethod]
        public async Task Return_Profile_When_User_Exists()
        {
            var user = new ApplicationUser
            {
                Id = 42,
                Name = "Alice",
                Email = "alice@example.com",
                Role = "renter",
                PhoneNumber = "0123",
                ProfilePictureUrl = "/img/a.png",
                IsVerified = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedAt = System.DateTime.UtcNow,
                IsActive = true,
                CitizenIdFrontImageUrl = "/front.png",
                CitizenIdBackImageUrl = "/back.png",
                VerificationRejectReason = null,
                CitizenIdNumber = "123456789",
                CitizenIdIssuedDate = System.DateTime.UtcNow.AddYears(-1),
                CitizenIdExpiryDate = System.DateTime.UtcNow.AddYears(5),
                VerificationStatus = "none"
            };

            _userManager.Setup(m => m.FindByIdAsync("42")).ReturnsAsync(user);
            _userManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "renter" });

            var profile = await _service.GetProfileAsync(42);

            Assert.IsNotNull(profile);
            Assert.AreEqual(42, profile.Id);
            Assert.AreEqual("Alice", profile.Name);
            Assert.AreEqual("renter", profile.Role);
            Assert.AreEqual("/img/a.png", profile.ProfilePictureUrl);
        }

        [TestMethod]
        public async Task Throw_When_User_Not_Found()
        {
            _userManager.Setup(m => m.FindByIdAsync("99")).ReturnsAsync((ApplicationUser)null);

            await Assert.ThrowsExceptionAsync<System.Exception>(async () =>
                await _service.GetProfileAsync(99));
        }
    }
}
