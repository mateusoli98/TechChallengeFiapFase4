using Application.UseCases.CreateContact.Common;
using Application.UseCases.CreateContact.Interfaces;
using CrossCutting.Extensions;
using CrossCutting.Utils;
using Domain.Entities;
using ErrorOr;
using System.Text.Json;

namespace Application.UseCases.CreateContact;

public class SendCreateContactRequestUseCase(IRabbitMqProducerService rabbitMqService) : ISendCreateContactRequestUseCase
{
    private readonly IRabbitMqProducerService _rabbitMqService = rabbitMqService;

    public async Task<ErrorOr<CreateContactResponse>> Execute(CreateContactRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = new CreateContactRequestValidator().Validate(request);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
        }

        var contact = new Contact()
        {
            Name = request.Name,
            AreaCode = request.AreaCode,
            Email = request.Email,
            Phone = request.Phone,
            State = AreaCodeDictionary.GetStateByAreaCode(request.AreaCode),
            CreatedAt = DateTime.UtcNow,
            IsEnabled = true
        };

        _rabbitMqService.SendMessage(JsonSerializer.Serialize(contact), "create_contact");

        return new CreateContactResponse()
        {
            Id = contact.Id,
            Message = $"Solicitação de criação de contato com id {contact.Id} enviada com sucesso"
        };
    }
}
