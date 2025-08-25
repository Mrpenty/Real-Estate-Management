using Microsoft.AspNetCore.Http;
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
    public class UploadPicService_UploadImageAsync_Tests
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
            _tempRoot = Path.Combine(Path.GetTempPath(), "UploadPicServiceTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempRoot);
            Directory.SetCurrentDirectory(_tempRoot); // service dùng Directory.GetCurrentDirectory()
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
            catch { /* best effort */ }
        }

        private static IFormFile MakeFormFile(byte[] bytes, string fileName)
        {
            var ms = new MemoryStream(bytes);
            return new FormFile(ms, 0, ms.Length, "file", fileName);
        }

        // Helper: cố gắng lấy message nếu UploadResult có Error/ErrorMessage/Message
        private static string TryGetErrorMessage(object result)
        {
            if (result == null) return null;
            var t = result.GetType();
            var prop = t.GetProperty("Error") ?? t.GetProperty("ErrorMessage") ?? t.GetProperty("Message");
            return prop?.GetValue(result) as string;
        }

        [TestMethod]
        public async Task Upload_Success_Returns_ImageUrl_And_Saves_File()
        {
            // Arrange
            var content = Encoding.UTF8.GetBytes("hello image");
            var file = MakeFormFile(content, "photo.png");

            // Act
            var result = await _svc.UploadImageAsync(file, folderName: "sliders", prefix: "slider_home");

            // Assert (không cần result.Error)
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Succeeded);
            Assert.IsFalse(string.IsNullOrEmpty(result.ImageUrl));
            StringAssert.StartsWith(result.ImageUrl, "/uploads/sliders/");
            StringAssert.Contains(result.ImageUrl, "slider_home_");

            // File đã được lưu thật trong wwwroot
            var rel = result.ImageUrl.TrimStart('/');
            var full = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", rel);
            Assert.IsTrue(File.Exists(full));
        }

        [TestMethod]
        public async Task Upload_Fails_When_File_Null()
        {
            var result = await _svc.UploadImageAsync(null, "sliders");

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            Assert.IsTrue(string.IsNullOrEmpty(result.ImageUrl));

            // Nếu có message, kiểm tra nội dung
            var msg = TryGetErrorMessage(result);
            if (msg != null)
                StringAssert.Contains(msg, "No file", "Expected error message to mention 'No file'.");
        }

        [TestMethod]
        public async Task Upload_Fails_When_Invalid_Extension()
        {
            var file = MakeFormFile(Encoding.UTF8.GetBytes("data"), "note.txt");

            var result = await _svc.UploadImageAsync(file, "sliders");

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            Assert.IsTrue(string.IsNullOrEmpty(result.ImageUrl));

            var msg = TryGetErrorMessage(result);
            if (msg != null)
                StringAssert.Contains(msg, "Invalid file type", "Expected error message to mention invalid file type.");
        }

        [TestMethod]
        public async Task Upload_Fails_When_Too_Big()
        {
            // > 5MB
            var big = new byte[5 * 1024 * 1024 + 1];
            var file = MakeFormFile(big, "huge.jpg");

            var result = await _svc.UploadImageAsync(file, "sliders");

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            Assert.IsTrue(string.IsNullOrEmpty(result.ImageUrl));

            var msg = TryGetErrorMessage(result);
            if (msg != null)
                StringAssert.Contains(msg, "5MB", "Expected error message to mention 5MB limit.");
        }
    }
}
