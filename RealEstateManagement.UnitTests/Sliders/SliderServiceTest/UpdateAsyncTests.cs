using Microsoft.AspNetCore.Http;
using Moq;
using RealEstateManagement.Business.DTO.SliderDTO;
using RealEstateManagement.Business.Services.UploadPicService;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SliderEntity = RealEstateManagement.Data.Entity.Slider;
namespace RealEstateManagement.UnitTests.Sliders.SliderServiceTest
{
    [TestClass]
    public class UpdateAsyncTests
    {
        private Mock<ISliderRepository> _repo;
        private Mock<IUploadPicService> _upload;
        private SliderService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<ISliderRepository>();
            _upload = new Mock<IUploadPicService>(); // loose
            _service = new SliderService(_repo.Object, _upload.Object);
        }

        [TestMethod]
        public async Task Throw_When_Dto_Null()
        {
            await Assert.ThrowsExceptionAsync<System.ArgumentNullException>(async () =>
                await _service.UpdateAsync(null, null));
        }

        [TestMethod]
        public async Task Throw_When_Slider_NotFound()
        {
            _repo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync((SliderEntity)null);

            var dto = new UpdateSliderDto { Id = 7, Title = "T", Description = "D" };

            await Assert.ThrowsExceptionAsync<KeyNotFoundException>(async () =>
                await _service.UpdateAsync(dto, null));

            _repo.Verify(r => r.GetByIdAsync(7), Times.Once);
            _repo.Verify(r => r.UpdateAsync(It.IsAny<SliderEntity>()), Times.Never);
            //_upload.Verify(u => u.IsValidImageFile(It.IsAny<IFormFile>()), Times.Never);
            _upload.Verify(u => u.DeleteImageAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task Update_Title_Desc_Without_Image()
        {
            var existing = new SliderEntity { Id = 8, Title = "Old", Description = "OldDesc", ImageUrl = "http://old" };
            _repo.Setup(r => r.GetByIdAsync(8)).ReturnsAsync(existing);
            _repo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            var dto = new UpdateSliderDto { Id = 8, Title = "NewTitle", Description = "NewDesc" };

            await _service.UpdateAsync(dto, imageFile: null);

            Assert.AreEqual("NewTitle", existing.Title);
            Assert.AreEqual("NewDesc", existing.Description);
            Assert.AreEqual("http://old", existing.ImageUrl); // không đụng ảnh

            //_upload.Verify(u => u.IsValidImageFile(It.IsAny<IFormFile>()), Times.Never);
            _upload.Verify(u => u.DeleteImageAsync(It.IsAny<string>()), Times.Never);
            _repo.Verify(r => r.UpdateAsync(existing), Times.Once);
        }

        // ----------- (1) NEW: image != null nhưng invalid -> không upload, không xóa, giữ nguyên URL ----------
        [TestMethod]
        public async Task With_Image_But_Invalid_File_Do_Not_Upload_Do_Not_Delete_Keep_Old_ImageUrl()
        {
            var existing = new SliderEntity { Id = 9, Title = "OldT", Description = "OldD", ImageUrl = "http://old" };
            _repo.Setup(r => r.GetByIdAsync(9)).ReturnsAsync(existing);
            _repo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            var dto = new UpdateSliderDto { Id = 9, Title = "NewT", Description = "NewD" };
            var imageMock = new Mock<IFormFile>();
            var image = imageMock.Object;

            //_upload.Setup(u => u.IsValidImageFile(image)).Returns(false);

            await _service.UpdateAsync(dto, image);

            Assert.AreEqual("NewT", existing.Title);
            Assert.AreEqual("NewD", existing.Description);
            Assert.AreEqual("http://old", existing.ImageUrl); // giữ URL cũ

            //_upload.Verify(u => u.IsValidImageFile(image), Times.Once);
            _upload.Verify(u => u.DeleteImageAsync(It.IsAny<string>()), Times.Never);
            _repo.Verify(r => r.UpdateAsync(existing), Times.Once);
        }



        // ----------- (3) NEW: image != null và valid nhưng không có ảnh cũ -> không xóa, thử upload, URL vẫn null nếu upload null ----------
        [TestMethod]
        public async Task With_Valid_Image_No_Old_Image_Do_Not_Delete_Keep_Null_Url_When_Upload_Null()
        {
            var existing = new SliderEntity { Id = 11, Title = "OldT", Description = "OldD", ImageUrl = null };
            _repo.Setup(r => r.GetByIdAsync(11)).ReturnsAsync(existing);
            _repo.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);

            var dto = new UpdateSliderDto { Id = 11, Title = "NewT", Description = "NewD" };
            var imageMock = new Mock<IFormFile>();
            var image = imageMock.Object;

            //_upload.Setup(u => u.IsValidImageFile(image)).Returns(true);

            await _service.UpdateAsync(dto, image);

            Assert.AreEqual("NewT", existing.Title);
            Assert.AreEqual("NewD", existing.Description);
            Assert.IsTrue(string.IsNullOrEmpty(existing.ImageUrl)); // vẫn null/empty

            //_upload.Verify(u => u.IsValidImageFile(image), Times.AtLeastOnce());
            _upload.Verify(u => u.DeleteImageAsync(It.IsAny<string>()), Times.Never);
            _repo.Verify(r => r.UpdateAsync(existing), Times.Once);
        }
    }
}
