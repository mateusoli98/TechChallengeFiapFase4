using Domain.DomainObjects.Filters;
using Domain.Entities;

namespace Domain.Repositories.Relational;

public interface IContactRepository
{
    Task<string> SaveAsync(Contact contact, CancellationToken cancellationToken = default);
    Task UpdateAsync(Contact contact, CancellationToken cancellationToken = default);
    Task<Contact?> GetByIdAsync(string id, CancellationToken cancellationToken = default, bool isEnabled = true);
    Task<PaginationResult<Contact>> SearchAsync(ContactFilter filter, CancellationToken cancellationToken = default);
    Task DeleteAsync(Contact contact, CancellationToken cancellationToken = default);
    Task PermanentDelete(Contact contact, CancellationToken cancellationToken = default);
}
