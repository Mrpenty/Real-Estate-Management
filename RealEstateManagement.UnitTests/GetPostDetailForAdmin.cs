using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.AddressRepo;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity.AddressEnity;
using RealEstateManagement.Data.Entity.PropertyEntity;
using RealEstateManagement.Data.Entity.User;
using RealEstateManagement.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[TestClass]
public class GetPostDetailForAdmin
{
    private Mock<IPropertyPostRepository> _mockPostRepository;
    private Mock<IAddressRepository> _mockAddressRepository;
    private Mock<IPropertyImageRepository> _mockImageRepository;
    private Mock<RentalDbContext> _mockContext;
    private PropertyPostService _propertyPostService;

    [TestInitialize]
    public void Setup()
    {
        _mockPostRepository = new Mock<IPropertyPostRepository>();
        _mockAddressRepository = new Mock<IAddressRepository>();
        _mockImageRepository = new Mock<IPropertyImageRepository>();
        _mockContext = new Mock<RentalDbContext>();
        _propertyPostService = new PropertyPostService(
            _mockPostRepository.Object,
            _mockAddressRepository.Object,
            _mockImageRepository.Object,
            _mockContext.Object
        );
    }

    [TestMethod]
    public async Task GetPostDetailForAdminAsync_WithExistingPostId_ReturnsPostDetails()
    {
        // Arrange
        var postId = 1;
        var mockPost = new PropertyPost
        {
            Id = postId,
            LandlordId = 101,
            Status = PropertyPost.PropertyPostStatus.Approved,
            Landlord = new ApplicationUser { Id = 101, Name = "John Doe" },
            Property = new Property
            {
                Id = 201,
                Title = "Test Property",
                Description = "A nice apartment.",
                Address = new Address
                {
                    Id = 301,
                    DetailedAddress = "123 Main St",
                    Province = new Province { Id = 1, Name = "Province A" },
                    Ward = new Ward { Id = 2, Name = "Ward B" },
                    Street = new Street { Id = 3, Name = "Street C" }
                },
                Images = new List<PropertyImage>
                {
                    new PropertyImage { Id = 401, Url = "url1.jpg", IsPrimary = true },
                    new PropertyImage { Id = 402, Url = "url2.jpg", IsPrimary = false }
                },
                PropertyAmenities = new List<PropertyAmenity>
                {
                    new PropertyAmenity { Amenity = new Amenity { Id = 501, Name = "Pool" } },
                    new PropertyAmenity { Amenity = new Amenity { Id = 502, Name = "Gym" } }
                }
            }
        };

        var mockPosts = new List<PropertyPost> { mockPost }.AsQueryable();
        var mockDbSet = new Mock<DbSet<PropertyPost>>();
        mockDbSet.As<IQueryable<PropertyPost>>().Setup(m => m.Provider).Returns(mockPosts.Provider);
        mockDbSet.As<IQueryable<PropertyPost>>().Setup(m => m.Expression).Returns(mockPosts.Expression);
        mockDbSet.As<IQueryable<PropertyPost>>().Setup(m => m.ElementType).Returns(mockPosts.ElementType);
        mockDbSet.As<IQueryable<PropertyPost>>().Setup(m => m.GetEnumerator()).Returns(mockPosts.GetEnumerator());

        _mockContext.Setup(c => c.PropertyPosts).Returns(mockDbSet.Object);
        _mockPostRepository.Setup(r => r.GetAll()).Returns(mockDbSet.Object);

        // Act
        dynamic result = await _propertyPostService.GetPostDetailForAdminAsync(postId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(postId, result.Id);
        Assert.AreEqual("Đã duyệt", result.StatusDisplay);
        Assert.AreEqual("John Doe", result.Landlord.Name);
        Assert.AreEqual("Test Property", result.Property.Title);
        Assert.AreEqual("123 Main St", result.Property.Address.DetailedAddress);
        Assert.AreEqual("Province A", result.Property.Address.Province.Name);
        Assert.AreEqual(2, result.Property.Images.Count);
        Assert.AreEqual(2, result.Property.Amenities.Count);
    }

    [TestMethod]
    public async Task GetPostDetailForAdminAsync_WithNonExistingPostId_ReturnsNull()
    {
        // Arrange
        var postId = 999;
        var mockPosts = new List<PropertyPost>().AsQueryable();
        var mockDbSet = new Mock<DbSet<PropertyPost>>();
        mockDbSet.As<IQueryable<PropertyPost>>().Setup(m => m.Provider).Returns(mockPosts.Provider);
        mockDbSet.As<IQueryable<PropertyPost>>().Setup(m => m.Expression).Returns(mockPosts.Expression);
        mockDbSet.As<IQueryable<PropertyPost>>().Setup(m => m.ElementType).Returns(mockPosts.ElementType);
        mockDbSet.As<IQueryable<PropertyPost>>().Setup(m => m.GetEnumerator()).Returns(mockPosts.GetEnumerator());

        _mockContext.Setup(c => c.PropertyPosts).Returns(mockDbSet.Object);
        _mockPostRepository.Setup(r => r.GetAll()).Returns(mockDbSet.Object);

        // Act
        var result = await _propertyPostService.GetPostDetailForAdminAsync(postId);

        // Assert
        Assert.IsNull(result);
    }
}
