using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Identity;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RealEstateManagement.Business.Services.UploadPicService;
using RealEstateManagement.Business.DTO.UserDTO;
namespace RealEstateManagement.UnitTests.UserTest
{
    [TestClass]
    public class RequestVerificationAsyncTests
    {
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<ILogger<ProfileService>> _mockLogger;
        private Mock<IUploadPicService> _mockUploadPicService;
        private ProfileService _profileService;

        [TestInitialize]
        public void Setup()
        {
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            _mockLogger = new Mock<ILogger<ProfileService>>();
            _mockUploadPicService = new Mock<IUploadPicService>();

            _profileService = new ProfileService(_mockUserManager.Object, _mockLogger.Object, _mockUploadPicService.Object);
        }

        [TestMethod]
        public async Task RequestVerificationAsync_ValidRenter_ReturnsSuccess()
        {
            var userId = 1;
            var user = new ApplicationUser
            {
                Id = userId,
                Role = "renter",
                VerificationStatus = "none",
                CitizenIdNumber = "123456789",
                CitizenIdIssuedDate = DateTime.Now.AddYears(-1),
                CitizenIdExpiryDate = DateTime.Now.AddYears(5),
                CitizenIdFrontImageUrl = "front.jpg",
                CitizenIdBackImageUrl = "back.jpg"
            };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await _profileService.RequestVerificationAsync(userId);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("pending", user.VerificationStatus);
            _mockUserManager.Verify(m => m.UpdateAsync(user), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task RequestVerificationAsync_UserNotFound_ThrowsException()
        {
            var userId = 999;
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync((ApplicationUser)null);

            await _profileService.RequestVerificationAsync(userId);
        }

        [TestMethod]
        public async Task RequestVerificationAsync_UserIsNotRenter_ReturnsFailed()
        {
            var userId = 2;
            var user = new ApplicationUser { Id = userId, Role = "owner" };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);

            var result = await _profileService.RequestVerificationAsync(userId);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Chỉ người dùng renter mới có thể yêu cầu duyệt", result.Errors.First().Description);
        }

        [TestMethod]
        public async Task RequestVerificationAsync_MissingRequiredInfo_ReturnsFailed()
        {
            var userId = 3;
            var user = new ApplicationUser { Id = userId, Role = "renter", VerificationStatus = "none" };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);

            var result = await _profileService.RequestVerificationAsync(userId);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Vui lòng điền đầy đủ thông tin CCCD trước khi yêu cầu duyệt", result.Errors.First().Description);
        }

        [TestMethod]
        public async Task RequestVerificationAsync_StatusIsPending_ReturnsFailed()
        {
            var userId = 4;
            var user = new ApplicationUser
            {
                Id = userId,
                Role = "renter",
                VerificationStatus = "pending",
                CitizenIdNumber = "123456789",
                CitizenIdIssuedDate = DateTime.Now.AddYears(-1),
                CitizenIdExpiryDate = DateTime.Now.AddYears(5),
                CitizenIdFrontImageUrl = "front.jpg",
                CitizenIdBackImageUrl = "back.jpg"
            };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);

            var result = await _profileService.RequestVerificationAsync(userId);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Không thể yêu cầu duyệt với trạng thái hiện tại", result.Errors.First().Description);
        }

        [TestMethod]
        public async Task RequestVerificationAsync_StatusIsVerified_ReturnsFailed()
        {
            var userId = 5;
            var user = new ApplicationUser
            {
                Id = userId,
                Role = "renter",
                VerificationStatus = "verified",
                CitizenIdNumber = "123456789",
                CitizenIdIssuedDate = DateTime.Now.AddYears(-1),
                CitizenIdExpiryDate = DateTime.Now.AddYears(5),
                CitizenIdFrontImageUrl = "front.jpg",
                CitizenIdBackImageUrl = "back.jpg"
            };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);

            var result = await _profileService.RequestVerificationAsync(userId);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Không thể yêu cầu duyệt với trạng thái hiện tại", result.Errors.First().Description);
        }

        [TestMethod]
        public async Task RequestVerificationAsync_StatusIsRejected_ReturnsSuccessAndResetsReason()
        {
            var userId = 6;
            var user = new ApplicationUser
            {
                Id = userId,
                Role = "renter",
                VerificationStatus = "rejected",
                VerificationRejectReason = "Invalid details",
                CitizenIdNumber = "123456789",
                CitizenIdIssuedDate = DateTime.Now.AddYears(-1),
                CitizenIdExpiryDate = DateTime.Now.AddYears(5),
                CitizenIdFrontImageUrl = "front.jpg",
                CitizenIdBackImageUrl = "back.jpg"
            };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await _profileService.RequestVerificationAsync(userId);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("pending", user.VerificationStatus);
            Assert.IsNull(user.VerificationRejectReason);
        }

        [TestMethod]
        public async Task RequestVerificationAsync_UpdateFails_ReturnsFailed()
        {
            var userId = 7;
            var user = new ApplicationUser
            {
                Id = userId,
                Role = "renter",
                VerificationStatus = "none",
                CitizenIdNumber = "123456789",
                CitizenIdIssuedDate = DateTime.Now.AddYears(-1),
                CitizenIdExpiryDate = DateTime.Now.AddYears(5),
                CitizenIdFrontImageUrl = "front.jpg",
                CitizenIdBackImageUrl = "back.jpg"
            };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Update failed" }));

            var result = await _profileService.RequestVerificationAsync(userId);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Update failed", result.Errors.First().Description);
        }
    }
}