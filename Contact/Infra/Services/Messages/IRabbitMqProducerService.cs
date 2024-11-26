using RabbitMQ.Client;

namespace Infra.Services.Messages
{
    public interface IRabbitMqProducerService
    {
        public void SendMessage(string message, string queueName);
        public void DeclareQueue(string queueName);
        public (IConnection, IModel) GetConnectionAndChannel();
        public void Dispose();
    }
}
