using Application.UseCases.CreateContact.Common;
using Application.UseCases.CreateContact.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace CreateAPI.Controllers.v1;

[Route("contact")]
public class ContactController(ISendCreateContactRequestUseCase createContact) : BaseController
{
    private readonly ISendCreateContactRequestUseCase _createContact = createContact;

    /// <summary>
    /// Endpoint responsável por criar um novo contato
    /// </summary>
    /// <param name="request">Informações do novo contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Id do contato criado</returns>
    [HttpPost]
    [ProducesResponseType(202, Type = typeof(CreateContactResponse))]
    [ProducesResponseType(400, Type = typeof(object))]
    [ProducesResponseType(404, Type = typeof(void))]
    public async Task<ActionResult<CreateContactResponse>> CreateContact([FromBody] CreateContactRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _createContact.Execute(request, cancellationToken);
        return result.Match<ActionResult>(
            success => Ok(success),
            errors => BadRequest(new { message = errors })
        );
    }
}
