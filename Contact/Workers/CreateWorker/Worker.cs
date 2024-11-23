using Application.UseCases.CreateContact.Interfaces;
using Domain.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CreateWorker
{
    public class Worker(ICreateContactProcessingUseCase usecase) : IHostedService
    {
        private readonly ICreateContactProcessingUseCase _useCase = usecase;
        private IConnection _connection;
        private IModel _channel;

        private void InitializeRabbitMQListener()
        {
            Console.WriteLine("Iniciando conexão com RabbitMQ");
            var factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
                Port = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT")),
                UserName = Environment.GetEnvironmentVariable("RABBITMQ_USER"),
                Password = Environment.GetEnvironmentVariable("RABBITMQ_PASS")
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
                      
            _channel.QueueDeclare(queue: "create_contact",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            InitializeRabbitMQListener();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var contact = JsonSerializer.Deserialize<Contact>(message);
                Console.WriteLine($"Iniciando processamento do usuário '{contact.Id}'");

                if (contact != null)
                {
                    _useCase.Execute(contact);                   
                }
            };

            _channel.BasicConsume(queue: "create_contact",
                                  autoAck: true,
                                  consumer: consumer);

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
