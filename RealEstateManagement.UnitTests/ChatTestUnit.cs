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
}