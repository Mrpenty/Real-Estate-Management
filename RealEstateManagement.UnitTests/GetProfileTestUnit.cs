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

[TestClass]
public class GetProfileAsyncTests
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
    public async Task GetProfileAsync_UserExists_ReturnsViewProfileDto()
    {
        // Arrange
        var userId = 1;
        var user = new ApplicationUser
        {
            Id = userId,
            Name = "Test User",
            Email = "test@example.com",
            PhoneNumber = "1234567890",
            Role = "renter",
            IsVerified = true,
            ProfilePictureUrl = "http://example.com/profile.jpg"
        };
        var roles = new List<string> { "renter" };

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
        _mockUserManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(roles);

        // Act
        var result = await _profileService.GetProfileAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userId, result.Id);
        Assert.AreEqual(user.Name, result.Name);
        Assert.AreEqual(user.Email, result.Email);
        Assert.AreEqual(user.PhoneNumber, result.PhoneNumber);
        Assert.AreEqual(user.Role, result.Role);
        Assert.AreEqual(user.IsVerified, result.IsVerified);
        Assert.AreEqual(user.ProfilePictureUrl, result.ProfilePictureUrl);
    }

    [TestMethod]
    public async Task GetProfileAsync_UserDoesNotExist_ThrowsException()
    {
        // Arrange
        var userId = 999;
        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync((ApplicationUser)null);

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(() => _profileService.GetProfileAsync(userId));
    }

    [TestMethod]
    public async Task GetProfileAsync_UserHasNoRoles_ReturnsDefaultRole()
    {
        // Arrange
        var userId = 2;
        var user = new ApplicationUser { Id = userId, Role = "User" };
        var roles = new List<string>(); // User has no roles

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
        _mockUserManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(roles);

        // Act
        var result = await _profileService.GetProfileAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("User", result.Role);
    }

    [TestMethod]
    public async Task GetProfileAsync_UserWithMultipleRoles_ReturnsFirstRole()
    {
        // Arrange
        var userId = 3;
        var user = new ApplicationUser { Id = userId };
        var roles = new List<string> { "admin", "renter" };

        _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
        _mockUserManager.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(roles);

        // Act
        var result = await _profileService.GetProfileAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("admin", result.Role); // The first role in the list should be returned
    }
}