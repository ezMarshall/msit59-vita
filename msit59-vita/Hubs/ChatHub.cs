using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        //後台訂單管理 >> 前台當前訂單+通知
        public async Task SwitchOrderStatus(string CustomerId, string OrderId, string CustomerOrderStatus)
        {
            await Clients.All.SendAsync("StatusChange", CustomerId, OrderId, CustomerOrderStatus);
        }
    }
}