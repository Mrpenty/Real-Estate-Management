using Microsoft.VisualStudio.TestTools.UnitTesting;             // ✅ thêm dòng này
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

using RealEstateManagement.Business.DTO.UserDTO;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Data.Entity.User;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RealEstateManagement.UnitTests.UserTest; // ✅ để dùng ToAsyncQueryable()

namespace RealEstateManagement.UnitTests.UserTest.ProfileServiceTest
{
    [TestClass]
    public class ProfileService_UpdateProfileAsync_Tests
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
        public async Task Fail_When_Email_Taken_By_Other_User()
        {
            var user = new ApplicationUser { Id = 1, Email = "old@x.com" };
            var other = new ApplicationUser { Id = 2, Email = "new@x.com" };

            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManager.Setup(m => m.FindByEmailAsync("new@x.com")).ReturnsAsync(other);

            var dto = new UpdateProfileDto { Name = "N", Email = "new@x.com", PhoneNumber = "111" };

            var result = await _service.UpdateProfileAsync(1, dto);

            Assert.IsFalse(result.Succeeded);
            StringAssert.Contains(result.Errors.First().Description, "Email is already taken");
        }

        [TestMethod]
        public async Task Fail_When_Phone_Taken_By_Other_User()
        {
            var user = new ApplicationUser { Id = 1, Email = "me@x.com", PhoneNumber = "000" };
            var others = new List<ApplicationUser>
            {
                new ApplicationUser { Id = 2, PhoneNumber = "111" }
            };

            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);

            // ✅ Trả về async IQueryable (bắt buộc để FirstOrDefaultAsync không lỗi)
            _userManager.SetupGet(m => m.Users).Returns(others.ToAsyncQueryable());

            var dto = new UpdateProfileDto { Name = "N", Email = "me@x.com", PhoneNumber = "111" };

            var result = await _service.UpdateProfileAsync(1, dto);

            Assert.IsFalse(result.Succeeded);
            StringAssert.Contains(result.Errors.First().Description, "Phone number is already taken");
        }

        [TestMethod]
        public async Task Success_Update_Basic_Fields_And_Call_UpdateAsync()
        {
            var user = new ApplicationUser
            {
                Id = 1,
                Email = "old@x.com",
                PhoneNumber = "000",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(user);
            _userManager.Setup(m => m.FindByEmailAsync("new@x.com")).ReturnsAsync((ApplicationUser)null);

            // ✅ Async IQueryable rỗng
            _userManager.SetupGet(m => m.Users).Returns(new List<ApplicationUser>().ToAsyncQueryable());

            _userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            var dto = new UpdateProfileDto
            {
                Name = "New Name",
                Email = "new@x.com",
                PhoneNumber = "111",
                ProfilePictureUrl = "/pp.png"
            };

            var result = await _service.UpdateProfileAsync(1, dto);

            Assert.IsTrue(result.Succeeded);
            _userManager.Verify(m => m.UpdateAsync(user), Times.Once);
        }
    }
}
