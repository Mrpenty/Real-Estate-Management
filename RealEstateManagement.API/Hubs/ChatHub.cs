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

        public async Task SendMessage(string conversationId, string senderId, string message)
        {
            // Gửi đến tất cả các client trong cùng cuộc hội thoại
            await Clients.Group(conversationId).SendAsync("ReceiveMessage", senderId, message);
        }
    }


}
