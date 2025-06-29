using Microsoft.AspNetCore.SignalR;
using Nest;

namespace RealEstateManagement.API.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinConversation(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task SendMessage(string conversationId, string senderId, string content)
        {
            var message = new
            {
                SenderId = senderId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            await Clients.Group(conversationId).SendAsync("ReceiveMessage", message);
        }
    }


}
