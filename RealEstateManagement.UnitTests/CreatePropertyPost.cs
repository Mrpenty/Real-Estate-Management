//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
//using RealEstateManagement.Business.Repositories.AddressRepo;
//using RealEstateManagement.Business.Repositories.OwnerRepo;
//using RealEstateManagement.Business.Services.OwnerService;
//using RealEstateManagement.Data.Entity.AddressEnity;
//using RealEstateManagement.Data.Entity.PropertyEntity;
//using RealEstateManagement.Data.Entity.User;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//[TestClass]
//public class CreatePropertyPost
//{
//    private Mock<IPropertyPostRepository> _mockPostRepository;
//    private Mock<IAddressRepository> _mockAddressRepository;
//    private Mock<IPropertyImageRepository> _mockImageRepository;
//    private Mock<RentalDbContext> _mockContext;
//    private PropertyPostService _propertyPostService;

//    [TestInitialize]
//    public void Setup()
//    {
//        _mockPostRepository = new Mock<IPropertyPostRepository>();
//        _mockAddressRepository = new Mock<IAddressRepository>();
//        _mockImageRepository = new Mock<IPropertyImageRepository>();
//        _mockContext = new Mock<RentalDbContext>();
//        _propertyPostService = new PropertyPostService(
//            _mockPostRepository.Object,
//            _mockAddressRepository.Object,
//            _mockImageRepository.Object,
//            _mockContext.Object
//        );
//    }

//    [TestMethod]
//    public async Task CreatePropertyPostAsync_WithValidData_ReturnsNewPropertyId()
//    {
//        // Arrange
//        var landlordId = 1;
//        var dto = new PropertyCreateRequestDto
//        {
//            Title = "Test Title",
//            Description = "Test Description",
//            Area = 100,
//            Price = 5000000,
//            Type = "Apartment",
//            Bedrooms = 2,
//            ProvinceId = 1,
//            WardId = 2,
//            StreetId = 3,
//            DetailedAddress = "123 Test St",
//            Location = "Test Location",
//            AmenityIds = new List<int> { 1, 2 }
//        };

//        _mockAddressRepository.Setup(r => r.AddAsync(It.IsAny<Address>()))
//            .Callback<Address>(a => a.Id = 101); // Mock Address ID to be set
//        _mockAddressRepository.Setup(r => r.SaveChangesAsync())
//            .Returns((Task<int>)Task.CompletedTask);
//        _mockPostRepository.Setup(r => r.CreatePropertyPostAsync(
//                It.IsAny<Property>(),
//                It.IsAny<PropertyPost>(),
//                It.IsAny<List<int>>()))
//            .ReturnsAsync(201); // Mock Property ID to be returned

//        // Act
//        var result = await _propertyPostService.CreatePropertyPostAsync(dto, landlordId);

//        // Assert
//        Assert.AreEqual(201, result);
//        _mockAddressRepository.Verify(r => r.AddAsync(It.Is<Address>(a => a.PropertyId == null)), Times.Once);
//        _mockAddressRepository.Verify(r => r.SaveChangesAsync(), Times.Exactly(2));
//        _mockPostRepository.Verify(r => r.CreatePropertyPostAsync(
//            It.Is<Property>(p => p.Title == dto.Title && p.LandlordId == landlordId && p.AddressId == 101 && p.Status == "active"),
//            It.Is<PropertyPost>(p => p.LandlordId == landlordId && p.Status == PropertyPost.PropertyPostStatus.Draft),
//            It.Is<List<int>>(ids => ids.SequenceEqual(dto.AmenityIds))
//        ), Times.Once);
//    }

//    [TestMethod]
//    public async Task CreatePropertyPostAsync_WithInvalidTitle_ThrowsArgumentException()
//    {
//        // Arrange
//        var dto = new PropertyCreateRequestDto { Title = "", Area = 1, Price = 1, ProvinceId = 1, WardId = 1, StreetId = 1 };

//        // Act & Assert
//        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _propertyPostService.CreatePropertyPostAsync(dto, 1));
//        _mockAddressRepository.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Never);
//    }

//    [TestMethod]
//    public async Task CreatePropertyPostAsync_WithInvalidAreaAndPrice_ThrowsArgumentException()
//    {
//        // Arrange
//        var dto = new PropertyCreateRequestDto { Title = "Valid Title", Area = 0, Price = -1, ProvinceId = 1, WardId = 1, StreetId = 1 };

//        // Act & Assert
//        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _propertyPostService.CreatePropertyPostAsync(dto, 1));
//        _mockAddressRepository.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Never);
//    }

//    [TestMethod]
//    public async Task CreatePropertyPostAsync_WithInvalidAddress_ThrowsArgumentException()
//    {
//        // Arrange
//        var dto = new PropertyCreateRequestDto { Title = "Valid Title", Area = 1, Price = 1, ProvinceId = 0, WardId = 1, StreetId = 1 };

//        // Act & Assert
//        await Assert.ThrowsExceptionAsync<ArgumentException>(() => _propertyPostService.CreatePropertyPostAsync(dto, 1));
//        _mockAddressRepository.Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Never);
//    }
//}
