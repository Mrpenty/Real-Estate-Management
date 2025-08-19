using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyPostServiceTest
{
    [TestClass]
    public class GetPostsByStatusAsync_NoStatusTests : PropertyPostTestBase
    {
        [TestMethod]
        public async Task Returns_All_When_Status_Null_And_Pages()
        {
            // Arrange
            var posts = new List<PropertyPost>
    {
        new PropertyPost { Id = 1, Status = PropertyPost.PropertyPostStatus.Pending },
        new PropertyPost { Id = 2, Status = PropertyPost.PropertyPostStatus.Approved }
    };

            PostRepo.Setup(r => r.CountByStatusAsync(null))
                    .ReturnsAsync(posts.Count);

            PostRepo.Setup(r => r.GetPostsByStatusAsync(null, 1, 10))
                    .ReturnsAsync(posts);

            // Act
            var result = await Svc.GetPostsByStatusAsync(null, 1, 10);

            // Assert
            Assert.IsNotNull(result);
            PostRepo.Verify(r => r.CountByStatusAsync(null), Times.Once);
            PostRepo.Verify(r => r.GetPostsByStatusAsync(null, 1, 10), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }
        [TestMethod]
        public async Task Returns_Filtered_When_Status_Is_Pending()
        {
            // Arrange
            var posts = new List<PropertyPost>
    {
        new PropertyPost { Id = 1, Status = PropertyPost.PropertyPostStatus.Pending },
        new PropertyPost { Id = 2, Status = PropertyPost.PropertyPostStatus.Pending }
    };

            PostRepo.Setup(r => r.CountByStatusAsync(PropertyPost.PropertyPostStatus.Pending))
                    .ReturnsAsync(posts.Count);

            PostRepo.Setup(r => r.GetPostsByStatusAsync(PropertyPost.PropertyPostStatus.Pending, 1, 10))
                    .ReturnsAsync(posts);

            // Act
            var result = await Svc.GetPostsByStatusAsync("Pending", 1, 10);

            // Assert
            Assert.IsNotNull(result);
            PostRepo.Verify(r => r.CountByStatusAsync(PropertyPost.PropertyPostStatus.Pending), Times.Once);
            PostRepo.Verify(r => r.GetPostsByStatusAsync(PropertyPost.PropertyPostStatus.Pending, 1, 10), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }
        [TestMethod]
        public async Task Returns_Empty_When_No_Posts()
        {
            // Arrange
            PostRepo.Setup(r => r.CountByStatusAsync(null)).ReturnsAsync(0);
            PostRepo.Setup(r => r.GetPostsByStatusAsync(null, 1, 10))
                    .ReturnsAsync(new List<PropertyPost>());

            // Act
            var result = await Svc.GetPostsByStatusAsync(null, 1, 10);

            // Convert object ẩn danh sang dynamic JSON
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            dynamic obj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(json);

            // Assert
            Assert.AreEqual(0, (int)obj.total);
            Assert.AreEqual(0, obj.posts.Count);

            PostRepo.Verify(r => r.CountByStatusAsync(null), Times.Once);
            PostRepo.Verify(r => r.GetPostsByStatusAsync(null, 1, 10), Times.Once);
            PostRepo.VerifyNoOtherCalls();
        }



    }

}
