using Application.UseCases.UpdateContact.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Domain.Entities;

namespace UpdateWorker
{
    public class Worker(IUpdateContactProcessingUseCase usecase) : IHostedService
    {
        private readonly IUpdateContactProcessingUseCase _useCase = usecase;
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

            _channel.QueueDeclare(queue: "update_contact",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                InitializeRabbitMQListener();

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var contact = JsonSerializer.Deserialize<Contact>(message);
                    if (contact != null)
                    {
                        contact.IsEnabled = true;
                        _useCase.Execute(contact, cancellationToken);
                    }
                };

                _channel.BasicConsume(queue: "update_contact",
                                      autoAck: true,
                                      consumer: consumer);
            }
            catch (Exception ex)
            {
                //Logar erro
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Close();
            _connection.Close();
            return Task.CompletedTask;
        }
    }
}
