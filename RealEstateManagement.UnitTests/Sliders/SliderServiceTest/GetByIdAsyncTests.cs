using Moq;
using RealEstateManagement.Business.Services.UploadPicService;
using RealEstateManagement.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Sliders.SliderServiceTest
{
    [TestClass]
    public class GetByIdAsyncTests
    {
        private Mock<ISliderRepository> _repo = null!;
        private Mock<IUploadPicService> _upload = null!;
        private SliderService _svc = null!;

        [TestInitialize]
        public void Setup()
        {
            _repo = new Mock<ISliderRepository>(MockBehavior.Strict);
            _upload = new Mock<IUploadPicService>(MockBehavior.Strict);
            _svc = new SliderService(_repo.Object, _upload.Object);
        }

        [TestMethod]
        public async Task Returns_Slider_When_Exists()
        {
            var s = new Data.Entity.Slider { Id = 7, Title = "t", Description = "d", ImageUrl = "u" };
            _repo.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(s);

            var result = await _svc.GetByIdAsync(7);

            Assert.IsNotNull(result);
            Assert.AreEqual(7, result.Id);
            _repo.Verify(r => r.GetByIdAsync(7), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task Throws_When_NotFound()
        {
            _repo.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Data.Entity.Slider)null!);

            _ = await _svc.GetByIdAsync(999);
        }
    }
}
