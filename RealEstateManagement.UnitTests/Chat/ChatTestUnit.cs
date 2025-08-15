using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RealEstateManagement.Business.DTO.Chat;

using RealEstateManagement.Business.Repositories.Chat.Conversations;
using RealEstateManagement.Business.Services.Chat.Conversations;

using System;
using System.Threading.Tasks;


using RealEstateManagement.Data.Entity.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateManagement.Data.Entity.User;

[TestClass]
public class ChatTestUnit
{
    private Mock<IConversationRepository> _mockRepository;
    private ConversationService _conversationService;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IConversationRepository>();
        _conversationService = new ConversationService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task CreateConversationAsync_ExistingConversation_ReturnsExistingConversation()
    {
        // Arrange
        Random random = new Random();
        var createConversationDto = new CreateConversationDTO
        {
            RenterId = random.Next(),
            LandlordId = random.Next(),
            PropertyId = random.Next()
        };

        var existingConversation = new Conversation
        {
            Id = random.Next(),
            RenterId = createConversationDto.RenterId,
            LandlordId = createConversationDto.LandlordId,
            PropertyId = createConversationDto.PropertyId
        };

        // Mock the repository to return an existing conversation
        _mockRepository.Setup(r => r.GetByUsersAsync(
            createConversationDto.RenterId,
            createConversationDto.LandlordId,
            createConversationDto.PropertyId))
            .ReturnsAsync(existingConversation);

        // Act
        var result = await _conversationService.CreateConversationAsync(createConversationDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(existingConversation.Id, result.Id);
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Conversation>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateConversationAsync_NewConversation_CreatesAndReturnsNewConversation()
    {
        // Arrange
        Random random = new Random();
        var createConversationDto = new CreateConversationDTO
        {
            RenterId = random.Next(),
            LandlordId = random.Next(),
            PropertyId = random.Next()
        };

        var newConversation = new Conversation
        {
            Id = random.Next(),
            RenterId = createConversationDto.RenterId,
            LandlordId = createConversationDto.LandlordId,
            PropertyId = createConversationDto.PropertyId,
            CreatedAt = DateTime.UtcNow
        };

        // Mock the repository to return null (no existing conversation)
        _mockRepository.Setup(r => r.GetByUsersAsync(
            createConversationDto.RenterId,
            createConversationDto.LandlordId,
            createConversationDto.PropertyId))
            .ReturnsAsync((Conversation)null);

        // Mock the repository to return the newly created conversation
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Conversation>()))
            .ReturnsAsync(newConversation);

        // Act
        var result = await _conversationService.CreateConversationAsync(createConversationDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(newConversation.Id, result.Id);
        _mockRepository.Verify(r => r.CreateAsync(It.Is<Conversation>(c =>
            c.RenterId == createConversationDto.RenterId &&
            c.LandlordId == createConversationDto.LandlordId &&
            c.PropertyId == createConversationDto.PropertyId)), Times.Once);
    }
    [TestMethod]
    public async Task CreateConversationAsync_WhenExisting_ReturnsExisting_AndNoCreate()
    {
        var dto = new CreateConversationDTO { RenterId = 1, LandlordId = 2, PropertyId = 3 };
        var existing = new Conversation { Id = 99, RenterId = 1, LandlordId = 2, PropertyId = 3 };

        _mockRepository.Setup(r => r.GetByUsersAsync(1, 2, 3)).ReturnsAsync(existing);

        var result = await _conversationService.CreateConversationAsync(dto);

        Assert.IsNotNull(result);
        Assert.AreEqual(99, result.Id);
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Conversation>()), Times.Never);
    }

    [TestMethod]
    public async Task CreateConversationAsync_WhenNotExisting_CreatesWithCorrectFields()
    {
        var dto = new CreateConversationDTO { RenterId = 10, LandlordId = 20, PropertyId = 30 };
        _mockRepository.Setup(r => r.GetByUsersAsync(10, 20, 30)).ReturnsAsync((Conversation)null);

        Conversation captured = null!;
        var created = new Conversation { Id = 123, RenterId = 10, LandlordId = 20, PropertyId = 30, CreatedAt = DateTime.UtcNow };

        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Conversation>()))
             .Callback<Conversation>(c => captured = c)
             .ReturnsAsync(created);

        var before = DateTime.UtcNow;
        var result = await _conversationService.CreateConversationAsync(dto);
        var after = DateTime.UtcNow;

        Assert.IsNotNull(result);
        Assert.AreEqual(123, result.Id);

        Assert.IsNotNull(captured);
        Assert.AreEqual(10, captured.RenterId);
        Assert.AreEqual(20, captured.LandlordId);
        Assert.AreEqual(30, captured.PropertyId);
        // CreatedAt được set trong service (gần thời gian thực thi)
        Assert.IsTrue(captured.CreatedAt >= before && captured.CreatedAt <= after);

        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Conversation>()), Times.Once);
    }

    [TestMethod]
    public async Task CreateConversationAsync_mockRepositoryThrows_PropagatesException()
    {
        var dto = new CreateConversationDTO { RenterId = 1, LandlordId = 2, PropertyId = 3 };
        _mockRepository.Setup(r => r.GetByUsersAsync(1, 2, 3)).ReturnsAsync((Conversation)null);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Conversation>()))
             .ThrowsAsync(new InvalidOperationException("db error"));

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _conversationService.CreateConversationAsync(dto));
    }

    // ===== GetAllByUserIdAsync mapping =====

    [TestMethod]
    public async Task GetAllByUserIdAsync_AsLandlord_FillsRenterNameOnly()
    {
        // userId == LandlordId => RenterName hiển thị, LandlordName = null
        var convs = new List<Conversation>
        {
            new Conversation
            {
                Id = 1, RenterId = 10, LandlordId = 99,
                LastMessage = "hi", LastSentAt = DateTime.UtcNow.AddMinutes(-1),
                Renter = new ApplicationUser { Name = "Renter A", Email = "renterA@mail" },
                Landlord = new ApplicationUser { Name = "Landlord Z", Email = "z@mail" }
            }
        };
        _mockRepository.Setup(r => r.GetAllByUserIdAsync(99)).ReturnsAsync(convs);

        var dtos = (await _conversationService.GetAllByUserIdAsync(99)).ToList();

        Assert.AreEqual(1, dtos.Count);
        Assert.AreEqual("Renter A", dtos[0].RenterName);
        Assert.IsNull(dtos[0].LandlordName);
        Assert.AreEqual("hi", dtos[0].LastMessage);
    }

    [TestMethod]
    public async Task GetAllByUserIdAsync_AsRenter_FillsLandlordNameOnly()
    {
        // userId == RenterId => LandlordName hiển thị, RenterName = null
        var convs = new List<Conversation>
        {
            new Conversation
            {
                Id = 2, RenterId = 55, LandlordId = 77,
                LastMessage = "hello", LastSentAt = DateTime.UtcNow.AddMinutes(-2),
                Renter = new ApplicationUser { Name = "Renter B", Email = "b@mail" },
                Landlord = new ApplicationUser { Name = "Landlord C", Email = "c@mail" }
            }
        };
        _mockRepository.Setup(r => r.GetAllByUserIdAsync(55)).ReturnsAsync(convs);

        var dtos = (await _conversationService.GetAllByUserIdAsync(55)).ToList();

        Assert.AreEqual(1, dtos.Count);
        Assert.AreEqual("Landlord C", dtos[0].LandlordName);
        Assert.IsNull(dtos[0].RenterName);
        Assert.AreEqual("hello", dtos[0].LastMessage);
    }

    [TestMethod]
    public async Task GetAllByUserIdAsync_AsOtherUser_FillsNoNames()
    {
        // userId không trùng renter/landlord => cả 2 name null
        var convs = new List<Conversation>
        {
            new Conversation
            {
                Id = 3, RenterId = 1, LandlordId = 2,
                Renter = new ApplicationUser { Name = "A", Email = "a@mail" },
                Landlord = new ApplicationUser { Name = "B", Email = "b@mail" }
            }
        };
        _mockRepository.Setup(r => r.GetAllByUserIdAsync(999)).ReturnsAsync(convs);

        var dtos = (await _conversationService.GetAllByUserIdAsync(999)).ToList();

        Assert.AreEqual(1, dtos.Count);
        Assert.IsNull(dtos[0].RenterName);
        Assert.IsNull(dtos[0].LandlordName);
    }

    // ===== FilterConversationAsync =====

    [TestMethod]
    public async Task FilterConversationAsync_MapsBasicFields_AndPassesParameters()
    {
        var userId = 5;
        var term = "rent";
        var skip = 10;
        var take = 5;

        var convs = new List<Conversation>
        {
            new Conversation
            {
                Id = 100, PropertyId = 200, RenterId = 5, LandlordId = 7,
                Renter = new ApplicationUser { Name = "Renter X" },
                Landlord = new ApplicationUser { Name = "Land Y" }
            },
            new Conversation
            {
                Id = 101, PropertyId = 201, RenterId = 6, LandlordId = 5,
                Renter = new ApplicationUser { Name = "Renter Z" },
                Landlord = new ApplicationUser { Name = "Land W" }
            }
        };

        _mockRepository.Setup(r => r.FilterConversationAsync(userId, term, skip, take))
             .ReturnsAsync(convs);

        var result = (await _conversationService.FilterConversationAsync(userId, term, skip, take)).ToList();

        // verify mapping
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(100, result[0]!.Id);
        Assert.AreEqual(200, result[0]!.PropertyId);
        Assert.AreEqual(5, result[0]!.RenterId);
        Assert.AreEqual(7, result[0]!.LandlordId);
        Assert.AreEqual("Renter X", result[0]!.RenterName);
        Assert.AreEqual("Land Y", result[0]!.LandlordName);

        // verify repo called with correct params
        _mockRepository.Verify(r => r.FilterConversationAsync(userId, term, skip, take), Times.Once);
    }

    [TestMethod]
    public async Task FilterConversationAsync_mockRepositoryThrows_PropagatesException()
    {
        _mockRepository.Setup(r => r.FilterConversationAsync(1, "x", 0, 5))
             .ThrowsAsync(new InvalidOperationException("db err"));

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(
            () => _conversationService.FilterConversationAsync(1, "x", 0, 5));
    }
}