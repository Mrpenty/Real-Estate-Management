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
    }

}
