using Application.UseCases.GetContact;
using Application.UseCases.UpdateContact.Common;
using Application.UseCases.UpdateContact.Interfaces;
using CrossCutting.Extensions;
using CrossCutting.Utils;
using System.Text.Json;
using ErrorOr;
using Infra.Services.Messages;

namespace Application.UseCases.UpdateContact;

public class SendUpdateContactRequestUseCase(IGetContactUseCase getContactUseCase, IRabbitMqProducerService rabbitMqService) : ISendUpdateContactRequestUseCase
{
    private readonly IGetContactUseCase _getContactUse = getContactUseCase;
    private readonly IRabbitMqProducerService _rabbitMqService = rabbitMqService;

    public async Task<ErrorOr<UpdateContactResponse>> Execute(string contactId, UpdateContactRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = new UpdateContactRequestValidator().Validate(request);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrorList();
        }

        var contact = await _getContactUse.Execute(contactId, cancellationToken);

        if (!contact.IsError)
        {
            contact.Value.Id = contactId;
            contact.Value.Name = request.Name;
            contact.Value.AreaCode = request.AreaCode;
            contact.Value.Phone = request.Phone;
            contact.Value.Email = request.Email;
            contact.Value.State = AreaCodeDictionary.GetStateByAreaCode(request.AreaCode);


            _rabbitMqService.SendMessage(JsonSerializer.Serialize(contact.Value), "update_contact");

            return new UpdateContactResponse
            {
                Message = $"Solicitação de alteração de contato com Id {contactId} realizado com sucesso."
            };
        }

        return Error.NotFound(description: $"Contato com id: {contactId} não encontrado. Revise o Id informado ou tente novamente mais tarde");
    }
}
