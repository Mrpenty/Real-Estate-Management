using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyPostServiceTest
{
    [TestClass]
    public class GetPostDetailForAdminAsync_MinimalTests : PropertyPostTestBase
    {
        [TestMethod]
        public async Task Returns_Object_When_Property_Exists_And_Landlord_Null()
        {
            // Arrange
            var post = new PropertyPost
            {
                Id = 1,
                Status = PropertyPost.PropertyPostStatus.Pending,
                Property = new Property { Id = 10, Title = "T" },
                Landlord = null
            };

            PostRepo.Setup(r => r.GetPostDetailForAdminAsync(1))
                    .ReturnsAsync(post);

            // Act
            var result = await Svc.GetPostDetailForAdminAsync(1);

            // Assert
            Assert.IsNotNull(result);
            PostRepo.Verify(r => r.GetPostDetailForAdminAsync(1), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }
        [TestMethod]
        public async Task Returns_Object_When_Property_Exists_With_Landlord()
        {
            // Arrange
            var landlord = new ApplicationUser
            {
                Id = 99,
                UserName = "landlord_a",
                Email = "a@ex.com",
                PhoneNumber = "0123",
                Name = "Landlord A"
            };

            var post = new PropertyPost
            {
                Id = 2,
                Status = PropertyPost.PropertyPostStatus.Approved,
                Property = new Property { Id = 20, Title = "House" },
                Landlord = landlord
            };

            PostRepo.Setup(r => r.GetPostDetailForAdminAsync(2))
                    .ReturnsAsync(post);

            // Act
            var result = await Svc.GetPostDetailForAdminAsync(2);

            // Assert
            Assert.IsNotNull(result);
            PostRepo.Verify(r => r.GetPostDetailForAdminAsync(2), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Returns_Null_When_Post_Not_Found()
        {
            // Arrange
            PostRepo.Setup(r => r.GetPostDetailForAdminAsync(999))
                    .ReturnsAsync((PropertyPost)null);

            // Act
            var result = await Svc.GetPostDetailForAdminAsync(999);

            // Assert
            Assert.IsNull(result);
            PostRepo.Verify(r => r.GetPostDetailForAdminAsync(999), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }


    }
}
