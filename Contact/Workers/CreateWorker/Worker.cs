using Application.UseCases.CreateContact.Interfaces;
using Infra.Services.Messages;
using RabbitMQ.Client.Events;
using Domain.Entities;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CreateWorker
{
    public class Worker(ICreateContactProcessingUseCase usecase, IRabbitMqProducerService rabbitMqProducerService) : IHostedService
    {
        private readonly ICreateContactProcessingUseCase _useCase = usecase;
        private readonly IRabbitMqProducerService _rabbitMqProducerService = rabbitMqProducerService;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            (_, var _channel) = _rabbitMqProducerService.GetConnectionAndChannel();
            _rabbitMqProducerService.DeclareQueue("create_contact");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var contact = JsonSerializer.Deserialize<Contact>(message) ?? throw new Exception("Erro ao deserializar mensagem");

                Console.WriteLine($"Iniciando processamento do usuário '{contact?.Id}'");
                _useCase.Execute(contact!);
            };

            _channel.BasicConsume(queue: "create_contact", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _rabbitMqProducerService.Dispose();
            return Task.CompletedTask;
        }
    }
}
