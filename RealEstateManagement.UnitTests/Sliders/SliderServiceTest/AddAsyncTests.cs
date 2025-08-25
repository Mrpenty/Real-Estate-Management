using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.SliderDTO;
using RealEstateManagement.Business.Repositories; // ISliderRepository
using RealEstateManagement.Business.Services.UploadPicService;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

// alias cho đúng kiểu đầy đủ như bạn yêu cầu
using SliderEntity = RealEstateManagement.Data.Entity.Slider;

namespace RealEstateManagement.UnitTests.Sliders.SliderServiceTest
{
    [TestClass]
    public class SliderService_AddAsync_Tests
    {
        private Mock<ISliderRepository> _repo;
        private Mock<IUploadPicService> _upload;
        private SliderService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<ISliderRepository>();            // Loose: không cần setup upload
            _upload = new Mock<IUploadPicService>();          // Không setup/verify gì ở upload
            _service = new SliderService(_repo.Object, _upload.Object);
        }

        [TestMethod]
        public async Task Throw_When_Dto_Null()
        {
            await Assert.ThrowsExceptionAsync<System.ArgumentNullException>(async () =>
                await _service.AddAsync(null, null));
        }

        [TestMethod]
        public async Task Add_Slider_Without_Image_When_Image_Null()
        {
            // Arrange
            var dto = new CreateSliderDto { Title = "New", Description = "Desc" };
            IFormFile image = null;

            SliderEntity captured = null;
            _repo.Setup(r => r.AddAsync(It.IsAny<SliderEntity>()))
                 .Callback<SliderEntity>(s => captured = s)
                 .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AddAsync(dto, image);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(captured);
            Assert.AreEqual(dto.Title, captured.Title);
            Assert.AreEqual(dto.Description, captured.Description);
            Assert.IsTrue(captured.CreatedAt != default);
            Assert.IsTrue(string.IsNullOrEmpty(captured.ImageUrl));

            _repo.Verify(r => r.AddAsync(It.IsAny<SliderEntity>()), Times.Once);

        }
    }
}
