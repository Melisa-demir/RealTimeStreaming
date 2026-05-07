using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StreamingService.Hubs;
using System.Text;

namespace StreamingService.Consumers
{
    public class RabbitMqConsumer : BackgroundService
    {
        private readonly IHubContext<StreamingHub> _hubContext;

        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqConsumer(IHubContext<StreamingHub> hubContext)
        {
            _hubContext = hubContext;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "notification-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();

                var message = Encoding.UTF8.GetString(body);

                await _hubContext.Clients.All.SendAsync(
                    "ReceiveMessage",
                    "RabbitMQ",
                    message
                );
            };

            _channel.BasicConsume(
                queue: "notification-queue",
                autoAck: true,
                consumer: consumer
            );

            return Task.CompletedTask;
        }
    }
}