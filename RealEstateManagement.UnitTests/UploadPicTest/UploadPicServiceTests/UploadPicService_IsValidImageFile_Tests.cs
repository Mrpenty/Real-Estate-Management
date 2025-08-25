// File: UploadPicService_IsValidImageFile_Tests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;
using Moq;
using RealEstateManagement.Business.Services.UploadPicService;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;

namespace RealEstateManagement.UnitTests.UploadPicTest.UploadPicServiceTests
{
    [TestClass]
    public class UploadPicService_IsValidImageFile_Tests
    {
        private UploadPicService _svc;

        [TestInitialize]
        public void Setup()
        {
            var logger = new Mock<ILogger<UploadPicService>>();
            _svc = new UploadPicService(logger.Object);
        }

        private static IFormFile MakeFormFile(byte[] bytes, string fileName)
        {
            var ms = new MemoryStream(bytes);
            return new FormFile(ms, 0, ms.Length, "file", fileName);
        }

        [TestMethod]
        public void True_For_Png_Jpg_Jpeg_Gif_Under_5MB()
        {
            var bytes = Encoding.UTF8.GetBytes("img");
            Assert.IsTrue(_svc.IsValidImageFile(MakeFormFile(bytes, "a.png")));
            Assert.IsTrue(_svc.IsValidImageFile(MakeFormFile(bytes, "b.jpg")));
            Assert.IsTrue(_svc.IsValidImageFile(MakeFormFile(bytes, "c.jpeg")));
            Assert.IsTrue(_svc.IsValidImageFile(MakeFormFile(bytes, "d.gif")));
        }

        [TestMethod]
        public void False_For_Invalid_Extension()
        {
            var bytes = Encoding.UTF8.GetBytes("img");
            Assert.IsFalse(_svc.IsValidImageFile(MakeFormFile(bytes, "doc.txt")));
            Assert.IsFalse(_svc.IsValidImageFile(MakeFormFile(bytes, "script.exe")));
        }

        [TestMethod]
        public void False_For_Zero_Length()
        {
            var zero = new byte[0];
            Assert.IsFalse(_svc.IsValidImageFile(MakeFormFile(zero, "empty.png")));
        }

        [TestMethod]
        public void False_When_Over_5MB_Default_Limit()
        {
            var big = new byte[5 * 1024 * 1024 + 1];
            Assert.IsFalse(_svc.IsValidImageFile(MakeFormFile(big, "big.jpeg")));
        }

        [TestMethod]
        public void True_When_Exactly_5MB()
        {
            var edge = new byte[5 * 1024 * 1024];
            Assert.IsTrue(_svc.IsValidImageFile(MakeFormFile(edge, "edge.jpg")));
        }

        [TestMethod]
        public void False_When_Null_File()
        {
            Assert.IsFalse(_svc.IsValidImageFile(null));
        }

        [TestMethod]
        public void True_For_Uppercase_Extensions()
        {
            var bytes = Encoding.UTF8.GetBytes("img");
            Assert.IsTrue(_svc.IsValidImageFile(MakeFormFile(bytes, "UP.PNG")));
            Assert.IsTrue(_svc.IsValidImageFile(MakeFormFile(bytes, "UP.JPG")));
            Assert.IsTrue(_svc.IsValidImageFile(MakeFormFile(bytes, "UP.JPEG")));
            Assert.IsTrue(_svc.IsValidImageFile(MakeFormFile(bytes, "UP.GIF")));
        }

        [TestMethod]
        public void False_When_Over_Custom_MaxSize()
        {
            var bytes = new byte[10]; // 10 bytes
            Assert.IsFalse(_svc.IsValidImageFile(MakeFormFile(bytes, "tiny.png"), maxSizeInBytes: 5));
        }
    }
}
