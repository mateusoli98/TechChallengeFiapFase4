namespace Application
{
    public interface IRabbitMqProducerService
    {
        void SendMessage(string message, string queueName);
    }
}
