using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using RealEstateManagement.Business.DTO.UserDTO;
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
    public class ProfileService_ResetPasswordAsync_Tests
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
        public async Task Fail_When_User_Not_Found()
        {
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync((ApplicationUser)null);

            var result = await _service.ResetPasswordAsync(1, new ResetPasswordDto
            {
                OldPassword = "old",
                NewPassword = "new"
            });

            Assert.IsFalse(result.Succeeded);
        }

        [TestMethod]
        public async Task Success_When_ChangePassword_Ok()
        {
            var user = new ApplicationUser { Id = 2 };
            _userManager.Setup(m => m.FindByIdAsync("2")).ReturnsAsync(user);
            _userManager.Setup(m => m.ChangePasswordAsync(user, "old", "new"))
                        .ReturnsAsync(IdentityResult.Success);

            var result = await _service.ResetPasswordAsync(2, new ResetPasswordDto
            {
                OldPassword = "old",
                NewPassword = "new"
            });

            Assert.IsTrue(result.Succeeded);
            _userManager.Verify(m => m.ChangePasswordAsync(user, "old", "new"), Times.Once);
        }
    }

}
