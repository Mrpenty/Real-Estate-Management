using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.ReportEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyPostServiceTest
{
    [TestClass]
    public class BanPropertyPost_InvalidActionTests : PropertyPostTestBase
    {
        [TestMethod]
        public async Task Ban_Sets_Inactive_And_Resolves_Pending_Reports()
        {
            // Arrange
            var post = new PropertyPost
            {
                Id = 5,
                Property = new Property { Id = 1, Status = "active" }
            };
            var reports = new List<Report>
            {
                new Report { Id = 1, TargetType = "PropertyPost", TargetId = 5, Status = "Pending" }
            };

            PostRepo.Setup(r => r.GetPostWithPropertyAsync(5)).ReturnsAsync(post);
            PostRepo.Setup(r => r.GetReportsForPostAsync(5)).ReturnsAsync(reports);
            PostRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await Svc.BanPropertyPost(5, "ban", 999, "note");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("inactive", post.Property.Status);
            Assert.AreEqual("Resolved", reports[0].Status);
            Assert.AreEqual(999, reports[0].ResolvedByUserId);
            PostRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Unban_Sets_Active_And_Ignores_Resolved_Reports()
        {
            // Arrange
            var post = new PropertyPost
            {
                Id = 5,
                Property = new Property { Id = 1, Status = "inactive" }
            };
            var reports = new List<Report>
            {
                new Report { Id = 1, TargetType = "PropertyPost", TargetId = 5, Status = "Resolved" }
            };

            PostRepo.Setup(r => r.GetPostWithPropertyAsync(5)).ReturnsAsync(post);
            PostRepo.Setup(r => r.GetReportsForPostAsync(5)).ReturnsAsync(reports);
            PostRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await Svc.BanPropertyPost(5, "unban", 1000, "restore");

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("active", post.Property.Status);
            Assert.AreEqual("Ignored", reports[0].Status);
            Assert.AreEqual(1000, reports[0].ResolvedByUserId);
            PostRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Throws_When_Action_Invalid()
        {
            // Arrange
            var post = new PropertyPost
            {
                Id = 5,
                Property = new Property { Id = 1, Status = "active" }
            };

            PostRepo.Setup(r => r.GetPostWithPropertyAsync(5)).ReturnsAsync(post);
            PostRepo.Setup(r => r.GetReportsForPostAsync(5)).ReturnsAsync(new List<Report>());

            // Act + Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
                Svc.BanPropertyPost(5, "pause", 999, "note"));

            // Verify: không save khi action invalid
            PostRepo.Verify(r => r.SaveChangesAsync(), Times.Never);
        }
        [TestMethod]
        public async Task Unban_ReturnsFalse_When_No_Resolved_Reports_To_Update()
        {
            // Arrange: post ở trạng thái inactive
            var post = new PropertyPost
            {
                Id = 7,
                Property = new Property { Id = 1, Status = "inactive" }
            };

            // 👇 reports đều Pending, không có Resolved
            var reports = new List<Report>
    {
        new Report { Id = 1, TargetType = "PropertyPost", TargetId = 7, Status = "Pending" },
        new Report { Id = 2, TargetType = "PropertyPost", TargetId = 7, Status = "Pending" }
    };

            PostRepo.Setup(r => r.GetPostWithPropertyAsync(7)).ReturnsAsync(post);
            PostRepo.Setup(r => r.GetReportsForPostAsync(7)).ReturnsAsync(reports);
            PostRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            var result = await Svc.BanPropertyPost(7, "unban", 123, "try restore");

            // Assert: kỳ vọng hợp lý là false (không có report nào update được)
            // Nhưng code hiện tại luôn trả true -> test này sẽ FAIL tự nhiên.
            Assert.IsFalse(result);
        }

    }
}
