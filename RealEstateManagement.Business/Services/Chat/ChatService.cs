using Microsoft.AspNetCore.SignalR;
using RealEstateManagement.Business.DTO.Chat;

using RealEstateManagement.Business.Repositories.Chat;

namespace RealEstateManagement.Business.Services.Chat
{
    public class ChatService : IChatService
    {
        //private readonly IChatRepository _repository;
        //private readonly IHubContext<ChatHub> _hubContext;

        //public ChatService(IChatRepository repository, IHubContext<ChatHub> hubContext)
        //{
        //    _repository = repository;
        //    _hubContext = hubContext;
        //}

        //public async Task<MessageDTO> SendMessageAsync(SendMessageDTO dto)
        //{
        //    var message = new Data.Entity.Messages.Message
        //    {
        //        ConversationId = dto.ConversationId,
        //        SenderId = dto.SenderId,
        //        Content = dto.Content,
        //        IsRead = false,
        //        NotificationSent = false,
        //        SentAt = DateTime.UtcNow
        //    };

        //    var savedMessage = await _repository.SaveMessageAsync(message);

        //    var result = new MessageDTO
        //    {
        //        Id = savedMessage.Id,
        //        SenderName = savedMessage.Sender?.Name ?? "Unknown",
        //        Content = savedMessage.Content,
        //        SentAt = savedMessage.SentAt
        //    };

        //    await _hubContext.Clients.Group(dto.ConversationId.ToString())
        //        .SendAsync("ReceiveMessage", result.SenderName, result.Content);

        //    return result;
        //}

        //public async Task<List<MessageDTO>> GetMessagesAsync(int conversationId)
        //{
        //    var messages = await _repository.GetMessagesByConversationIdAsync(conversationId);
        //    return messages.Select(m => new MessageDTO
        //    {
        //        Id = m.Id,
        //        SenderName = m.Sender?.Name ?? "Unknown",
        //        Content = m.Content,
        //        SentAt = m.SentAt
        //    }).ToList();
        //}
    }


}