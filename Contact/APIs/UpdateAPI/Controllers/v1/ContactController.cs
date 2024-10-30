using Application.UseCases.UpdateContact.Common;
using Application.UseCases.UpdateContact.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UpdateAPI.Controllers.v1;

[Route("contact")]
public class ContactController(ISendUpdateContactRequestUseCase updateContact) : BaseController
{
    private readonly ISendUpdateContactRequestUseCase _updateContact = updateContact;

    /// <summary>
    /// Endpoint responsável por atualizar um contato existente
    /// </summary>
    /// <param name="id">Identificador unico do contato</param>
    /// <param name="request">Novos valores do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Informações atualizadas do contato</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(200, Type = typeof(UpdateContactResponse))]
    [ProducesResponseType(400, Type = typeof(object))]
    [ProducesResponseType(404, Type = typeof(void))]
    public async Task<ActionResult<UpdateContactResponse>> UpdateContact([FromRoute] string id, [FromBody] UpdateContactRequest request, CancellationToken cancellationToken = default)
    {
        return await Result(_updateContact.Execute(id, request, cancellationToken));
    }
}
