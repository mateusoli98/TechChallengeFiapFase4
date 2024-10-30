using Domain.DomainObjects.Filters;
using ErrorOr;

namespace Application.UseCases.SearchContact;

public interface ISearchContactUseCase
{
    Task<ErrorOr<PaginationResult<SearchContactResponse>>> Execute(ContactFilter filter, CancellationToken cancellationToken = default);
}
