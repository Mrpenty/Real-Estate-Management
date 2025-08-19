using Moq;
using RealEstateManagement.Business.Services.UploadPicService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliderEntity = RealEstateManagement.Data.Entity.Slider;
namespace RealEstateManagement.UnitTests.Sliders.SliderServiceTest
{
    [TestClass]
    public class DeleteAsyncTests
    {
        private Mock<ISliderRepository> _repo;
        private Mock<IUploadPicService> _upload;
        private SliderService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<ISliderRepository>();
            _upload = new Mock<IUploadPicService>();
            _service = new SliderService(_repo.Object, _upload.Object);
        }

        [TestMethod]
        public async Task Throw_When_NotFound()
        {
            // Arrange
            _repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync((SliderEntity)null);

            // Act + Assert
            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
                await _service.DeleteAsync(5));

            _repo.Verify(r => r.GetByIdAsync(5), Times.Once);
            _upload.Verify(u => u.DeleteImageAsync(It.IsAny<string>()), Times.Never);
            _repo.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task Delete_With_ImageUrl_Calls_DeleteImage_Then_DeleteRepo()
        {
            // Arrange
            var s = new SliderEntity { Id = 6, ImageUrl = "http://cdn/img.png" };
            _repo.Setup(r => r.GetByIdAsync(6)).ReturnsAsync(s);
            _upload.Setup(u => u.DeleteImageAsync("http://cdn/img.png")).ReturnsAsync(true);
            _repo.Setup(r => r.DeleteAsync(6)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(6);

            // Assert
            _repo.Verify(r => r.GetByIdAsync(6), Times.Once);
            _upload.Verify(u => u.DeleteImageAsync("http://cdn/img.png"), Times.Once);
            _repo.Verify(r => r.DeleteAsync(6), Times.Once);
        }

        [TestMethod]
        public async Task Delete_No_ImageUrl_Skips_DeleteImage()
        {
            // Arrange
            var s = new SliderEntity { Id = 7, ImageUrl = "" };
            _repo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(s);
            _repo.Setup(r => r.DeleteAsync(7)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(7);

            // Assert
            _upload.Verify(u => u.DeleteImageAsync(It.IsAny<string>()), Times.Never);
            _repo.Verify(r => r.DeleteAsync(7), Times.Once);
        }
    }
}
