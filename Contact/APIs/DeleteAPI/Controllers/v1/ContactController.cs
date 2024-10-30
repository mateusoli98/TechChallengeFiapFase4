using Application.UseCases.DeleteContact.Interfaces;
using Application.UseCases.DeleteContactPermanently.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeleteAPI.Controllers.v1;

[Route("contact")]
public class ContactController(ISendDeleteContactRequestUseCase deleteContact, ISendDeleteContactPermanentlyRequestUseCase deleteContactPermanently) : BaseController
{
    private readonly ISendDeleteContactRequestUseCase _deleteContact = deleteContact;
    private readonly ISendDeleteContactPermanentlyRequestUseCase _deleteContactPermanently = deleteContactPermanently;

    /// <summary>
    /// Endpoint responsável por realizar soft delete no contato
    /// </summary>
    /// <param name="id">Identificador unico do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = typeof(object))]
    public async Task<ActionResult> DeleteContact([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        return await Result(_deleteContact.Execute(id, cancellationToken));
    }

    /// <summary>
    /// Endpoint responsável por realizar hard delete no contato
    /// </summary>
    /// <param name="id">Identificador unico do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns></returns>
    [HttpDelete("{id}/permanently")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = typeof(object))]
    public async Task<ActionResult> DeleteContactPermanently([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        return await Result(_deleteContactPermanently.Execute(id, cancellationToken));
    }
}
