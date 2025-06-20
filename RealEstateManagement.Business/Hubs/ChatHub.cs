

namespace RealEstateManagement.Business.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;
    public class ChatHub : Hub
    {
        public async Task SendMessageToGroup(string conversationId, string senderName, string content)
        {
            await Clients.Group(conversationId).SendAsync("ReceiveMessage", senderName, content);
        }

        public async Task JoinGroup(string conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }

        public async Task LeaveGroup(string conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
        }
    }

}
