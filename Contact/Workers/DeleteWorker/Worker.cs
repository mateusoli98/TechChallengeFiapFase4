using Application.UseCases.DeleteContact.Interfaces;
using Application.UseCases.DeleteContactPermanently.Interfaces;
using Domain.Entities;
using Infra.Services.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DeleteWorker;

public class Worker(
    //ILogger<Worker> logger,
    IServiceScopeFactory scopeFactory,
    IDeleteContactProcessingUseCase deleteContactUsecase,
    IDeleteContactPermanentlyProcessingUseCase deleteContactPermanentlyProcessingUsecase,
    IRabbitMqProducerService rabbitMqProducerService) : IHostedService
{
    //private readonly ILogger<Worker> _logger;
    private readonly IDeleteContactProcessingUseCase _deleteContactProcessingUseCase = deleteContactUsecase;
    private readonly IDeleteContactPermanentlyProcessingUseCase _deleteContactPermanentlyUsecase = deleteContactPermanentlyProcessingUsecase;
    private readonly IRabbitMqProducerService _rabbitMqProducerService = rabbitMqProducerService;
    private IModel _channel;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        (_, _channel) = _rabbitMqProducerService.GetConnectionAndChannel();
        _rabbitMqProducerService.DeclareQueue("delete_contact");
        _rabbitMqProducerService.DeclareQueue("delete_permanently_contact");

        var deleteContactConsumer = new EventingBasicConsumer(_channel);
        deleteContactConsumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var contactId = JsonSerializer.Deserialize<string>(message);
            Console.WriteLine($"Iniciando delecao logica do usuario '{contactId}'");

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
            Console.WriteLine($"Iniciando delecao permanente do usuario '{contactId}'");

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
        _rabbitMqProducerService.Dispose();
        return Task.CompletedTask;
    }
}
