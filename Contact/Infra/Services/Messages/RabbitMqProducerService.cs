using RabbitMQ.Client;
using System.Text;

namespace Infra.Services.Messages;

public class RabbitMqProducerService() : IRabbitMqProducerService
{       
    public void SendMessage(string message, string queueName)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "host.docker.internal",
            UserName = "guest",
            Password = "guest",
        };

        using (var connection = factory.CreateConnection())
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: queueName,
                    basicProperties: null,
                    body: body
                );
            }
        };
    }
}
