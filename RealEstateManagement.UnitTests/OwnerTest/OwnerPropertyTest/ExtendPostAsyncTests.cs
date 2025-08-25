using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Moq;
//using Moq.EntityFrameworkCore; // <-- quan trọng: để dùng ReturnsDbSet
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq.EntityFrameworkCore;
using static RealEstateManagement.Data.Entity.PropertyEntity.PropertyPost;

namespace RealEstateManagement.UnitTests.OwnerTest.OwnerPropertyTest
{
    [TestClass]
    public class ExtendPostAsyncTests : OwnerPropertyTestBase
    {
        [TestMethod]
        public async Task ReturnsFalse_WhenPostNotFound()
        {
            // Arrange
            var posts = new List<PropertyPost>(); // IEnumerable là đủ

            Db.Setup(d => d.Set<PropertyPost>())
              .ReturnsDbSet(posts);               // <-- extension từ Moq.EntityFrameworkCore

            // Act
            var (ok, msg) = await Svc.ExtendPostAsync(1, 10, 9);

            // Assert
            Assert.IsFalse(ok);
            StringAssert.Contains(msg, "not found");
            Db.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
        [TestMethod]
        public async Task ReturnsTrue_AndUpdatesArchiveDate_WhenPostFound_AndAllowedStatus()
        {
            // Arrange
            // Dùng status PENDING (được phép extend theo code hiện tại)
            var baseDateUtc = new DateTime(2025, 08, 01, 0, 0, 0, DateTimeKind.Utc);
            var post = new PropertyPost
            {
                Id = 10,
                LandlordId = 1,
                Status = PropertyPostStatus.Pending,
                ArchiveDate = baseDateUtc
            };
            var posts = new List<PropertyPost> { post };

            Db.Setup(d => d.Set<PropertyPost>()).ReturnsDbSet(posts);
            Db.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var extendDays = 15;
            var expectedArchiveDate = baseDateUtc.AddDays(extendDays);
            var expectedDateStr = expectedArchiveDate.ToString("dd/MM/yyyy");

            // Act
            var (ok, msg) = await Svc.ExtendPostAsync(postId: 10, days: extendDays, landlordId: 1);

            // Assert
            Assert.IsTrue(ok);
            StringAssert.Contains(msg, expectedDateStr);
            Assert.AreEqual(expectedArchiveDate, post.ArchiveDate);
            Assert.IsTrue(post.UpdatedAt.HasValue);
            Db.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task Throws_When_SaveChangesFails()
        {
            // Arrange
            var post = new PropertyPost
            {
                Id = 10,
                LandlordId = 1,
                Status = PropertyPostStatus.Pending
            };
            var posts = new List<PropertyPost> { post };

            Db.Setup(d => d.Set<PropertyPost>()).ReturnsDbSet(posts);
            Db.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()))
              .ThrowsAsync(new InvalidOperationException("DB err"));

            // Act + Assert
            var ex = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => Svc.ExtendPostAsync(postId: 10, days: 7, landlordId: 1));

            Assert.AreEqual("DB err", ex.Message);
            Db.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        [TestMethod]
        public async Task Wrongly_ExpectTrue_WhenDaysInvalid()
        {
            // Arrange
            var post = new PropertyPost
            {
                Id = 10,
                LandlordId = 1,
                Status = PropertyPost.PropertyPostStatus.Pending
            };
            var posts = new List<PropertyPost> { post };

            Db.Setup(d => d.Set<PropertyPost>()).ReturnsDbSet(posts);
            Db.Setup(d => d.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var (ok, msg) = await Svc.ExtendPostAsync(postId: 10, days: 0, landlordId: 1); // days=0 là invalid

            Assert.IsTrue(ok); 
        }


    }

}
