using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Identity;
using RealEstateManagement.Business.Services.User;
using RealEstateManagement.Data.Entity.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using RealEstateManagement.Business.DTO.UserDTO;
using Microsoft.Extensions.Logging;
using RealEstateManagement.Business.Services.UploadPicService;
using RealEstateManagement.Data.Entity.User;

[TestClass]
public class ProfileServiceTests
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
    public async Task UpdateProfileAsync_ValidData_ReturnsSuccess()
    {
        var userId = 1;
        var existingUser = new ApplicationUser { Id = userId, Name = "Old Name", Email = "old@example.com", PhoneNumber = "1234567890" };
        var updateDto = new UpdateProfileDto { Name = "New Name", Email = "new@example.com", PhoneNumber = "0987654321" };

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(existingUser);
        _mockUserManager.Setup(m => m.FindByEmailAsync(updateDto.Email)).ReturnsAsync((ApplicationUser)null);
        _mockUserManager.Setup(m => m.Users).Returns(new List<ApplicationUser>().AsQueryable());
        _mockUserManager.Setup(m => m.UpdateAsync(existingUser)).ReturnsAsync(IdentityResult.Success);

        var result = await _profileService.UpdateProfileAsync(userId, updateDto);

        Assert.IsTrue(result.Succeeded);
        Assert.AreEqual(updateDto.Name, existingUser.Name);
        Assert.AreEqual(updateDto.Email, existingUser.Email);
        Assert.AreEqual(updateDto.PhoneNumber, existingUser.PhoneNumber);
        _mockUserManager.Verify(m => m.UpdateAsync(existingUser), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public async Task UpdateProfileAsync_UserNotFound_ThrowsException()
    {
        var userId = 999;
        var updateDto = new UpdateProfileDto { Name = "New Name" };

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync((ApplicationUser)null);

        await _profileService.UpdateProfileAsync(userId, updateDto);
    }

    [TestMethod]
    public async Task UpdateProfileAsync_EmailAlreadyTaken_ReturnsFailedResult()
    {
        var userId = 1;
        var existingUser = new ApplicationUser { Id = userId, Email = "old@example.com" };
        var otherUserWithSameEmail = new ApplicationUser { Id = 2, Email = "taken@example.com" };
        var updateDto = new UpdateProfileDto { Email = "taken@example.com" };

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(existingUser);
        _mockUserManager.Setup(m => m.FindByEmailAsync(updateDto.Email)).ReturnsAsync(otherUserWithSameEmail);

        var result = await _profileService.UpdateProfileAsync(userId, updateDto);

        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual("Email is already taken by another user", result.Errors.First().Description);
    }

    [TestMethod]
    public async Task UpdateProfileAsync_PhoneNumberAlreadyTaken_ReturnsFailedResult()
    {
        var userId = 1;
        var existingUser = new ApplicationUser { Id = userId, PhoneNumber = "1234567890" };
        var otherUserWithSamePhone = new ApplicationUser { Id = 2, PhoneNumber = "0987654321" };
        var updateDto = new UpdateProfileDto { PhoneNumber = "0987654321" };

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(existingUser);
        _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
        _mockUserManager.Setup(m => m.Users).Returns(new List<ApplicationUser> { existingUser, otherUserWithSamePhone }.AsQueryable());

        var result = await _profileService.UpdateProfileAsync(userId, updateDto);

        Assert.IsFalse(result.Succeeded);
        Assert.AreEqual("Phone number is already taken by another user", result.Errors.First().Description);
    }

    [TestMethod]
    public async Task UpdateProfileAsync_EmailChanged_SetsEmailConfirmedToFalse()
    {
        var userId = 1;
        var existingUser = new ApplicationUser { Id = userId, Email = "old@example.com", EmailConfirmed = true };
        var updateDto = new UpdateProfileDto { Email = "new@example.com" };

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(existingUser);
        _mockUserManager.Setup(m => m.FindByEmailAsync(updateDto.Email)).ReturnsAsync((ApplicationUser)null);
        _mockUserManager.Setup(m => m.Users).Returns(new List<ApplicationUser>().AsQueryable());
        _mockUserManager.Setup(m => m.UpdateAsync(existingUser)).ReturnsAsync(IdentityResult.Success);

        var result = await _profileService.UpdateProfileAsync(userId, updateDto);

        Assert.IsTrue(result.Succeeded);
        Assert.IsFalse(existingUser.EmailConfirmed);
    }

    [TestMethod]
    public async Task UpdateProfileAsync_PhoneNumberChanged_SetsPhoneNumberConfirmedToFalse()
    {
        var userId = 1;
        var existingUser = new ApplicationUser { Id = userId, PhoneNumber = "1234567890", PhoneNumberConfirmed = true };
        var updateDto = new UpdateProfileDto { PhoneNumber = "0987654321" };

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(existingUser);
        _mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
        _mockUserManager.Setup(m => m.Users).Returns(new List<ApplicationUser> { existingUser }.AsQueryable());
        _mockUserManager.Setup(m => m.UpdateAsync(existingUser)).ReturnsAsync(IdentityResult.Success);

        var result = await _profileService.UpdateProfileAsync(userId, updateDto);

        Assert.IsTrue(result.Succeeded);
        Assert.IsFalse(existingUser.PhoneNumberConfirmed);
    }
}