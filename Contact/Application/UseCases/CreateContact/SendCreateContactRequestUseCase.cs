using Application.UseCases.CreateContact.Common;
using Application.UseCases.CreateContact.Interfaces;
using CrossCutting.Extensions;
using CrossCutting.Utils;
using Domain.Repositories.Relational;
using Domain.Entities;
using Infra.Services.Messages;
using System.Text.Json;
using ErrorOr;

namespace Application.UseCases.CreateContact;

public class SendCreateContactRequestUseCase(
    IRabbitMqProducerService rabbitMqService,
    IContactRepository contactRepository) : ISendCreateContactRequestUseCase
{
    private readonly IRabbitMqProducerService _rabbitMqService = rabbitMqService;
    private readonly IContactRepository _contactRepository = contactRepository;

    public async Task<ErrorOr<CreateContactResponse>> Execute(CreateContactRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = new CreateContactRequestValidator().Validate(request);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
        }

        var alreadyExists = await _contactRepository.Exists(request.AreaCode, request.Phone, cancellationToken);
        if (!alreadyExists) 
        {
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

        return Error.Validation("Validation", "Telefone informado já está cadastrado.");
    }
}
