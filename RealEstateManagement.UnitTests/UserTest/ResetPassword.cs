using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Identity;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Data.Entity.User;
using System;
using System.Threading.Tasks;
using RealEstateManagement.Business.DTO.UserDTO;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using RealEstateManagement.Business.Services.UploadPicService;

namespace RealEstateManagement.UnitTests.UserTest
{
    [TestClass]
    public class ResetPasswordTests
    {
        private Mock<UserManager<ApplicationUser>> _mockUserManager;
        private Mock<IUploadPicService> _mockUploadPicService;
        private ProfileService _profileService;
        private Mock<ILogger<ProfileService>> _mockLogger;

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
        public async Task ResetPasswordAsync_ValidCredentials_ReturnsSuccess()
        {
            var userId = 1;
            var user = new ApplicationUser { Id = userId };
            var resetPasswordDto = new ResetPasswordDto { OldPassword = "OldPassword123!", NewPassword = "NewPassword123!" };

            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.ChangePasswordAsync(user, resetPasswordDto.OldPassword, resetPasswordDto.NewPassword))
                            .ReturnsAsync(IdentityResult.Success);

            var result = await _profileService.ResetPasswordAsync(userId, resetPasswordDto);

            Assert.IsTrue(result.Succeeded);
            _mockUserManager.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
            _mockUserManager.Verify(m => m.ChangePasswordAsync(user, resetPasswordDto.OldPassword, resetPasswordDto.NewPassword), Times.Once);
        }

        [TestMethod]
        public async Task ResetPasswordAsync_UserNotFound_ReturnsFailed()
        {
            var userId = 999;
            var resetPasswordDto = new ResetPasswordDto { OldPassword = "OldPassword123!", NewPassword = "NewPassword123!" };

            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync((ApplicationUser)null);

            var result = await _profileService.ResetPasswordAsync(userId, resetPasswordDto);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("User not found.", result.Errors.First().Description);
            _mockUserManager.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
            _mockUserManager.Verify(m => m.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task ResetPasswordAsync_IncorrectOldPassword_ReturnsFailed()
        {
            var userId = 1;
            var user = new ApplicationUser { Id = userId };
            var resetPasswordDto = new ResetPasswordDto { OldPassword = "IncorrectPassword", NewPassword = "NewPassword123!" };
            var error = new IdentityError { Description = "Incorrect old password." };

            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.ChangePasswordAsync(user, resetPasswordDto.OldPassword, resetPasswordDto.NewPassword))
                            .ReturnsAsync(IdentityResult.Failed(error));

            var result = await _profileService.ResetPasswordAsync(userId, resetPasswordDto);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(error.Description, result.Errors.First().Description);
            _mockUserManager.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
            _mockUserManager.Verify(m => m.ChangePasswordAsync(user, resetPasswordDto.OldPassword, resetPasswordDto.NewPassword), Times.Once);
        }

        [TestMethod]
        public async Task ResetPasswordAsync_InvalidNewPassword_ReturnsFailed()
        {
            var userId = 1;
            var user = new ApplicationUser { Id = userId };
            var resetPasswordDto = new ResetPasswordDto { OldPassword = "OldPassword123!", NewPassword = "weak" };
            var error = new IdentityError { Description = "Password must be at least 8 characters long." };

            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.ChangePasswordAsync(user, resetPasswordDto.OldPassword, resetPasswordDto.NewPassword))
                            .ReturnsAsync(IdentityResult.Failed(error));

            var result = await _profileService.ResetPasswordAsync(userId, resetPasswordDto);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(error.Description, result.Errors.First().Description);
            _mockUserManager.Verify(m => m.FindByIdAsync(userId.ToString()), Times.Once);
            _mockUserManager.Verify(m => m.ChangePasswordAsync(user, resetPasswordDto.OldPassword, resetPasswordDto.NewPassword), Times.Once);
        }
    }
}