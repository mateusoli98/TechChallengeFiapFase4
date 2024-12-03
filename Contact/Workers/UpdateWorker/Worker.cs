using Application.UseCases.UpdateContact.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Domain.Entities;
using Infra.Services.Messages;

namespace UpdateWorker
{
    public class Worker(IUpdateContactProcessingUseCase usecase, IRabbitMqProducerService rabbitMqProducerService) : IHostedService
    {
        private readonly IUpdateContactProcessingUseCase _useCase = usecase;
        private readonly IRabbitMqProducerService _rabbitMqProducerService = rabbitMqProducerService;
        private IModel _channel;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                (_, _channel) = _rabbitMqProducerService.GetConnectionAndChannel();
                _rabbitMqProducerService.DeclareQueue("update_contact");

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var contact = JsonSerializer.Deserialize<Contact>(message) ?? throw new Exception("Erro ao deserializar mensagem") ;
                    
                    Console.WriteLine($"Iniciando atualizacao do usuario '{contact.Id}'");
                    contact.IsEnabled = true;
                   await _useCase.Execute(contact, cancellationToken);
                };

                _channel.BasicConsume(queue: "update_contact", autoAck: true, consumer: consumer);
            }
            catch (Exception ex)
            {
                //Logar erro
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _rabbitMqProducerService.Dispose();
            return Task.CompletedTask;
        }
    }
}
