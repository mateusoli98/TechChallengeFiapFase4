using Application.UseCases.DeleteContactPermanently.Interfaces;
using Application.UseCases.GetContact;
using ErrorOr;
using System.Text.Json;

namespace Application.UseCases.DeleteContactPermanently;

public class SendDeleteContactPermanentlyRequestUseCase(IGetContactUseCase getContactUseCase, IRabbitMqProducerService rabbitMqService) : ISendDeleteContactPermanentlyRequestUseCase
{
    private readonly IGetContactUseCase _getContactUseCase = getContactUseCase;
    private readonly IRabbitMqProducerService _rabbitMqService = rabbitMqService;

    public async Task<Error?> Execute(string id, CancellationToken cancellationToken)
    {       
        var contact = await _getContactUseCase.Execute(id, cancellationToken);

        if (!contact.IsError)
        {
            _rabbitMqService.SendMessage(JsonSerializer.Serialize(contact.Value.Id), "delete_permanently_contact");          
        }

        return Error.NotFound($"Contato com Id {id} não encontrado. Revise o Id informado ou tente novamente mais tarde");
    }
}
