using Microsoft.Extensions.Logging;
using Moq;
using RealEstateManagement.Business.Services.UploadPicService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.UploadPicTest.UploadPicServiceTests
{
    [TestClass]
    public class UploadPicService_DeleteImageAsync_Tests
    {
        private UploadPicService _svc;
        private Mock<ILogger<UploadPicService>> _logger;
        private string _oldCwd;
        private string _tempRoot;

        [TestInitialize]
        public void Setup()
        {
            _logger = new Mock<ILogger<UploadPicService>>();
            _svc = new UploadPicService(_logger.Object);

            _oldCwd = Directory.GetCurrentDirectory();
            _tempRoot = Path.Combine(Path.GetTempPath(), "UploadPicService_Delete_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempRoot);
            Directory.SetCurrentDirectory(_tempRoot);
        }

        [TestCleanup]
        public void Cleanup()
        {
            try
            {
                Directory.SetCurrentDirectory(_oldCwd);
                if (Directory.Exists(_tempRoot))
                    Directory.Delete(_tempRoot, true);
            }
            catch { }
        }

        [TestMethod]
        public async Task Delete_Returns_False_When_Url_Null_Or_Empty()
        {
            Assert.IsFalse(await _svc.DeleteImageAsync(null));
            Assert.IsFalse(await _svc.DeleteImageAsync(""));
        }

        [TestMethod]
        public async Task Delete_Returns_True_And_Removes_File_When_File_Exists()
        {
            var wwwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var folder = Path.Combine(wwwroot, "uploads", "sliders");
            Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, "to-delete.png");
            await File.WriteAllBytesAsync(filePath, new byte[] { 1, 2, 3 });
            var imageUrl = "/uploads/sliders/to-delete.png";

            var ok = await _svc.DeleteImageAsync(imageUrl);

            Assert.IsTrue(ok);
            Assert.IsFalse(File.Exists(filePath));
        }

        [TestMethod]
        public async Task Delete_Returns_False_When_File_Not_Found()
        {
            var imageUrl = "/uploads/sliders/not-exist.png";
            var ok = await _svc.DeleteImageAsync(imageUrl);
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public async Task Delete_Returns_False_When_Whitespace_Path()
        {
            var ok = await _svc.DeleteImageAsync("   ");
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public async Task Delete_Returns_False_When_Wwwroot_Not_Exists()
        {
            var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (Directory.Exists(root)) Directory.Delete(root, true);

            var ok = await _svc.DeleteImageAsync("/uploads/sliders/anything.png");
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public async Task Delete_Returns_False_For_Sneaky_Relative_Path()
        {
            var ok = await _svc.DeleteImageAsync("../uploads/sliders/escape.png");
            Assert.IsFalse(ok);
        }

        [TestMethod]
        public async Task Delete_Returns_False_For_Path_With_Backslashes()
        {
            var ok = await _svc.DeleteImageAsync(@"\uploads\sliders\nope.gif");
            Assert.IsFalse(ok);
        }
    }
}
