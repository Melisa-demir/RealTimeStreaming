using Microsoft.AspNetCore.SignalR;

namespace StreamingService.Hubs
{
    public class StreamingHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}