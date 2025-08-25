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
    public class ProfileService_UploadCccdImageAsync_Tests
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
            _userManager.Setup(m => m.FindByIdAsync("10")).ReturnsAsync((ApplicationUser)null);

            var result = await _service.UploadCccdImageAsync(10, new Mock<IFormFile>().Object, isFront: true);

            Assert.IsFalse(result.Succeeded);
            StringAssert.Contains(result.Errors.First(), "User not found");
        }

        [TestMethod]
        public async Task Fail_When_Upload_Front_Fails()
        {
            var user = new ApplicationUser { Id = 11 };
            _userManager.Setup(m => m.FindByIdAsync("11")).ReturnsAsync(user);
            _upload.Setup(u => u.UploadImageAsync(It.IsAny<IFormFile>(), "cccd-front", "cccd_front_11"))
                   .ReturnsAsync(UploadResult.Failure("invalid"));

            var result = await _service.UploadCccdImageAsync(11, new Mock<IFormFile>().Object, isFront: true);

            Assert.IsFalse(result.Succeeded);
            StringAssert.Contains(result.Errors.First(), "invalid");
            _userManager.Verify(m => m.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }

        [TestMethod]
        public async Task Success_Front_Replaces_Old_And_Update_User()
        {
            var user = new ApplicationUser { Id = 12, CitizenIdFrontImageUrl = "/old-front.png" };
            _userManager.Setup(m => m.FindByIdAsync("12")).ReturnsAsync(user);
            _upload.Setup(u => u.UploadImageAsync(It.IsAny<IFormFile>(), "cccd-front", "cccd_front_12"))
                   .ReturnsAsync(UploadResult.Success("/new-front.png"));
            _upload.Setup(u => u.DeleteImageAsync("/old-front.png")).ReturnsAsync(true);
            _userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await _service.UploadCccdImageAsync(12, new Mock<IFormFile>().Object, isFront: true);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("/new-front.png", user.CitizenIdFrontImageUrl);
            _upload.Verify(u => u.DeleteImageAsync("/old-front.png"), Times.Once);
            _userManager.Verify(m => m.UpdateAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task Success_Back_Replaces_Old_And_Update_User()
        {
            var user = new ApplicationUser { Id = 13, CitizenIdBackImageUrl = "/old-back.png" };
            _userManager.Setup(m => m.FindByIdAsync("13")).ReturnsAsync(user);
            _upload.Setup(u => u.UploadImageAsync(It.IsAny<IFormFile>(), "cccd-back", "cccd_back_13"))
                   .ReturnsAsync(UploadResult.Success("/new-back.png"));
            _upload.Setup(u => u.DeleteImageAsync("/old-back.png")).ReturnsAsync(true);
            _userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var result = await _service.UploadCccdImageAsync(13, new Mock<IFormFile>().Object, isFront: false);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual("/new-back.png", user.CitizenIdBackImageUrl);
            _upload.Verify(u => u.DeleteImageAsync("/old-back.png"), Times.Once);
            _userManager.Verify(m => m.UpdateAsync(user), Times.Once);
        }

        [TestMethod]
        public async Task Fail_When_Update_User_Fails()
        {
            var user = new ApplicationUser { Id = 14 };
            _userManager.Setup(m => m.FindByIdAsync("14")).ReturnsAsync(user);
            _upload.Setup(u => u.UploadImageAsync(It.IsAny<IFormFile>(), "cccd-back", "cccd_back_14"))
                   .ReturnsAsync(UploadResult.Success("/ok.png"));
            _userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "DB err" }));

            var result = await _service.UploadCccdImageAsync(14, new Mock<IFormFile>().Object, isFront: false);

            Assert.IsFalse(result.Succeeded);
            StringAssert.Contains(result.Errors.First(), "DB err");
        }
    }
}
