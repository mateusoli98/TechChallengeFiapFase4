using Application.UseCases.GetContact;
using Application.UseCases.SearchContact;
using Domain.DomainObjects.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ReadAPI.Controllers.v1;

[Route("contact")]
public class ContactController(
    ISearchContactUseCase searchContact,
    IGetContactUseCase getContact) : BaseController
{
    private readonly IGetContactUseCase _getContact = getContact;
    private readonly ISearchContactUseCase _searchContacts = searchContact;



    /// <summary>
    /// Endpoint responsável por recuperar um contato pelo seu Id
    /// </summary>
    /// <param name="id">Identificação do contato</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Informações do contato</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(200, Type = typeof(GetContactResponse))]
    [ProducesResponseType(404, Type = typeof(void))]
    public async Task<ActionResult<GetContactResponse>> GetContact([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        return await Result(_getContact.Execute(id, cancellationToken));
    }

    /// <summary>
    /// Endpoint responsável por listar contatos cadastrados podendo usar filtros
    /// </summary>
    /// <param name="filter">Valores para serem usados como filtro</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Informações do contato</returns>
    [HttpGet("search")]
    [ProducesResponseType(200, Type = typeof(PaginationResult<SearchContactResponse>))]
    [ProducesResponseType(400, Type = typeof(object))]
    public async Task<ActionResult<PaginationResult<SearchContactResponse>>> SearchContacts([FromQuery] ContactFilter filter, CancellationToken cancellationToken = default)
    {
        return await Result(_searchContacts.Execute(filter, cancellationToken));
    }
}
