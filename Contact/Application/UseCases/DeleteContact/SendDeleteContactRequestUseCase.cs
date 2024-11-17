using Application.UseCases.DeleteContact.Interfaces;
using Application.UseCases.GetContact;
using Infra.Services.Messages;
using System.Text.Json;
using ErrorOr;

namespace Application.UseCases.DeleteContact;

public class SendDeleteContactRequestUseCase(IGetContactUseCase getContactUseCase, IRabbitMqProducerService rabbitMqService) : ISendDeleteContactRequestUseCase
{
    private readonly IGetContactUseCase _getContactUseCase = getContactUseCase;
    private readonly IRabbitMqProducerService _rabbitMqService = rabbitMqService;

    public async Task<Error?> Execute(string id, CancellationToken cancellationToken = default)
    {
        var contact = await _getContactUseCase.Execute(id, cancellationToken);

        if (!contact.IsError)
        {
            _rabbitMqService.SendMessage(JsonSerializer.Serialize(contact.Value.Id), "delete_contact");
            return null;
        }

        return Error.NotFound($"Contato com Id {id} não encontrado. Revise o Id informado ou tente novamente mais tarde");
    }
}
