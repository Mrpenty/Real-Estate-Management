using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Data.Entity.AddressEnity;
using System;
using System.Threading.Tasks;

namespace RealEstateManagement.UnitTests.OwnerTest.PropertyPostServiceTest
{
    [TestClass]
    public class CreatePropertyPostAsyncTests : PropertyPostTestBase
    {
        [TestMethod]
        public async Task Throws_When_Invalid_Input()
        {
            var dto = new PropertyCreateRequestDto
            {
                Title = "",
                Area = 0,
                Price = 0,
                ProvinceId = 0,
                WardId = 0,
                StreetId = 0
            };

            await Assert.ThrowsExceptionAsync<ArgumentException>(() => Svc.CreatePropertyPostAsync(dto, 1));
            AddressRepo.VerifyNoOtherCalls();
            PostRepo.VerifyNoOtherCalls();
            ImageRepo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Creates_Address_Property_Post_And_Returns_PropertyId()
        {
            var dto = new PropertyCreateRequestDto
            {
                Title = "Title",
                Description = "Desc",
                Type = "Apartment",
                Area = 50,
                Bedrooms = 2,
                Price = 1000,
                ProvinceId = 1,
                WardId = 2,
                StreetId = 3,
                DetailedAddress = "No.1",
                Location = "HCMC",
                AmenityIds = new System.Collections.Generic.List<int> { 5, 6 }
            };

            // Giữ lại instance Address để mô phỏng DB set Id
            Address capturedAddress = null!;

            // Khi AddAsync được gọi, gán Id cho address như DB làm
            AddressRepo.Setup(r => r.AddAsync(It.IsAny<Address>()))
                       .Callback<Address>(a =>
                       {
                           capturedAddress = a;
                           capturedAddress.Id = 456; // giả lập DB tạo Id
                       })
                       .Returns(Task.CompletedTask);

            // SaveChangesAsync của AddressRepo trả về Task<int> -> dùng ReturnsAsync(1)
            // Gọi 2 lần (sau Add và sau khi set PropertyId) đều OK
            AddressRepo.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

            // PostRepo.CreatePropertyPostAsync -> trả về propertyId mới
            PostRepo.Setup(r => r.CreatePropertyPostAsync(
                It.IsAny<RealEstateManagement.Data.Entity.PropertyEntity.Property>(),
                It.IsAny<RealEstateManagement.Data.Entity.PropertyEntity.PropertyPost>(),
                It.IsAny<System.Collections.Generic.List<int>>()
            )).ReturnsAsync(123);

            var propertyId = await Svc.CreatePropertyPostAsync(dto, landlordId: 77);

            Assert.AreEqual(123, propertyId);
            // Đảm bảo AddAsync đã được gọi và address đã có Id
            AddressRepo.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Once);
            Assert.IsNotNull(capturedAddress);
            Assert.AreEqual(456, capturedAddress.Id);

            // SaveChangesAsync được gọi 2 lần
            AddressRepo.Verify(r => r.SaveChangesAsync(), Times.Exactly(2));

            PostRepo.Verify(r => r.CreatePropertyPostAsync(
                It.IsAny<RealEstateManagement.Data.Entity.PropertyEntity.Property>(),
                It.IsAny<RealEstateManagement.Data.Entity.PropertyEntity.PropertyPost>(),
                It.Is<System.Collections.Generic.List<int>>(ids => ids.Count == 2 && ids[0] == 5 && ids[1] == 6)
            ), Times.Once);

            PostRepo.VerifyNoOtherCalls();
            ImageRepo.VerifyNoOtherCalls();
        }

        [TestMethod]
        public async Task Throws_When_Dto_Is_Null()
        {
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => Svc.CreatePropertyPostAsync(null!, 1));
            AddressRepo.VerifyNoOtherCalls();
            PostRepo.VerifyNoOtherCalls();
            ImageRepo.VerifyNoOtherCalls();
        }
    }
}
