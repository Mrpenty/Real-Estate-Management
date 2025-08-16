using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.PropertyOwnerDTO;
using RealEstateManagement.Business.Repositories.OwnerRepo;
using RealEstateManagement.Business.Services.OwnerService;
using RealEstateManagement.Data.Entity.PropertyEntity;
using System;
using System.Threading.Tasks;
using RealEstateManagement.Data.Entity.User;

[TestClass]
public class UpdateProperty
{
    private Mock<IOwnerPropertyRepository> _mockOwnerPropertyRepo;
    private Mock<RentalDbContext> _mockRentalDbContext;
    private OwnerPropertyService _ownerPropertyService;

    [TestInitialize]
    public void Setup()
    {
        _mockOwnerPropertyRepo = new Mock<IOwnerPropertyRepository>();
        _mockRentalDbContext = new Mock<RentalDbContext>();
        _ownerPropertyService = new OwnerPropertyService(_mockOwnerPropertyRepo.Object, _mockRentalDbContext.Object);
    }

    [TestMethod]
    public async Task UpdatePropertyAsync_WithValidData_UpdatesPropertySuccessfully()
    {
        // Arrange
        var landlordId = 1;
        var propertyId = 101;
        var existingProperty = new Property
        {
            Id = propertyId,
            LandlordId = landlordId,
            Title = "Old Title",
            Description = "Old Description",
            Price = 1000000,
            Area = 50,
            Bedrooms = 1
        };
        var dto = new PropertyCreateRequestDto
        {
           // Id = propertyId,
            Title = "New Title",
            Description = "New Description",
            Price = 2000000,
            Area = 75,
            Bedrooms = 2
        };

        _mockOwnerPropertyRepo.Setup(r => r.GetByIdAsync(propertyId, landlordId))
                              .ReturnsAsync(existingProperty);
        _mockOwnerPropertyRepo.Setup(r => r.UpdateAsync(It.IsAny<Property>()))
                              .Returns(Task.CompletedTask);

        // Act
        await _ownerPropertyService.UpdatePropertyAsync(dto, landlordId, propertyId);

        // Assert
        _mockOwnerPropertyRepo.Verify(r => r.UpdateAsync(It.Is<Property>(p =>
            p.Id == propertyId &&
            p.Title == "New Title" &&
            p.Description == "New Description" &&
            p.Price == 2000000 &&
            p.Area == 75 &&
            p.Bedrooms == 2
        )), Times.Once);
    }

    [TestMethod]
    public async Task UpdatePropertyAsync_WithNonExistingPropertyId_ThrowsException()
    {
        // Arrange
        var landlordId = 1;
        var propertyId = 999;
        var dto = new PropertyCreateRequestDto
        {
           // Id = propertyId,
            Title = "New Title",
            Description = "New Description"
        };

        _mockOwnerPropertyRepo.Setup(r => r.GetByIdAsync(propertyId, landlordId))
                              .ReturnsAsync((Property)null);

        // Act & Assert
        var exception = await Assert.ThrowsExceptionAsync<Exception>(() =>
            _ownerPropertyService.UpdatePropertyAsync(dto, landlordId, propertyId));
        Assert.AreEqual("Property not found or not owned by landlord.", exception.Message);
        _mockOwnerPropertyRepo.Verify(r => r.UpdateAsync(It.IsAny<Property>()), Times.Never);
    }
}
