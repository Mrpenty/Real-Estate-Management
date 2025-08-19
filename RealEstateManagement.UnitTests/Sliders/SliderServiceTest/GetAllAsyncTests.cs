using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.Sliders.SliderServiceTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using RealEstateManagement.Data.Entity;
    using RealEstateManagement.Business.Services.UploadPicService;

    [TestClass]
    public class GetAllAsyncTests
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
        public async Task Returns_All_From_Repository()
        {
            var data = new List<Slider>
        {
            new Slider{ Id = 1, Title = "s1", Description = "d1", ImageUrl = "u1" },
            new Slider{ Id = 2, Title = "s2", Description = "d2", ImageUrl = "u2" }
        };

            _repo.Setup(r => r.GetAsync()).ReturnsAsync(data);

            var result = await _svc.GetAllAsync();

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(data, new List<Slider>(result));
            _repo.Verify(r => r.GetAsync(), Times.Once);
        }
    }

}
