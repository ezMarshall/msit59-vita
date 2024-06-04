using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        public async Task SwitchOrderStatus(int CustomerId, int OrderId, string CustomerOrderStatus)
        {
            await Clients.All.SendAsync("StatusChange", CustomerId, OrderId, CustomerOrderStatus);
        }
    }
}