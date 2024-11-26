using RabbitMQ.Client;
using System.Text;

namespace Infra.Services.Messages;

public class RabbitMqProducerService : IRabbitMqProducerService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqProducerService()
    {
        var factory = new ConnectionFactory()
        {
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
            Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "80"),
            UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER"),
            Password = Environment.GetEnvironmentVariable("RABBITMQ_PASS")
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void SendMessage(string message, string queueName)
    {
        DeclareQueue(queueName);

        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body
        );
    }

    public void DeclareQueue(string queueName)
    {
        _channel.QueueDeclare(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
    }

    public (IConnection, IModel) GetConnectionAndChannel()
    {
        return (_connection, _channel);
    }

    public void Dispose()
    {
        _channel.Close();
        _connection.Close();
    }
}
