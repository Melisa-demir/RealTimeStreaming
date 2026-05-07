using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace SharedLibrary
{
    public class RabbitMqHelper
    {
        private readonly IConnection? _connection;
        private readonly IModel _channel;

        public RabbitMqHelper(string hostname, string username, string password)
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostname,
                UserName = username,
                Password = password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void PublishMessage(string queueName, string message)
        {
            Console.WriteLine($"RabbitMQ Publish başladı. Queue: {queueName}, Message: {message}");

            _channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: null,
                body: body
            );

            Console.WriteLine("RabbitMQ Publish tamamlandı.");
        }
    }
}

