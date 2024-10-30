using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Application.UseCases.DeleteContact.Interfaces;
using Application.UseCases.DeleteContactPermanently.Interfaces;

namespace DeleteWorker
{
    public class Worker(
        //ILogger<Worker> logger,
        IServiceScopeFactory scopeFactory,
        IDeleteContactProcessingUseCase deleteContactUsecase,
        IDeleteContactPermanentlyProcessingUseCase deleteContactPermanentlyProcessingUsecase) : IHostedService
    {
        //private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IDeleteContactProcessingUseCase _deleteContactProcessingUseCase = deleteContactUsecase;
        private readonly IDeleteContactPermanentlyProcessingUseCase _deleteContactPermanentlyUsecase = deleteContactPermanentlyProcessingUsecase;
        private IConnection _connection;
        private IModel _channel;

        private void InitializeRabbitMQListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = "host.docker.internal",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "delete_contact",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            _channel.QueueDeclare(queue: "delete_permanently_contact",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            InitializeRabbitMQListener();

            var deleteContactConsumer = new EventingBasicConsumer(_channel);
            deleteContactConsumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var contactId = JsonSerializer.Deserialize<string>(message);

                if (!string.IsNullOrWhiteSpace(contactId))
                {
                    //_logger.LogInformation("Processing delete_contact for contactId: {contactId}", contactId);
                    _deleteContactProcessingUseCase.Execute(contactId);
                }
            };
            _channel.BasicConsume(queue: "delete_contact", autoAck: true, consumer: deleteContactConsumer);

            var deletePermanentlyContactConsumer = new EventingBasicConsumer(_channel);
            deletePermanentlyContactConsumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var contactId = JsonSerializer.Deserialize<string>(message);

                if (!string.IsNullOrWhiteSpace(contactId))
                {
                    //_logger.LogInformation("Processing delete_permanently_contact for contactId: {contactId}", contactId);
                    _deleteContactPermanentlyUsecase.Execute(contactId);
                }
            };
            _channel.BasicConsume(queue: "delete_permanently_contact", autoAck: true, consumer: deletePermanentlyContactConsumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();
            return Task.CompletedTask;
        }
    }
}
