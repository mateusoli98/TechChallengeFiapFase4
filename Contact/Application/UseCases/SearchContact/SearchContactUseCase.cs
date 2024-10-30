using Domain.DomainObjects.Filters;
using Domain.Repositories.Relational;
using ErrorOr;

namespace Application.UseCases.SearchContact;

public class SearchContactUseCase(IContactRepository contactRepository) : ISearchContactUseCase
{
    private readonly IContactRepository _contactRepository = contactRepository;

    public async Task<ErrorOr<PaginationResult<SearchContactResponse>>> Execute(ContactFilter filter, CancellationToken cancellationToken = default)
    {
        var result = await _contactRepository.SearchAsync(filter, cancellationToken);

        return new PaginationResult<SearchContactResponse>(result.Total, result.Items.Select(c => SearchContactResponse.Create(c)));
    }
}
