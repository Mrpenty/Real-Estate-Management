using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static RealEstateManagement.Data.Entity.PropertyEntity.PropertyPost;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyPostServiceTest
{
    [TestClass]
    public class ContinueDraftPostAsync_Tests : PropertyPostTestBase
    {
        private Property MakeProperty() => new Property { Id = 10, Address = new RealEstateManagement.Data.Entity.AddressEnity.Address() };

        [TestMethod]
        public async Task Adds_New_Images_Only_And_Sets_Pending()
        {
            var post = new PropertyPost { Id = 5, Status = PropertyPostStatus.Draft, Property = MakeProperty() };
            PostRepo.Setup(r => r.GetPropertyPostByIdAsync(5, 77)).ReturnsAsync(post);

            AddressRepo.Setup(r => r.UpdateAsync(It.IsAny<RealEstateManagement.Data.Entity.AddressEnity.Address>()))
                       .Returns(Task.CompletedTask);

            PostRepo.Setup(r => r.UpdatePropertyAmenities(10, It.IsAny<List<int>>()))
                    .Returns(Task.CompletedTask);

            ImageRepo.Setup(r => r.AddImageAsync(It.IsAny<PropertyImage>()))
                     .ReturnsAsync(new PropertyImage { Id = 1 });

            ImageRepo.Setup(r => r.HasAnyImage(10)).ReturnsAsync(true);

            PostRepo.Setup(r => r.UpdateAsync(post)).Returns(Task.CompletedTask);

            var dto = new ContinuePropertyPostDto
            {
                PostId = 5,
                Title = "T",
                Description = "D",
                Type = "A",
                Area = 50,
                Bedrooms = 2,
                Price = 1000,
                Location = "HCMC",
                ProvinceId = 1,
                WardId = 2,
                StreetId = 3,
                DetailedAddress = "No.1",
                AmenityIds = new List<int> { 1 },
                Images = new List<PropertyImageCreateDto>
                {
                    new PropertyImageCreateDto{ Id=0, Url="new1", IsPrimary=true, Order=1 },
                    new PropertyImageCreateDto{ Id=0, Url="new2", IsPrimary=false, Order=2 },
                }
            };

            await Svc.ContinueDraftPostAsync(dto, 77);

            Assert.AreEqual(PropertyPostStatus.Pending, post.Status);
            Assert.IsTrue(post.UpdatedAt.HasValue);

            ImageRepo.Verify(r => r.AddImageAsync(It.Is<PropertyImage>(x => x.Url == "new1")), Times.Once);
            ImageRepo.Verify(r => r.AddImageAsync(It.Is<PropertyImage>(x => x.Url == "new2")), Times.Once);
            ImageRepo.Verify(r => r.UpdateImageAsync(It.IsAny<PropertyImage>()), Times.Never);

            PostRepo.Verify(r => r.UpdateAsync(post), Times.Once);
        }

        [TestMethod]
        public async Task No_Images_Does_Not_Set_Pending()
        {
            var post = new PropertyPost { Id = 6, Status = PropertyPostStatus.Draft, Property = MakeProperty() };
            PostRepo.Setup(r => r.GetPropertyPostByIdAsync(6, 88)).ReturnsAsync(post);

            AddressRepo.Setup(r => r.UpdateAsync(It.IsAny<RealEstateManagement.Data.Entity.AddressEnity.Address>()))
                       .Returns(Task.CompletedTask);
            PostRepo.Setup(r => r.UpdatePropertyAmenities(10, It.IsAny<List<int>>()))
                    .Returns(Task.CompletedTask);

            // Không có ảnh
            ImageRepo.Setup(r => r.HasAnyImage(10)).ReturnsAsync(false);

            var dto = new ContinuePropertyPostDto
            {
                PostId = 6,
                Title = "Draft",
                Images = new List<PropertyImageCreateDto>() // empty
            };

            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => Svc.ContinueDraftPostAsync(dto, 88));
            Assert.AreEqual("Phải có ít nhất 1 ảnh.", ex.Message);

            PostRepo.Verify(r => r.UpdateAsync(It.IsAny<PropertyPost>()), Times.Never);
        }


        [TestMethod]
        public async Task Updates_Existing_Images_When_Id_Present()
        {
            var post = new PropertyPost { Id = 7, Status = PropertyPostStatus.Draft, Property = MakeProperty() };
            PostRepo.Setup(r => r.GetPropertyPostByIdAsync(7, 99)).ReturnsAsync(post);

            AddressRepo.Setup(r => r.UpdateAsync(It.IsAny<RealEstateManagement.Data.Entity.AddressEnity.Address>()))
                       .Returns(Task.CompletedTask);

            ImageRepo.Setup(r => r.HasAnyImage(10)).ReturnsAsync(true);
            ImageRepo.Setup(r => r.UpdateImageAsync(It.IsAny<PropertyImage>()))
         .ReturnsAsync((PropertyImage img) => img); // 🔥 thêm dòng này
            PostRepo.Setup(r => r.UpdateAsync(post)).Returns(Task.CompletedTask);
            PostRepo.Setup(r => r.UpdatePropertyAmenities(10, It.IsAny<List<int>>()))
                    .Returns(Task.CompletedTask);

            var dto = new ContinuePropertyPostDto
            {
                PostId = 7,
                Title = "With Existing Images",
                Images = new List<PropertyImageCreateDto>
        {
            new PropertyImageCreateDto{ Id=100, Url="old1", IsPrimary=true, Order=1 },
            new PropertyImageCreateDto{ Id=101, Url="old2", IsPrimary=false, Order=2 }
        }
            };

            await Svc.ContinueDraftPostAsync(dto, 99);

            Assert.AreEqual(PropertyPostStatus.Pending, post.Status);

            ImageRepo.Verify(r => r.UpdateImageAsync(It.Is<PropertyImage>(x => x.Id == 100 && x.Url == "old1")), Times.Once);
            ImageRepo.Verify(r => r.UpdateImageAsync(It.Is<PropertyImage>(x => x.Id == 101 && x.Url == "old2")), Times.Once);
            ImageRepo.Verify(r => r.AddImageAsync(It.IsAny<PropertyImage>()), Times.Never);
        }


    }
}
