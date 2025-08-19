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
    public class ProfileService_RequestVerificationAsync_Tests
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
        public async Task Fail_When_Not_Renter_Role()
        {
            var user = new ApplicationUser { Id = 1, Role = "owner" };
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);

            var result = await _service.RequestVerificationAsync(1);

            Assert.IsFalse(result.Succeeded);
            StringAssert.Contains(result.Errors.First().Description, "renter");
        }

        [TestMethod]
        public async Task Fail_When_Missing_Required_Fields()
        {
            var user = new ApplicationUser { Id = 1, Role = "renter" /* thiếu CCCD */ };
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);

            var result = await _service.RequestVerificationAsync(1);

            Assert.IsFalse(result.Succeeded);
            StringAssert.Contains(result.Errors.First().Description, "CCCD");
        }

        [TestMethod]
        public async Task Fail_When_Status_Not_Allowed()
        {
            var user = new ApplicationUser
            {
                Id = 1,
                Role = "renter",
                CitizenIdNumber = "123",
                CitizenIdIssuedDate = System.DateTime.UtcNow.AddYears(-1),
                CitizenIdExpiryDate = System.DateTime.UtcNow.AddYears(5),
                CitizenIdFrontImageUrl = "/f.png",
                CitizenIdBackImageUrl = "/b.png",
                VerificationStatus = "pending" // không cho
            };
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);

            var result = await _service.RequestVerificationAsync(1);

            Assert.IsFalse(result.Succeeded);
            StringAssert.Contains(result.Errors.First().Description, "Không thể yêu cầu duyệt");
        }

        [TestMethod]
        public async Task Success_Set_Pending_And_Clear_RejectReason()
        {
            var user = new ApplicationUser
            {
                Id = 1,
                Role = "renter",
                CitizenIdNumber = "123",
                CitizenIdIssuedDate = System.DateTime.UtcNow.AddYears(-1),
                CitizenIdExpiryDate = System.DateTime.UtcNow.AddYears(5),
                CitizenIdFrontImageUrl = "/f.png",
                CitizenIdBackImageUrl = "/b.png",
                VerificationStatus = "none",
                VerificationRejectReason = "old reason"
            };
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await _service.RequestVerificationAsync(1);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("pending", user.VerificationStatus);
            Assert.IsNull(user.VerificationRejectReason);
            _userManager.Verify(m => m.UpdateAsync(user), Times.Once);
        }
    }
}
