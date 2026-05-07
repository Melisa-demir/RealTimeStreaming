using Microsoft.AspNetCore.SignalR;
using SharedLibrary;

namespace StreamingService.Hubs
{
    public class StreamingHub : Hub
    {
        private readonly RabbitMqHelper _rabbitMqHelper;

        public StreamingHub(RabbitMqHelper rabbitMqHelper)
        {
            _rabbitMqHelper = rabbitMqHelper;
        }

        public async Task SendMessage(string user, string message)
        {
            var fullMessage = $"{user}: {message}";

            _rabbitMqHelper.PublishMessage(
                "notification-queue",
                fullMessage
            );

            await Task.CompletedTask;
        }
    }
}