using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using RealEstateManagement.Business.Services.UploadPicService;
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
    public class ProfileService_UploadProfilePictureAsync_Tests
    {
        private Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<ILogger<ProfileService>> _logger;
        private Mock<IUploadPicService> _upload;
        private ProfileService _service;

        [TestInitialize]
        public void Setup()
        {
            _userManager = UserManagerMockHelper.CreateMock<ApplicationUser>();
            _logger = new Mock<ILogger<ProfileService>>();
            _upload = new Mock<IUploadPicService>();
            _service = new ProfileService(_userManager.Object, _logger.Object, _upload.Object);
        }

        [TestMethod]
        public async Task Fail_When_User_Not_Found()
        {
            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync((ApplicationUser)null);
            var file = new Mock<IFormFile>().Object;

            var result = await _service.UploadProfilePictureAsync(1, file);

            Assert.IsFalse(result.Succeeded);
            StringAssert.Contains(result.Errors.First(), "User not found");
        }

        [TestMethod]
        public async Task Fail_When_Upload_Fails()
        {
            var user = new ApplicationUser { Id = 2 };
            _userManager.Setup(m => m.FindByIdAsync("2")).ReturnsAsync(user);

            _upload.Setup(u => u.UploadImageAsync(It.IsAny<IFormFile>(), "profile-pictures", "profile_2"))
                   .ReturnsAsync(UploadResult.Failure("bad"));

            var file = new Mock<IFormFile>().Object;

            var result = await _service.UploadProfilePictureAsync(2, file);

            Assert.IsFalse(result.Succeeded);
            StringAssert.Contains(result.Errors.First(), "bad");
            _userManager.Verify(m => m.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [TestMethod]
        public async Task Success_Delete_Old_If_Exists_And_Update_User()
        {
            var user = new ApplicationUser { Id = 3, ProfilePictureUrl = "/old.png" };
            _userManager.Setup(m => m.FindByIdAsync("3")).ReturnsAsync(user);
            _upload.Setup(u => u.UploadImageAsync(It.IsAny<IFormFile>(), "profile-pictures", "profile_3"))
                   .ReturnsAsync(UploadResult.Success("/new.png"));
            _upload.Setup(u => u.DeleteImageAsync("/old.png")).ReturnsAsync(true);
            _userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var file = new Mock<IFormFile>().Object;

            var result = await _service.UploadProfilePictureAsync(3, file);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("/new.png", user.ProfilePictureUrl);
            _upload.Verify(u => u.DeleteImageAsync("/old.png"), Times.Once);
            _userManager.Verify(m => m.UpdateAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task Fail_When_Update_User_Fails()
        {
            var user = new ApplicationUser { Id = 4, ProfilePictureUrl = null };
            _userManager.Setup(m => m.FindByIdAsync("4")).ReturnsAsync(user);
            _upload.Setup(u => u.UploadImageAsync(It.IsAny<IFormFile>(), "profile-pictures", "profile_4"))
                   .ReturnsAsync(UploadResult.Success("/new.png"));
            _userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "DB error" }));

            var file = new Mock<IFormFile>().Object;

            var result = await _service.UploadProfilePictureAsync(4, file);

            Assert.IsFalse(result.Succeeded);
            StringAssert.Contains(result.Errors.First(), "DB error");
        }
    }
}
