namespace Infra.Services.Messages
{
    public interface IRabbitMqProducerService
    {
        void SendMessage(string message, string queueName);
    }
}
